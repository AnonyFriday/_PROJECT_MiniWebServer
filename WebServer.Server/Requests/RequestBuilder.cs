using Microsoft.Extensions.Primitives;
using WebServer.SDK;
using WebServer.SDK.Requests;

namespace WebServer.Server.Dtos.Requests;

/// <summary>
/// Create an object by adding multiple props of the object
/// </summary>
public class RequestBuilder
{
    public WMethods Method { get; private set; }
    public string Uri { get; private set; } = string.Empty;
    public string ProtocolVersion { get; private set; } = string.Empty;
    public string Host { get; private set; } = string.Empty;
    public bool IsKeepAlive { get; set; } = false;
    public Dictionary<string, StringValues> Headers { get; set; } = new Dictionary<string, StringValues>();

    /// <summary>
    /// Build a different type of request based on supplied fields
    /// </summary>
    /// <returns></returns>
    public WRequest Build()
    {
        Validate();

        var request = new WRequest()
        {
            Method = Method,
            Uri = Uri,
            ProtocolVersion = ProtocolVersion,
            Host = Host,
            IsKeepAlive = IsKeepAlive,
            Headers = Headers,
        };

        return request;
    }

    /// <summary>
    /// Validate to see if any field is missing
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    private void Validate()
    {
        if (string.IsNullOrEmpty(Uri))
        {
            throw new ArgumentNullException(nameof(Uri));
        }

        if (string.IsNullOrEmpty(ProtocolVersion))
        {
            throw new ArgumentNullException(nameof(ProtocolVersion));
        }

        if (string.IsNullOrEmpty(Host))
        {
            throw new ArgumentNullException(nameof(Host));
        }
    }

    public void AddRequestLine(RequestLine requestLine)
    {
        Method = requestLine.Method;
        Uri = requestLine.UriPath;
        ProtocolVersion = requestLine.ProtocolVersion;
    }

    /// <summary>
    /// Set the connection and host
    /// Set header not exist, then create and add
    /// Set header exists, just concate into the StringValues
    /// </summary>
    /// <param name="headerLine"></param>
    public void AddHeaderLine(HeaderLine headerLine)
    {
        // Host and Connection is part of headers but I want to seperate for further access 
        if ("Host".Equals(headerLine.Name, StringComparison.OrdinalIgnoreCase))
        {
            Host = headerLine.Values.FirstOrDefault() ?? String.Empty;
        }
        else if ("Connection".Equals(headerLine.Name, StringComparison.OrdinalIgnoreCase))
        {
            IsKeepAlive = "keep-alive".Equals(headerLine.Values.First(), StringComparison.OrdinalIgnoreCase);
        }

        // Other Headers
        // - if already exits, concatenate to follow the spec of MDN
        if (!Headers.TryGetValue(headerLine.Name, out var values))
        {
            Headers.Add(headerLine.Name, headerLine.Values);
        }
        else
        {
            Headers[headerLine.Name] = StringValues.Concat(values, headerLine.Values);
        }
    }
}