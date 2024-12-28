using WebServer.SDK.Middlewares;

namespace WebServer.Server.Middlewares;

/// <summary>
/// Follow the pattern of Null Middleware
/// Create to avoid the nextMiddleware is nullable
/// </summary>
public class NullMiddleware : IMiddleware
{
    public Task InvokeAsync(MiddlewareContext context, ICallable nextMiddlewareCallable,
        CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}