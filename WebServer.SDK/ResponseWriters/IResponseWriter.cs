using WebServer.SDK.Responses;

namespace WebServer.SDK.ResponseWriters;

public interface IResponseWriter
{
    public Task SendRespondToClientAsync(WResponse response);
}