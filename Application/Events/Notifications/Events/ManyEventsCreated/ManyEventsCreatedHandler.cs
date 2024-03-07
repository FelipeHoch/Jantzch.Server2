using MediatR;

namespace Jantzch.Server2;

public class ManyEventsCreatedHandler : INotificationHandler<ManyEventsCreatedNotification>
{
    public async Task Handle(ManyEventsCreatedNotification notification, CancellationToken cancellationToken)
    {
        // TODO: Implement the ManyEventsCreatedHandler

        await Task.CompletedTask;
    }
}
