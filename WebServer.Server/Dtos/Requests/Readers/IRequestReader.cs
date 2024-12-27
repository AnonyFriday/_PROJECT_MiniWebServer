using System.Net.Sockets;
using WebServer.SDK;

namespace WebServer.Server.Requests.Readers;

public interface IRequestReader
{
    public Task<WRequest> ReadRequestAsync(CancellationToken cancellationTokenSource);
}