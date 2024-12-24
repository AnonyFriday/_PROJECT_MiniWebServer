using System.Net;
using System.Net.Sockets;

namespace WebServer.Server;

public class Worker : BackgroundService
{
    // ===========================
    // === Props & Fields
    // ===========================

    private WebServerOptions _options;
    private readonly ILogger<Worker> _logger;

    // ===========================
    // === Constructors
    // ===========================

    public Worker(ILogger<Worker> logger, WebServerOptions options, CancellationTokenSource cancellationTokenSource)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
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
                HandleNewClientConnectionAsync(clientSocket, stoppingToken);
            }

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Server running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }

    /// <summary>
    /// Handle Request from the Client Socket asynchronously
    /// </summary>
    /// <param name="clientSocket"></param>
    /// <param name="stoppingToken"></param>
    /// <exception cref="NotImplementedException"></exception>
    private async Task HandleNewClientConnectionAsync(Socket clientSocket, CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}