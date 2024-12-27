namespace WebServer.SDK.Responses;

public static class WHttpResponsePhrases
{
    /// <summary>
    /// A book contains phrases of HTTP status codes.
    /// </summary>
    /// <param name="code">The HTTP status code.</param>
    /// <returns>The reason phrase corresponding to the status code.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the code is not supported.</exception>
    public static string GetByCode(WHttpResponseStatusCodes code) => code switch
    {
        // Informational responses (100–199)
        WHttpResponseStatusCodes.Informational_Continue => "Continue",
        WHttpResponseStatusCodes.Informational_SwitchingProtocols => "Switching Protocols",
        WHttpResponseStatusCodes.Informational_Processing => "Processing",

        // Successful responses (200–299)
        WHttpResponseStatusCodes.Success_OK => "OK",
        WHttpResponseStatusCodes.Success_Created => "Created",
        WHttpResponseStatusCodes.Success_Accepted => "Accepted",
        WHttpResponseStatusCodes.Success_NonAuthoritativeInformation => "Non-Authoritative Information",
        WHttpResponseStatusCodes.Success_NoContent => "No Content",
        WHttpResponseStatusCodes.Success_ResetContent => "Reset Content",
        WHttpResponseStatusCodes.Success_PartialContent => "Partial Content",

        // Redirection messages (300–399)
        WHttpResponseStatusCodes.Redirection_MultipleChoices => "Multiple Choices",
        WHttpResponseStatusCodes.Redirection_MovedPermanently => "Moved Permanently",
        WHttpResponseStatusCodes.Redirection_Found => "Found",
        WHttpResponseStatusCodes.Redirection_SeeOther => "See Other",
        WHttpResponseStatusCodes.Redirection_NotModified => "Not Modified",
        WHttpResponseStatusCodes.Redirection_UseProxy => "Use Proxy",
        WHttpResponseStatusCodes.Redirection_TemporaryRedirect => "Temporary Redirect",
        WHttpResponseStatusCodes.Redirection_PermanentRedirect => "Permanent Redirect",

        // Client error responses (400–499)
        WHttpResponseStatusCodes.ClientError_BadRequest => "Bad Request",
        WHttpResponseStatusCodes.ClientError_Unauthorized => "Unauthorized",
        WHttpResponseStatusCodes.ClientError_PaymentRequired => "Payment Required",
        WHttpResponseStatusCodes.ClientError_Forbidden => "Forbidden",
        WHttpResponseStatusCodes.ClientError_NotFound => "Not Found",
        WHttpResponseStatusCodes.ClientError_MethodNotAllowed => "Method Not Allowed",
        WHttpResponseStatusCodes.ClientError_NotAcceptable => "Not Acceptable",
        WHttpResponseStatusCodes.ClientError_ProxyAuthenticationRequired => "Proxy Authentication Required",
        WHttpResponseStatusCodes.ClientError_RequestTimeout => "Request Timeout",
        WHttpResponseStatusCodes.ClientError_Conflict => "Conflict",
        WHttpResponseStatusCodes.ClientError_Gone => "Gone",
        WHttpResponseStatusCodes.ClientError_LengthRequired => "Length Required",
        WHttpResponseStatusCodes.ClientError_PayloadTooLarge => "Payload Too Large",
        WHttpResponseStatusCodes.ClientError_URITooLong => "URI Too Long",
        WHttpResponseStatusCodes.ClientError_UnsupportedMediaType => "Unsupported Media Type",
        WHttpResponseStatusCodes.ClientError_RangeNotSatisfiable => "Range Not Satisfiable",
        WHttpResponseStatusCodes.ClientError_ExpectationFailed => "Expectation Failed",
        WHttpResponseStatusCodes.ClientError_ImATeapot => "I'm a teapot",
        WHttpResponseStatusCodes.ClientError_UpgradeRequired => "Upgrade Required",

        // Server error responses (500–599)
        WHttpResponseStatusCodes.ServerError_InternalServerError => "Internal Server Error",
        WHttpResponseStatusCodes.ServerError_NotImplemented => "Not Implemented",
        WHttpResponseStatusCodes.ServerError_BadGateway => "Bad Gateway",
        WHttpResponseStatusCodes.ServerError_ServiceUnavailable => "Service Unavailable",
        WHttpResponseStatusCodes.ServerError_GatewayTimeout => "Gateway Timeout",
        WHttpResponseStatusCodes.ServerError_HTTPVersionNotSupported => "HTTP Version Not Supported",

        // Unsupported HTTP Status Code
        _ => throw new ArgumentOutOfRangeException(nameof(code), "Unsupported HTTP status code")
    };
}