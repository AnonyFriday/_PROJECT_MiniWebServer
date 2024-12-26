using System.Net;
using System.Net.Sockets;
using System.Text;
using WebServer.SDK;
using WebServer.Server.Requests;

namespace WebServer.Server;

public class Worker : BackgroundService
{
    // ===========================
    // === Props & Fields
    // ===========================

    private WebServerOptions _options;
    private readonly ILogger<Worker> _logger;
    private List<ClientConnection> _clientConnections;

    // ===========================
    // === Constructors
    // ===========================

    public Worker(ILogger<Worker> logger, WebServerOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
        _clientConnections = new List<ClientConnection>();
    }

    // ===========================
    // === Methods
    // ===========================

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Server Configurations
        var serverEndPoint = new IPEndPoint(
            string.IsNullOrEmpty(_options.IpAddress) ? IPAddress.Any : IPAddress.Parse(_options.IpAddress),
            _options.Port
        );

        // TCP
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

            // if (_logger.IsEnabled(LogLevel.Information))
            // {
            //     _logger.LogInformation("Server running at: {time}", DateTimeOffset.Now);
            // }
            //
            // await Task.Delay(1000, stoppingToken);
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
            // Create 1 cancellation token restricted reading request in 3s
            var cts = new CancellationTokenSource();
            var combinedToken = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, stoppingToken).Token;

            // Parsing the incoming text into request object
            WRequest request = await ParseRequestAsync(clientSocket, combinedToken);

            // Create Response Object

            // Handle Request  

            // Send back the response
            await SendRespondToClientAsync(clientSocket);
        }
        catch (Exception ex)
        {
        }

        finally
        {
            // Close the client socket
            clientSocket.Close();
        }
    }

    /// <summary>
    /// Sending back respond object to the client
    /// </summary>
    /// <param name="clientSocket"></param>
    private async Task SendRespondToClientAsync(Socket clientSocket)
    {
        var byteStream = new NetworkStream(clientSocket);
        var textWriter = new StreamWriter(byteStream);

        await textWriter.WriteLineAsync("200 OK");
        await textWriter.FlushAsync();
    }

    /// <summary>
    /// Reading Request only cost 3s
    /// </summary>
    /// <param name="clientSocket"></param>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    private async Task<WRequest> ParseRequestAsync(Socket clientSocket, CancellationToken cancellationTokenSource)
    {
        var byteStream = new NetworkStream(clientSocket);
        var textReader = new StreamReader(byteStream, Encoding.UTF8);

        // Instead of checking key and mapping to WRequest for multiple
        // of methods: GET, POST, PUT, DELETE, we create
        var requestLine = await textReader.ReadLineAsync(cancellationTokenSource);
        _logger.LogInformation("{requestLine}", requestLine);


        // in this case, we just read the first line only => GET
        if (RequestHttpHeaderLineParser.TryParse(requestLine, out var requestHttpHeaderLineHeader))
        {
        }

        // Build a request of GET, POST, PUT, DELETE base on Method
        var requestBuilder = new RequestBuilder();
        return requestBuilder.Build();
    }
}