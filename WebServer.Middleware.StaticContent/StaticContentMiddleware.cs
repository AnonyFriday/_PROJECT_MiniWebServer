using System.Text;
using WebServer.SDK.Middlewares;
using WebServer.SDK.Requests;
using WebServer.SDK.Responses;
using WebServer.SDK.Responses.BodyWriters;

namespace WebServer.Middleware.StaticContent;

public class StaticContentMiddleware : IMiddleware
{
    // ===========================
    // === Fields & Props
    // ===========================
    private const string _webRootFolderPath = "wwwroot";

    // ===========================
    // === Methods
    // ===========================

    public async Task InvokeAsync(MiddlewareContext context, ICallable nextMiddlewareCallable,
        CancellationToken cancellationToken)
    {
        // If that is not a GET method, move to next NotFoundMiddleware
        if (context.Request.Method != WMethods.GET)
        {
            await nextMiddlewareCallable.InvokeAsync(context, cancellationToken);
            return;
        }

        // e.g. /index_demo.html
        var uri = context.Request.Uri;
        if (uri.TrimStart().StartsWith("/"))
        {
            // Cutoff the "/"
            uri = uri.Substring(1).Replace("/", "\\");
        }

        var staticFilePath = Path.Combine(_webRootFolderPath, uri);

        // Check if file exists
        if (!File.Exists(staticFilePath))
        {
            // Go to the NotFoundMiddleware
            await nextMiddlewareCallable.InvokeAsync(context, cancellationToken);
            return;
        }

        string stringRawContent = await File.ReadAllTextAsync(staticFilePath, Encoding.UTF8, cancellationToken);
        context.Response.ResponseCode = WHttpResponseStatusCodes.Success_Accepted;
        context.Response.ResponseBodyWriter = new StringResponseBodyWriter(stringRawContent);
        context.Response.ContentType = "text/html;charset=utf-8";
        context.Response.ContentLength = stringRawContent.Length;
        context.Response.ReasonPhrase =
            WHttpResponsePhrases.GetByCode(WHttpResponseStatusCodes.Success_Accepted);
    }
}