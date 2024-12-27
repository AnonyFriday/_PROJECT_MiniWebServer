using System.Net.Sockets;

namespace WebServer.SDK.Responses.ResponseWriters;

public interface IResponseWriterFactory
{
    /// <summary>
    /// Socket is maintained throughout the project, socket is destroyed ouside
    /// </summary>
    /// <param name="socket"></param>
    /// <returns></returns>
    IResponseWriter Create(Socket socket);
}