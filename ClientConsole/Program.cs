using System.Net;

namespace ClientConsole;

/// <summary>
/// The purpose of this project is to demonstrate the HttpClient sending request
/// to WebServer
/// </summary>
class Program
{
    // ===========================
    // === Fields & Props
    // ===========================

    private static readonly IPAddress _serverAddress = IPAddress.Loopback;
    private static readonly int _serverPort = 8888;

    private static readonly HttpClient sharedClient = new HttpClient()
    {
        BaseAddress = new Uri($"http://{_serverAddress}:{_serverPort}"),
    };

    // ===========================
    // === Main
    // ===========================

    /// <summary>
    /// Main Function
    /// </summary>
    /// <param name="args"></param>
    static async Task Main(string[] args)
    {
        await GetRequest("index_demo.html");
    }

    // ===========================
    // === Methods
    // ===========================

    public static async Task GetRequest(string uriPath)
    {
        try
        {
            // Get Async
            HttpResponseMessage response = await sharedClient.GetAsync(uriPath);

            // Throw an exception if it's not a success status code 200
            // - If successfull, then write the request information led to this response to the console
            response.EnsureSuccessStatusCode().WriteRequestToConsole();

            // Output Response to Console
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{stringResponse}\n");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
    }
}