using WebServer.SDK.Requests;

namespace WebServer.SDK.RequestReaders;

public interface IRequestReader
{
    public Task<WRequest> ReadRequestAsync(CancellationToken cancellationTokenSource);
}