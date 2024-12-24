using System.Net;

namespace WebServer.Server;

public class WebServerOptions
{
    public string IpAddress { get; set; } = string.Empty;
    public int Port { get; set; } = 80;
}