using System.Net.Sockets;

namespace WebServer.SDK.Requests.RequestReaders;

public interface IRequestReaderFactory
{
    IRequestReader Create(Socket socket);
}