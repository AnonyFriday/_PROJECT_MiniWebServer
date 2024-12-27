using WebServer.SDK;
using WebServer.SDK.Requests;

namespace WebServer.Server.Dtos.Requests;

public class RequestLine
{
    public WMethods Method { get; set; }
    public string UriPath { get; set; } = string.Empty;
    public string ProtocolVersion { get; set; } = string.Empty;
}