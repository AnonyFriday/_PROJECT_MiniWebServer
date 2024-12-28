using System.Net;
using System.Net.Sockets;
using System.Text;
using WebServer.Middleware.Authentication;
using WebServer.Middleware.StaticContent;
using WebServer.SDK;
using WebServer.SDK.Middlewares;
using WebServer.SDK.RequestReaders;
using WebServer.SDK.Requests;
using WebServer.SDK.Responses;
using WebServer.SDK.Responses.BodyWriters;
using WebServer.SDK.ResponseWriters;
using WebServer.Server.Middlewares;
using WebServer.Server.RequestReaders;

namespace WebServer.Server;

public class Worker : BackgroundService
{
    // ===========================
    // === Props & Fields
    // ===========================

    private WebServerOptions _options;
    private readonly ILogger<Worker> _logger;
    private readonly IRequestReaderFactory _requestReaderFactory;
    private readonly IResponseWriterFactory _responseWriterFactory;
    private readonly IMiddlewarePipeline _middlewarePipeline;
    private List<ClientConnection> _clientConnections;

    // ===========================
    // === Constructors
    // ===========================

    public Worker(ILogger<Worker> logger, WebServerOptions options,
        IMiddlewarePipeline middlewarePipeline,
        IRequestReaderFactory requestReaderFactory, IResponseWriterFactory responseWriterFactory)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
        _requestReaderFactory = requestReaderFactory;
        _responseWriterFactory = responseWriterFactory;
        _middlewarePipeline = middlewarePipeline;
        _clientConnections = new List<ClientConnection>();

        // Add Middlewares
        // AuthenticationMiddleware --> NotFoundMidddleware--> NullMiddleware
        _middlewarePipeline.AddMiddleware(new NotFoundMiddleware());
        _middlewarePipeline.AddMiddleware(new StaticContentMiddleware());
        _middlewarePipeline.AddMiddleware(new AuthenticationMiddleware());
    }

    // ===========================
    // === Methods
    // ===========================

    /// <summary>
    /// Execute the Task by the Worker
    /// </summary>
    /// <param name="stoppingToken"></param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Server Configurations
        var serverEndPoint = new IPEndPoint(
            string.IsNullOrEmpty(_options.IpAddress) ? IPAddress.Any : IPAddress.Parse(_options.IpAddress),
            _options.Port
        );

        // TCP protocol
        using var serverSocket = new Socket(serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        // Server binding to the Endpoint
        serverSocket.Bind(serverEndPoint);
        _logger.LogInformation("Listening... (port: {p}, Ip Address: {a})", _options.Port, _options.IpAddress);

        // Listening the requests
        serverSocket.Listen();

        while (!stoppingToken.IsCancellationRequested)
        {
            // Client Socket
            var clientSocket = await serverSocket.AcceptAsync(stoppingToken);

            if (clientSocket != null)
            {
                // Don't use await because it will pause method until handling
                // 1 request return 
                var t = HandleNewClientConnectionAsync(clientSocket, stoppingToken);
                _clientConnections.Add(new ClientConnection()
                {
                    HandlerTask = t
                });
            }
        }

        // If all tasks are finish then closing the server socket
        Task.WaitAll(_clientConnections.Select(con => con.HandlerTask), stoppingToken);
        serverSocket.Close();
    }

    /// <summary>
    /// Handle Request from the Client Socket asynchronously
    /// </summary>
    /// <param name="clientSocket"></param>
    /// <param name="stoppingToken"></param>
    /// <exception cref="NotImplementedException"></exception>
    private async Task HandleNewClientConnectionAsync(Socket clientSocket, CancellationToken stoppingToken)
    {
        try
        {
            // 1. Create 1 cancellation token restricted reading request in 3s
            var readingRequestCancellationTokenSource = new CancellationTokenSource(3000);
            IRequestReader requestReader = _requestReaderFactory.Create(clientSocket);
            IResponseWriter responseWriter = _responseWriterFactory.Create(clientSocket);

            // 2. Create Request Object and Parsing the incoming string request into Request Object
            WRequest request = await requestReader.ReadRequestAsync(CancellationTokenSource
                .CreateLinkedTokenSource(readingRequestCancellationTokenSource.Token, stoppingToken).Token);

            // 3. Create Empty Response Object
            WResponse response = new WResponse();

            // 4. Capture Request and passthrough Middleware Chain 
            // - original Request and Response object will be modified here
            // - after 15s, if haven't finished the processing, then return notfound
            var invokeCancellationTokenSource = new CancellationTokenSource(15000);
            await _middlewarePipeline.InvokeMiddlewarePipelineAsync(new MiddlewareContext
            {
                Request = request,
                Response = response
            }, CancellationTokenSource.CreateLinkedTokenSource(
                invokeCancellationTokenSource.Token, stoppingToken).Token);

            // 5. Send response back to the client
            await responseWriter.SendRespondToClientAsync(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        finally
        {
            // Close the client socket
            clientSocket.Close();
        }
    }
}