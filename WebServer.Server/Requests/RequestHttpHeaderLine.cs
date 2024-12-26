using WebServer.SDK;

namespace WebServer.Server.Requests;

public class RequestHttpHeaderLine
{
    public WMethods Method { get; set; }
    public string UriPath { get; set; } = string.Empty;
    public string ProtocolVersion { get; set; } = string.Empty;
}