using WebServer.SDK.Middlewares;

namespace WebServer.Server.Middlewares.MiddlewareChain;

/// <summary>
/// Act as a wrapper per middleware and current pointer in the linked list data structure <br />
/// - Points to current Middleware
/// - Contains the next IMiddleware
/// </summary>
public class Callable : ICallable
{
    // ===========================
    // === Props & Fields
    // ===========================

    public IMiddleware CurrMiddleware { get; set; } // as content

    public ICallable?
        NextCallable { get; set; } // since implement Null object Design Pattern, it never be null in practise

    // ===========================
    // === Methods
    // ===========================

    /// <summary>
    /// Invoking the Callable with captured logic
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task InvokeAsync(MiddlewareContext context, CancellationToken cancellationToken)
    {
        if (NextCallable == null)
        {
            throw new InvalidOperationException("Next callable is null");
        }

        // When involking the InvokeAsync of Callable
        // The logic behind is to invoke the middleware's InvokeAsync
        await CurrMiddleware.InvokeAsync(context, NextCallable, cancellationToken);
    }
}