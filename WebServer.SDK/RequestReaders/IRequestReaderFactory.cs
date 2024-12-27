using System.Net.Sockets;

namespace WebServer.SDK.RequestReaders;

public interface IRequestReaderFactory
{
    /// <summary>
    /// Socket is maintained throughout the project, socket is destroyed ouside
    /// </summary>
    /// <param name="socket"></param>
    /// <returns></returns>
    IRequestReader Create(Socket socket);
}