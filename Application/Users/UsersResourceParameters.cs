using Jantzch.Server2.Application.Shared;

namespace Jantzch.Server2.Application.Users;

public class UsersResourceParameters : ResourceParameters
{
    public string? Role { get; set; }
}
