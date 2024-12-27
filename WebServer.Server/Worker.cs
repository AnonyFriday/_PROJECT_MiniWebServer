using System.Net;
using System.Net.Sockets;
using System.Text;
using WebServer.SDK;
using WebServer.Server.Readers;

namespace WebServer.Server;

public class Worker : BackgroundService
{
    // ===========================
    // === Props & Fields
    // ===========================

    private WebServerOptions _options;
    private readonly ILogger<Worker> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private List<ClientConnection> _clientConnections;

    // ===========================
    // === Constructors
    // ===========================

    public Worker(ILogger<Worker> logger, ILoggerFactory loggerFactory, WebServerOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
        _loggerFactory = loggerFactory;
        _clientConnections = new List<ClientConnection>();
    }

    // ===========================
    // === Method
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
            IRequestReader requestReader =
                new DefaultRequestReader(_loggerFactory.CreateLogger<DefaultRequestReader>(), clientSocket);

            // Create Request Object and Parsing the incoming string request into Request Object
            WRequest request = await requestReader.ReadRequestAsync(combinedToken);

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
        var textWriterStream = new StreamWriter(byteStream);

        await textWriterStream.WriteLineAsync("HTTP/1.1 200 OK");
        await textWriterStream.WriteLineAsync("Content-Type: text/plain");
        await textWriterStream.WriteLineAsync("");
        await textWriterStream.WriteLineAsync("Hello World!");

        await textWriterStream.FlushAsync();
    }
}