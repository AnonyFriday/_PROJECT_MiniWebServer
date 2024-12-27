using System.Net.Sockets;
using System.Text;
using WebServer.SDK;
using WebServer.Server.Requests;
using WebServer.Server.Requests.Readers;

namespace WebServer.Server.Dtos.Requests.Readers;

public class DefaultRequestReader : IRequestReader
{
    // ===========================
    // === Props & Fields
    // ===========================

    private readonly ILogger<DefaultRequestReader> _logger;
    private readonly Socket _clientSocket;

    // ===========================
    // === Constructors
    // ===========================

    public DefaultRequestReader(ILogger<DefaultRequestReader> logger, Socket clientSocket)
    {
        _logger = logger;
        _clientSocket = clientSocket;
    }

    // ===========================
    // === Methods
    // ===========================

    /// <summary>
    /// Reading technique to read a whole line of header line or request line
    /// using StreamReader 
    /// </summary>
    /// <param name="cancellationTokenSource"></param>
    /// <returns></returns>
    public async Task<WRequest> ReadRequestAsync(CancellationToken cancellationTokenSource)
    {
        var byteStream = new NetworkStream(_clientSocket);
        var textReader = new StreamReader(byteStream, Encoding.UTF8);
        var requestBuilder = new RequestBuilder();

        // Instead of checking key and mapping to WRequest for multiple
        // of methods: GET, POST, PUT, DELETE, we create
        var aLineOfRequestString = await textReader.ReadLineAsync(cancellationTokenSource);

        // Parsing the Request Line 
        if (!string.IsNullOrEmpty(aLineOfRequestString))
        {
            // Logging
            _logger.LogInformation("{requestLine}", aLineOfRequestString);

            // Parsing
            if (RequestLineParser.TryParse(aLineOfRequestString, out var requestLine))
            {
                requestBuilder.AddRequestLine(requestLine);
            }
        }

        // Parsing Header Lines
        aLineOfRequestString = await textReader.ReadLineAsync(cancellationTokenSource);
        while (!string.IsNullOrEmpty(aLineOfRequestString))
        {
            // Logging
            _logger.LogInformation("{headerLine}", aLineOfRequestString);

            // Parsing
            if (HeaderLineParser.TryParse(aLineOfRequestString, out var headerLine))
            {
                requestBuilder.AddHeaderLine(headerLine);
                aLineOfRequestString = await textReader.ReadLineAsync(cancellationTokenSource);
            }
        }

        // Build a request of GET, POST, PUT, DELETE base on Request Line and Header Line
        return requestBuilder.Build();
    }
}