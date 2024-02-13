namespace Jantzch.Server2.Application.Abstractions.Jwt;

public interface IJwtService
{
    string GetNameFromToken();
    string GetRoleFromToken();
    string GetEmailFromToken();
    string GetSubFromToken();
    long GetIatFromToken();
    long GetExpFromToken();
    string GetIssFromToken();
    string GetAudFromToken();

    string GetNameIdentifierFromToken();
}
