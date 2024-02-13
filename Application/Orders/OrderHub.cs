using Jantzch.Server2.Domain.Entities.Orders;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Jantzch.Server2.Application.Orders;

public class OrderHub : Hub
{
    static ConcurrentDictionary<string, string> _connectedUsers = new();

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier; // get user id from Context
        _connectedUsers.TryAdd(Context.ConnectionId, userId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        string userId;
        _connectedUsers.TryRemove(Context.ConnectionId, out userId);

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendToManyUsersAsync(List<string> userIds, Order order, CancellationToken cancellationToken)
    {
        foreach (var userId in userIds)
        {
            await Clients.User(userId).SendAsync("ReceiveOrder", order, cancellationToken: cancellationToken);
        }
    }
}
