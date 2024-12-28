using WebServer.SDK.Middlewares;

namespace WebServer.Server.Middlewares.MiddlewareChain;

/// <summary>
/// A class manages all wrapper on middleware, which based on idea of Linkedlist
/// </summary>
public class CallableMiddlewarePipeline : IMiddlewarePipeline
{
    // ===========================
    // === Props & Fields
    // ===========================

    private ICallable _firstCallable = new NullCallable();

    // ===========================
    // === Methods
    // ===========================

    /// <summary>
    /// Wrapping the middleware into the Callable and push to head
    /// </summary>
    /// <param name="middleware"></param>
    public void AddMiddleware(IMiddleware middleware)
    {
        _firstCallable = new Callable()
        {
            CurrMiddleware = middleware,
            NextCallable = _firstCallable
        };
    }

    /// <summary>
    /// Invoke the callable InvokeAsync
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    public async Task InvokeMiddlewarePipelineAsync(MiddlewareContext context, CancellationToken cancellationToken)
    {
        await _firstCallable.InvokeAsync(context, cancellationToken);
    }
}