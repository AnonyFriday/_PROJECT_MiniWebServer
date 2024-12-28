using WebServer.SDK.Middlewares;
using WebServer.SDK.Responses;

namespace WebServer.Middleware.Authentication;

/// <summary>
/// DEMO of the authentication middleware in the middleware chainlink
/// </summary>
public class AuthenticationMiddleware : IMiddleware
{
    public async Task InvokeAsync(MiddlewareContext context, ICallable nextMiddlewareCallable,
        CancellationToken cancellationToken)
    {
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Response.ResponseCode = WHttpResponseStatusCodes.ClientError_Unauthorized;
            context.Response.ReasonPhrase =
                WHttpResponsePhrases.GetByCode(WHttpResponseStatusCodes.ClientError_Unauthorized);
            return;
        }

        // If authorized, passing the MiddlewareContext to the next middleware
        await nextMiddlewareCallable.InvokeAsync(context, cancellationToken);
    }
}