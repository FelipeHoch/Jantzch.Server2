using System.Text.Json.Serialization;

namespace Jantzch.Server2.Application.Users.Models;

public class UserFromIdpDto
{
    public string Id { get; set; } = default!;
    
    public string Name { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Role { get; set; } = default!;

    public string Provider { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Token { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? NewPassword { get; set; }
}
