using Microsoft.Extensions.Primitives;

namespace WebServer.Server.Requests;

public class HeaderLine
{
    public string Name { get; set; }
    public StringValues Values { get; set; } = String.Empty;
}