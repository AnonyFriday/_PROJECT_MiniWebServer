using WebServer.SDK;

namespace WebServer.Server.Requests;

public class RequestBuilder
{
    public WMethods Methods { get; set; } = WMethods.GET;

    public WRequest Build()
    {
        return default;
    }
}