using System.Net.Sockets;
using WebServer.SDK.Responses;
using WebServer.SDK.ResponseWriters;

namespace WebServer.Server.ResponseWriters;

public class DefaultResponseWriter : IResponseWriter
{
    // ===========================
    // === Fields & Props
    // ===========================
    private readonly Socket _socket;
    private readonly ILogger<DefaultResponseWriter> _logger;

    // ===========================
    // === Constructors
    // ===========================

    public DefaultResponseWriter(ILogger<DefaultResponseWriter> logger, Socket socket)
    {
        _socket = socket;
        _logger = logger;
    }

    // ===========================
    // === Methods
    // ===========================

    /// <summary>
    /// Sending back respond object to the client
    /// </summary>
    /// <param name="clientSocket"></param>
    public async Task SendRespondToClientAsync(WResponse response)
    {
        var byteStream = new NetworkStream(_socket);
        var textWriterStream = new StreamWriter(byteStream);

        string reasonPhrase = response.ReasonPhrase;
        if (string.IsNullOrEmpty(reasonPhrase))
        {
            reasonPhrase = WHttpResponsePhrases.GetByCode(response.ResponseCode);
        }

        string statusLine = string.Join(" ", response.ProtocolVersion, (int)response.ResponseCode, reasonPhrase);

        await textWriterStream.WriteLineAsync(statusLine);
        await textWriterStream.WriteLineAsync($"Content-Type: {response.ContentType}");
        await textWriterStream.WriteLineAsync($"Content-Length: {response.ContentLength}");
        await textWriterStream.WriteLineAsync($"Connection: {response.Connection}");
        await textWriterStream.WriteLineAsync("");

        // Write down text writer stream into the network stream underlying
        await textWriterStream.FlushAsync();

        // Write the body under the network stream, not the text stream
        if (response.ContentLength > 0)
        {
            await response.ResponseBodyWriter.WriteAsync(byteStream);
        }

        // Sending all bytes to the client
        await byteStream.FlushAsync();
    }
}