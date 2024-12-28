using Microsoft.Extensions.Primitives;

namespace WebServer.Server.Dtos.Requests;

public class HeaderLine
{
    public string Name { get; set; }
    public StringValues Values { get; set; } = String.Empty;
}