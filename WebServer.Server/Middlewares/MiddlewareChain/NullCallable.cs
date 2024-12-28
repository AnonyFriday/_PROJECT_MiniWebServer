using WebServer.SDK.Middlewares;

namespace WebServer.Server.Middlewares;

public class NullCallable : ICallable
{
    // ===========================
    // === Props & Fields
    // ===========================

    public IMiddleware CurrMiddleware { get; private set; } = new NullMiddleware();
    public ICallable NextCallable { get; } = null;

    // ===========================
    // === Methods
    // ===========================

    public Task InvokeAsync(MiddlewareContext context, CancellationToken cancellationToken)
    {
        // Can pass overhead on calling function in stack
        // return CurrMiddleware.InvokeAsync(context, null, cancellationToken);
        return Task.CompletedTask;
    }
}