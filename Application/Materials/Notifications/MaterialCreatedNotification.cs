using MediatR;

namespace Jantzch.Server2.Application.Materials.Notifications;

public class MaterialCreatedNotification : INotification
{
    public MaterialCreatedNotification(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public string Id { get; set; }
    public string Name { get; set; }
}
