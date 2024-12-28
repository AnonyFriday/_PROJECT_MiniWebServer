using WebServer.SDK.Middlewares;
using WebServer.SDK.Responses;
using WebServer.SDK.Responses.BodyWriters;

namespace WebServer.Server.Middlewares;

public class NotFoundMiddleware : IMiddleware
{
    // ===========================
    // === Props & Fields
    // ===========================

    // Because this one is the unmodifiable object, empty object is always the same
    private static readonly IResponseBodyWriter EmptyBodyContentWriter = new StringResponseBodyWriter("");

    // ===========================
    // === Methods
    // ===========================

    /// <summary>
    /// Middleware to return the response with 404 status code <br />
    /// Tail pointer of the Linked List Middleware
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task InvokeAsync(MiddlewareContext context, ICallable nextMiddlewareCallable,
        CancellationToken cancellationToken)
    {
        // 1. Process Request
        Console.WriteLine("NotFoundMiddleware: Processing request...");

        // 2. Execute the next middleware callable
        nextMiddlewareCallable.InvokeAsync(context, cancellationToken);

        // 3. Return Response
        context.Response.ContentLength = 0;
        context.Response.ResponseCode = WHttpResponseStatusCodes.ClientError_NotFound;
        context.Response.ContentType = "text/html";
        context.Response.ResponseBodyWriter = EmptyBodyContentWriter;
        return Task.CompletedTask;
    }
}