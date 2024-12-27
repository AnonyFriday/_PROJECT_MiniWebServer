using System.Net.Sockets;
using WebServer.SDK.RequestReaders;

namespace WebServer.Server.RequestReaders;

/// <summary>
/// Create an IRequestReader based on condition
/// </summary>
public class RequestReaderFactory : IRequestReaderFactory
{
    // ===========================
    // === Fields & Props
    // ===========================
    private readonly ILoggerFactory _loggerFactory;

    // ===========================
    // === Constructors
    // ===========================
    public RequestReaderFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    // ===========================
    // === Methods
    // ===========================

    /// <summary>
    /// Function to create a concrete class implements the IRequestReader
    /// </summary>
    /// <param name="socket"></param>
    /// <returns></returns>
    public IRequestReader Create(Socket socket)
    {
        return new DefaultRequestReader(_loggerFactory.CreateLogger<DefaultRequestReader>(), socket);
    }
}