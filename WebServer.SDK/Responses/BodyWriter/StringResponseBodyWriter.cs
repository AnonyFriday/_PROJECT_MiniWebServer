using System.Text;

namespace WebServer.SDK.Responses.BodyWriter;

public class StringResponseBodyWriter : IResponseBodyWriter
{
    // ===========================
    // === Fields & Props
    // ===========================

    private readonly byte[] _contentBytes;

    // ===========================
    // === Constructors
    // ===========================

    /// <summary>
    /// Convert the rawString content into UTF8
    /// </summary>
    /// <param name="rawStringContent"></param>
    public StringResponseBodyWriter(string rawStringContent)
    {
        // Convert into content bytes
        _contentBytes = Encoding.UTF8.GetBytes(rawStringContent);
    }

    // ===========================
    // === Methods
    // ===========================

    /// <summary>
    /// Write a raw string into the byte stream
    /// </summary>
    /// <param name="bodyStream"></param>
    public async Task WriteAsync(Stream bodyStream)
    {
        await bodyStream.WriteAsync(_contentBytes, 0, _contentBytes.Length);
    }
}