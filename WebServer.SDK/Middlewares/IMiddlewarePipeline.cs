using WebServer.SDK.Middlewares;

namespace WebServer.Server.Middlewares;

/// <summary>
/// Interface to manage the entire middleware
/// </summary>
public interface IMiddlewarePipeline
{
    public void AddMiddleware(IMiddleware middleware);
    public Task InvokeMiddlewarePipelineAsync(MiddlewareContext context, CancellationToken cancellationToken);
}