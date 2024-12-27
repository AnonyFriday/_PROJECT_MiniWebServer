using Microsoft.Extensions.Primitives;

namespace WebServer.SDK;

public class WRequest
{
    // HTTP Method (e.g. GET, POST, PUT, DELETE)
    public required WMethods Method { get; set; }
    public required string Uri { get; set; }
    public required string ProtocolVersion { get; set; }
    public required string Host { get; set; }

    // Connection: Keep-alive
    public bool IsKeepAlive { get; set; } = false;

    // 1 header can have 0, 1, or multiple values
    public required Dictionary<string, StringValues> Headers { get; set; } = new Dictionary<string, StringValues>();
}