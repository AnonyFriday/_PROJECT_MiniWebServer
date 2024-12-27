using WebServer.SDK;
using WebServer.SDK.Requests;

namespace WebServer.Server.Readers;

public interface IRequestReader
{
    public Task<WRequest> ReadRequestAsync(CancellationToken cancellationTokenSource);
}