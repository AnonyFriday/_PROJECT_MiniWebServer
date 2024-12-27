using WebServer.SDK;

namespace WebServer.Server.Dtos.Requests;

public static class RequestLineParser
{
    public static bool TryParse(String requestLineString, out RequestLine requestLine)
    {
        requestLine = default;
        if (string.IsNullOrEmpty(requestLineString)) return false;

        // REQUEST LINE: GET /Protocols/rfc23232/employee.html HTTP/1.1
        // Host: www.example.com
        // Accept: text/html
        // User-Agent: Mozilla/5.0
        // Connection: keep-alive

        // Using split
        // - Method = GET
        // - UriPath = /Protocols/rfc23232/employee.html
        // - VersionProtocol = HTTP/1.1

        string[] requestLineEntries = requestLineString.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        requestLine = new RequestLine();

        // Checking Method
        string method = requestLineEntries[0];
        switch (method)
        {
            case "GET": requestLine.Method = WMethods.GET; break;
            case "POST": requestLine.Method = WMethods.POST; break;
        }

        // Checking Uri Path
        string uriPath = requestLineEntries[1];
        requestLine.UriPath = string.IsNullOrEmpty(uriPath) ? string.Empty : uriPath;

        // Cheking Version
        string version = requestLineEntries[2];
        requestLine.ProtocolVersion = string.IsNullOrEmpty(version) ? string.Empty : version;

        return true;
    }
}