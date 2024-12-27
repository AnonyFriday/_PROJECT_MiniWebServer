using System.Net.Sockets;
using WebServer.SDK.Responses;
using WebServer.SDK.Responses.ResponseWriters;

namespace WebServer.Server.ResponseWriters;

public class ResponseWriterFactory : IResponseWriterFactory
{
    // ===========================
    // === Fields & Props
    // ===========================

    private readonly ILoggerFactory _loggerFactory;

    // ===========================
    // === Constructors
    // ===========================

    public ResponseWriterFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    // ===========================
    // === Methods
    // ===========================

    public IResponseWriter Create(Socket socket)
    {
        return new DefaultResponseWriter(_loggerFactory.CreateLogger<DefaultResponseWriter>(), socket);
    }
}