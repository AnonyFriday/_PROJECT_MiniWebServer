using WebServer.SDK;

namespace WebServer.Server.Readers;

public interface IRequestReader
{
    public Task<WRequest> ReadRequestAsync(CancellationToken cancellationTokenSource);
}