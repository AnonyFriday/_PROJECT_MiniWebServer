namespace WebServer.SDK.Requests.RequestReaders;

public interface IRequestReader
{
    public Task<WRequest> ReadRequestAsync(CancellationToken cancellationTokenSource);
}