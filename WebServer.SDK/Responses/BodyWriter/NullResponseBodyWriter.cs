namespace WebServer.SDK.Responses.BodyWriter;

/// <summary>
/// Follow the Null Object Design Pattern
/// - Reduce the use of checking null at everywhere
/// </summary>
public class NullResponseBodyWriter : IResponseBodyWriter
{
    // ===========================
    // === Singleton
    // ===========================

    /// <summary>
    /// Avoid create multiple NULL object for various difference requests
    /// </summary>
    public static readonly NullResponseBodyWriter Instance = new();

    private NullResponseBodyWriter()
    {
    }

    // ===========================
    // === Methods
    // ===========================

    /// <summary>
    /// Return a task successfully
    /// </summary>
    /// <param name="bodyStream"></param>
    /// <returns></returns>
    public Task WriteAsync(Stream bodyStream)
    {
        return Task.CompletedTask;
    }
}