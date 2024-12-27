namespace WebServer.SDK.Responses.BodyWriters;

public interface IResponseBodyWriter
{
    /// <summary>
    /// Writing the response body to the writer stream  <br />
    /// - If I/O - bound involved, using async/await
    /// - Else, just 
    /// </summary>
    /// <param name="bodyStream"></param>
    /// <returns></returns>
    public Task WriteAsync(Stream bodyStream);
}