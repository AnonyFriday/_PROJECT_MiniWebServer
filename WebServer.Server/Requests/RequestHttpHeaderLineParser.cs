using WebServer.SDK;

namespace WebServer.Server.Requests;

public class RequestHttpHeaderLineParser
{
    public static bool TryParse(String requestLine, out RequestHttpHeaderLine requestHttpHeaderLine)
    {
        requestHttpHeaderLine = default;

        // REQUEST HEADER
        // GET /Protocols/rfc23232/employee.html HTTP/1.1
        // Host: www.example.com
        // Accept: text/html
        // User-Agent: Mozilla/5.0
        // Connection: keep-alive

        // Using split
        // - Method = GET
        // - UriPath = /Protocols/rfc23232/employee.html
        // - VersionProtocol = HTTP/1.1

        string httpHeaderLine = requestLine.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None)[0];

        // If don't have first line
        if (string.IsNullOrEmpty(httpHeaderLine))
        {
            return false;
        }

        string[] httpHeaderLineEntries = httpHeaderLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        requestHttpHeaderLine = new RequestHttpHeaderLine();

        // Checking Method
        string method = httpHeaderLineEntries[0];
        switch (method)
        {
            case "GET": requestHttpHeaderLine.Method = WMethods.GET; break;
            case "POST": requestHttpHeaderLine.Method = WMethods.POST; break;
        }

        // Checking Uri Path
        string uriPath = httpHeaderLineEntries[1];
        requestHttpHeaderLine.UriPath = string.IsNullOrEmpty(uriPath) ? string.Empty : uriPath;

        // Cheking Version
        string version = httpHeaderLineEntries[2];
        requestHttpHeaderLine.ProtocolVersion = string.IsNullOrEmpty(version) ? string.Empty : version;

        return true;
    }
}