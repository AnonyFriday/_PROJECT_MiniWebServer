using WebServer.SDK.Responses.BodyWriters;

namespace WebServer.SDK.Responses;

public class WResponse
{
    public string ProtocolVersion { get; set; } = "HTTP/1.1";
    public WHttpResponseStatusCodes ResponseCode { get; set; } = WHttpResponseStatusCodes.ClientError_NotFound;
    public string ReasonPhrase { get; set; } = string.Empty;
    public int ContentLength { get; set; } = 0;
    public string Connection { get; set; } = "close";
    public string ContentType { get; set; } = "text/html";

    /// <summary>
    /// Body is unique based on the Content Type <br />
    /// Should delegate to the various type of body writer
    /// </summary>
    public IResponseBodyWriter ResponseBodyWriter { get; set; } = NullResponseBodyWriter.Instance;
}