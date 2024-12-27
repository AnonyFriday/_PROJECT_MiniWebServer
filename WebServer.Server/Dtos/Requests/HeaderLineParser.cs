namespace WebServer.Server.Requests;

public static class HeaderLineParser
{
    public static bool TryParse(string headerLineString, out HeaderLine headerLine)
    {
        headerLine = default;
        if (string.IsNullOrEmpty(headerLineString)) return false;

        // Paresing the header line into the dtos HeaderLine
        // - Split with : as the seperator
        // e.g. Content-Type: application/json 

        var idx = headerLineString.IndexOf(':');
        if (idx == -1) return false;

        headerLine = new HeaderLine()
        {
            Name = headerLineString.Substring(0, idx).Trim(),
            Values = headerLineString.Substring(idx + 1).Trim()
        };

        return true;
    }
}