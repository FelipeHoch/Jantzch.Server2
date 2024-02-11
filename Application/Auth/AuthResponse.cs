using System.Text.Json.Serialization;

namespace Jantzch.Server2.Application.Auth;

public class AuthResponse
{
    public string Id { get; set; }

    public string IdentityProviderId { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Role { get; set; } = string.Empty;
    
    public string? Token { get; set; }
}
