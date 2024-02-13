using Jantzch.Server2.Application.Abstractions.Jwt;
using System.Security.Claims;

namespace Jantzch.Server2.Infrastructure.Jwt;

public class JwtService : IJwtService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetNameFromToken()
    {
        return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
    }

    public string GetRoleFromToken()
    {
        return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
    }

    public string GetEmailFromToken()
    {
        return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
    }

    public string GetSubFromToken()
    {
        return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
    }

    public long GetIatFromToken()
    {
        return long.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "iat")?.Value);
    }

    public long GetExpFromToken()
    {
        return long.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "exp")?.Value);
    }

    public string GetIssFromToken()
    {
        return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "iss")?.Value;
    }

    public string GetAudFromToken()
    {
        return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "aud")?.Value;
    }

    public string GetNameIdentifierFromToken()
    {
        return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}
