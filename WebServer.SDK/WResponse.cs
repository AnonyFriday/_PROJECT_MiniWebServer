namespace WebServer.SDK;

public class WResponse
{
    public string ProtocolVersion { get; set; } = "HTTP/1.1";
    public WHttpResponseStatusCodes ResponseCode { get; set; } = WHttpResponseStatusCodes.ClientError_NotFound;
    public string ReasonPhrase { get; set; } = string.Empty;
}