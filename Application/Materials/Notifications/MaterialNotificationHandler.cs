using MediatR;

namespace Jantzch.Server2.Application.Materials.Notifications;

public class MaterialNotificationHandler : INotificationHandler<MaterialCreatedNotification>
{
    public Task Handle(MaterialCreatedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(notification.Name);

        return Task.FromResult(Unit.Value);
    }
}
