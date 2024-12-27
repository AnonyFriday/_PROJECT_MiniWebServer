namespace WebServer.SDK;

/// <summary>
/// Reference to the HTTP Status Code of the MDN specs <br />
/// Link: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status
/// </summary>
public enum WHttpResponseStatusCodes : int
{
    // 1xx: Informational
    Informational_Continue = 100,
    Informational_SwitchingProtocols = 101,
    Informational_Processing = 102,
    Informational_EarlyHints = 103,

    // 2xx: Success
    Success_OK = 200,
    Success_Created = 201,
    Success_Accepted = 202,
    Success_NonAuthoritativeInformation = 203,
    Success_NoContent = 204,
    Success_ResetContent = 205,
    Success_PartialContent = 206,
    Success_MultiStatus = 207,
    Success_AlreadyReported = 208,
    Success_IMUsed = 226,

    // 3xx: Redirection
    Redirection_MultipleChoices = 300,
    Redirection_MovedPermanently = 301,
    Redirection_Found = 302,
    Redirection_SeeOther = 303,
    Redirection_NotModified = 304,
    Redirection_UseProxy = 305,
    Redirection_TemporaryRedirect = 307,
    Redirection_PermanentRedirect = 308,

    // 4xx: Client Errors
    ClientError_BadRequest = 400,
    ClientError_Unauthorized = 401,
    ClientError_PaymentRequired = 402,
    ClientError_Forbidden = 403,
    ClientError_NotFound = 404,
    ClientError_MethodNotAllowed = 405,
    ClientError_NotAcceptable = 406,
    ClientError_ProxyAuthenticationRequired = 407,
    ClientError_RequestTimeout = 408,
    ClientError_Conflict = 409,
    ClientError_Gone = 410,
    ClientError_LengthRequired = 411,
    ClientError_PreconditionFailed = 412,
    ClientError_PayloadTooLarge = 413,
    ClientError_URITooLong = 414,
    ClientError_UnsupportedMediaType = 415,
    ClientError_RangeNotSatisfiable = 416,
    ClientError_ExpectationFailed = 417,
    ClientError_ImATeapot = 418,
    ClientError_MisdirectedRequest = 421,
    ClientError_UnprocessableEntity = 422,
    ClientError_Locked = 423,
    ClientError_FailedDependency = 424,
    ClientError_TooEarly = 425,
    ClientError_UpgradeRequired = 426,
    ClientError_PreconditionRequired = 428,
    ClientError_TooManyRequests = 429,
    ClientError_RequestHeaderFieldsTooLarge = 431,
    ClientError_UnavailableForLegalReasons = 451,

    // 5xx: Server Errors
    ServerError_InternalServerError = 500,
    ServerError_NotImplemented = 501,
    ServerError_BadGateway = 502,
    ServerError_ServiceUnavailable = 503,
    ServerError_GatewayTimeout = 504,
    ServerError_HTTPVersionNotSupported = 505,
    ServerError_VariantAlsoNegotiates = 506,
    ServerError_InsufficientStorage = 507,
    ServerError_LoopDetected = 508,
    ServerError_NotExtended = 510,
    ServerError_NetworkAuthenticationRequired = 511
}