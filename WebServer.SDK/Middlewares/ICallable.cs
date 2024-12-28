namespace WebServer.SDK.Middlewares;

/// <summary>
/// This interface contains method that share amongst middleware
/// </summary>
public interface ICallable
{
    Task InvokeAsync(MiddlewareContext context, CancellationToken cancellationToken);
}