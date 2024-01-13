using System.Net;

namespace Jantzch.Server2.Infraestructure.Errors;

public class RestException : Exception
{
    public RestException(HttpStatusCode code, object? errors = null)
    {
        Code = code;
        Errors = errors;
    }

    public object? Errors { get; set; }

    public HttpStatusCode Code { get; }
}