using WebServer.Server;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

// Register Singleton
// - Get the section "WebServerOptions" from the appsettings.json file
// - Return an instance of WebServerOptions by extracting from appsettings
//   or creating new instance with default properties
builder.Services.AddSingleton<WebServerOptions>(provider =>
{
    var configurationSection = builder.Configuration.GetSection("WebServerOptions");
    WebServerOptions webServerOptions = configurationSection.Get<WebServerOptions>();
    return webServerOptions ?? new WebServerOptions();
});

var host = builder.Build();
host.Run();