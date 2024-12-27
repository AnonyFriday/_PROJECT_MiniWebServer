using System.Net.Sockets;

namespace WebServer.SDK.Responses.ResponseWriters;

public interface IResponseWriter
{
    public Task SendRespondToClientAsync(WResponse response);
}