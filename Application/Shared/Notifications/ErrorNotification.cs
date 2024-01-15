using MediatR;

namespace Jantzch.Server2.Application.Shared.Notifications;

public class ErrorNotification(string message, string stackTrace) : INotification
{
    public string Message { get; } = message;
    public string StackTrace { get; } = stackTrace;
}
