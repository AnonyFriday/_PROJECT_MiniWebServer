namespace ClientConsole;

public static class HttpResponseMessageExtension
{
    internal static void WriteRequestToConsole(this HttpResponseMessage response)
    {
        if (response is null)
        {
            return;
        }

        var request = response.RequestMessage;
        Console.WriteLine("== Request (Request Line) ==================\n");
        Console.WriteLine($"{request?.Method} {request?.RequestUri} HTTP/{request?.Version}\n");
        Console.WriteLine("== Response (Body) =========================\n");
    }
}