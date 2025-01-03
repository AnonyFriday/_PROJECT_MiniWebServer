# Mini Web Server Project

## Overview
This project is a mini web server implemented in C# Worker Service template with .NET 9, that currently supports basic middleware functionalities. The server processes HTTP requests, but as of now, it only supports the GET method. The architecture follows a modular and extensible design, leveraging design patterns like Chain of Responsibility, Null Object, and Wrapper/ Decorator for the Middleware Pipelines.

## Features
- **Middleware Pipeline:**
  - Middleware functionality implemented using `IMiddleware` and `ICallable` interfaces.
  - Chain of Responsibility pattern used to pass the `MiddlewareContext` through each middleware.
  - Null Object pattern for safe handling of uninitialized middleware.
  - Extensible pipeline for adding new middleware.

- **Static Content Middleware:**
  - Serves static files from a specified folder (`wwwroot`).
  - Checks file existence before returning a response.
  - Returns a 404 response if the file does not exist.

- **Authentication Middleware:**
  - Checks for the presence of an `Authorization` header.
  - Returns a 401 Unauthorized response if the header is missing.

- **Not Found Middleware:**
  - Acts as the tail middleware, always returning a 404 response if no other middleware handles the request.

- **Null Middleware:**
  - Implements the Null Object pattern as a safe placeholder when no middleware is configured.

## Code Highlights

### Middleware Pipeline
The `CallableMiddlewarePipeline` class manages the middleware chain:
```csharp
public class CallableMiddlewarePipeline : IMiddlewarePipeline
{
    private ICallable _firstCallable = new NullCallable();

    public void AddMiddleware(IMiddleware middleware)
    {
        _firstCallable = new Callable()
        {
            CurrMiddleware = middleware,
            NextCallable = _firstCallable
        };
    }

    public async Task InvokeMiddlewarePipelineAsync(MiddlewareContext context, CancellationToken cancellationToken)
    {
        await _firstCallable.InvokeAsync(context, cancellationToken);
    }
}
```

### Static Content Middleware
Serves static files based on the request URI:
```csharp
public class StaticContentMiddleware : IMiddleware
{
    private const string _webRootFolderPath = "wwwroot";

    public async Task InvokeAsync(MiddlewareContext context, ICallable nextMiddlewareCallable,
        CancellationToken cancellationToken)
    {
        if (context.Request.Method == WMethods.GET)
        {
            var uri = context.Request.Uri.TrimStart('/').Replace("/", "\\");
            var staticFilePath = Path.Combine(_webRootFolderPath, uri);

            if (!File.Exists(staticFilePath))
            {
                await nextMiddlewareCallable.InvokeAsync(context, cancellationToken);
            }
            else
            {
                string content = await File.ReadAllTextAsync(staticFilePath, Encoding.UTF8, cancellationToken);
                context.Response.ResponseCode = WHttpResponseStatusCodes.Success_Accepted;
                context.Response.ContentType = "text/html;charset=utf-8";
                context.Response.ResponseBodyWriter = new StringResponseBodyWriter(content);
            }
        }
    }
}
```

### Authentication Middleware
Ensures requests have an `Authorization` header:
```csharp
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

        await nextMiddlewareCallable.InvokeAsync(context, cancellationToken);
    }
}
```

### Not Found Middleware
Final middleware to handle unmatched requests:
```csharp
public class NotFoundMiddleware : IMiddleware
{
    private static readonly IResponseBodyWriter EmptyBodyContentWriter = new StringResponseBodyWriter("");

    public Task InvokeAsync(MiddlewareContext context, ICallable nextMiddlewareCallable,
        CancellationToken cancellationToken)
    {
        context.Response.ContentLength = 0;
        context.Response.ResponseCode = WHttpResponseStatusCodes.ClientError_NotFound;
        context.Response.ContentType = "text/html";
        context.Response.ResponseBodyWriter = EmptyBodyContentWriter;
        return Task.CompletedTask;
    }
}
```

### Null Middleware
Placeholder middleware to safely terminate the chain:
```csharp
public class NullMiddleware : IMiddleware
{
    public Task InvokeAsync(MiddlewareContext context, ICallable nextMiddlewareCallable, CancellationToken cancellationToken)
    {
        // Do nothing and safely return
        return Task.CompletedTask;
    }
}
```

## Middleware Registration
Instead of configuring the middleware pipeline manually, the project uses the `Host` service with dependency injection:

```csharp
// Register Singleton CallableMiddlewarePipeline
builder.Services.AddSingleton<IMiddlewarePipeline>(services => new CallableMiddlewarePipeline());

// Add Middlewares
// AuthenticationMiddleware --> StaticContentMiddleware --> NotFoundMiddleware --> NullMiddleware
_middlewarePipeline.AddMiddleware(new NotFoundMiddleware());
_middlewarePipeline.AddMiddleware(new StaticContentMiddleware());
_middlewarePipeline.AddMiddleware(new AuthenticationMiddleware());
```

## How Middleware Works
1. **Request Handling:**
   - Each middleware processes the incoming request.
   - If a middleware cannot handle the request, it passes the `MiddlewareContext` to the next middleware.

2. **Response Handling:**
   - Middlewares can modify the response before passing it to the previous middleware in the chain.

3. **Cancellation Support:**
   - Each middleware checks the `CancellationToken` to gracefully exit if cancellation is requested.

## Setup
### Steps to Run the Project
1. Clone the repository.
2. Open the solution in your favorite IDE.
3. Build the solution to restore dependencies.
4. Navigate to the `WebServer.Server` project and run it.
5. Open any browser and visit: `http://localhost:8888/index_demo.html`.
   - Ensure you have the `wwwroot` folder with `index_demo.html` in the root directory.
6. Alternatively, you can run the `ClientConsole` application to see the response object.

## Example Middleware Pipeline Configuration
```csharp
_middlewarePipeline.AddMiddleware(new AuthenticationMiddleware());
_middlewarePipeline.AddMiddleware(new StaticContentMiddleware());
_middlewarePipeline.AddMiddleware(new NotFoundMiddleware());

await _middlewarePipeline.InvokeMiddlewarePipelineAsync(context, cancellationToken);
```

### Static Content Middleware Demonstration
Below is an example of the `index_demo.html` file served by the `StaticContentMiddleware`. If the middleware pipeline is configured correctly, you should see the following content in your browser:

**HTML Content:**
```html
<!DOCTYPE html>
<html>
<head>
    <title>StaticContentMiddleware Demo</title>
</head>
<body>
    <h1>This is a demonstration on StaticContentMiddleware</h1>
    <p>If you see this page, it means you configured the middleware pipeline successfully.</p>
</body>
</html>
```

## Future Plans
- Add support for other HTTP methods (POST, PUT, DELETE).
- Implement logging middleware.
- Enhance error handling and response customization.
- Add support for query parameters and request body parsing.

## License
This project is licensed under the MIT License. See the LICENSE file for details.

