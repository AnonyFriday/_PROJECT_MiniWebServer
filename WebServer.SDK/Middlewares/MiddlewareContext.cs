using WebServer.SDK.Requests;
using WebServer.SDK.Responses;

namespace WebServer.SDK.Middlewares;

/// <summary>
/// Processing the Request and Return the Response
/// </summary>
public class MiddlewareContext
{
    public required WRequest Request { get; set; }
    public required WResponse Response { get; set; }
}