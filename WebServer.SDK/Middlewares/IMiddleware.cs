namespace WebServer.SDK.Middlewares;

public interface IMiddleware
{
    /// <summary>
    /// Middleware handling request and return response. Capture inside the MiddlewareContext
    /// </summary>
    /// <param name="context"></param>
    /// <param name="nextMiddleware"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task InvokeAsync(MiddlewareContext context, ICallable nextMiddlewareCallable, CancellationToken cancellationToken);
}