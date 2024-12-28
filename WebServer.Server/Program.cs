using WebServer.SDK.Middlewares;
using WebServer.SDK.RequestReaders;
using WebServer.SDK.ResponseWriters;
using WebServer.Server;
using WebServer.Server.Middlewares;
using WebServer.Server.Middlewares.MiddlewareChain;
using WebServer.Server.RequestReaders;
using WebServer.Server.ResponseWriters;

// ===========================
// === Builder Design Pattern
// ===========================

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

// Register Singleton WebServerOptions
// - Get the section "WebServerOptions" from the appsettings.json file
// - Return an instance of WebServerOptions by extracting from appsettings
//   or creating new instance with default properties
builder.Services.AddSingleton<WebServerOptions>(provider =>
{
    var configurationSection = builder.Configuration.GetSection("WebServerOptions");
    WebServerOptions? webServerOptions = configurationSection.Get<WebServerOptions>();
    return webServerOptions ?? new WebServerOptions();
});

// Register Singleton RequestReaderFactory
// - Register an interface and instance of the RequestReaderFactory with the instance of ILoggerFactory
// - ILoggerFactory retrieved from the service providers that register ILoggerFactory by default
builder.Services.AddSingleton<IRequestReaderFactory>(services =>
    new RequestReaderFactory(services.GetRequiredService<ILoggerFactory>()));

// Register Singleton RequestReaderFactory
// - Register an interface and instance of the RequestReaderFactory with the instance of ILoggerFactory
// - ILoggerFactory retrieved from the service providers that register ILoggerFactory by default
builder.Services.AddSingleton<IResponseWriterFactory>(services =>
    new ResponseWriterFactory(services.GetRequiredService<ILoggerFactory>()));

// Register Singleton CallableMidlewarePipeline
builder.Services.AddSingleton<IMiddlewarePipeline>(services => new CallableMiddlewarePipeline());

// ===========================
// === Build the Host
// ===========================

var host = builder.Build();
host.Run();