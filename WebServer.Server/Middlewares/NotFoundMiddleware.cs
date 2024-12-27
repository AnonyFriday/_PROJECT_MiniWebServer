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
    public Task InvokeAsync(MiddlewareContext context, IMiddleware nextMiddleware,
        CancellationToken cancellationToken)
    {
        // 1. Process Request
        // ...

        // 2. Execute the next middleware
        nextMiddleware.InvokeAsync(context, new NullMiddleware(), cancellationToken);

        // 3. Return Response
        context.Response.ContentLength = 0;
        context.Response.ResponseCode = WHttpResponseStatusCodes.ClientError_NotFound;
        context.Response.ContentType = "text/html";
        context.Response.ResponseBodyWriter = EmptyBodyContentWriter;
        return Task.CompletedTask;
    }
}