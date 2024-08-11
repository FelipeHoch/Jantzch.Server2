using MongoDB.Bson;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using Jantzch.Server2.Domain.Entities.Users.Enums;

namespace Jantzch.Server2.Domain.Entities.Users;

public class User
{
    [JsonIgnore]
    public ObjectId? Id { get; set; } = ObjectId.GenerateNewId();

    public ObjectId? IdentityProviderId { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Provider { get; set; }

    public string Role { get; set; }

    public int? CustByHour { get; set; } = 0;

    public List<UserTypeEnum>? Types { get; set; } = [UserTypeEnum.Technical];

    public double CalculateTotalManPower(double hoursWorked)
    {
        return hoursWorked * (int)CustByHour;
    }
}
