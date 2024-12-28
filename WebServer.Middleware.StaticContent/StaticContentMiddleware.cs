using WebServer.SDK.Middlewares;

namespace WebServer.Middleware.StaticContent;

public class StaticContentMiddleware : IMiddleware
{
    public async Task InvokeAsync(MiddlewareContext context, ICallable nextMiddlewareCallable,
        CancellationToken cancellationToken)
    {
        // 1. Request


        // 2. Next Middleware 
        await nextMiddlewareCallable.InvokeAsync(context, cancellationToken);


        // 3. Response
    }
}