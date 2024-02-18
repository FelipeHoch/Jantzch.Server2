using FluentValidation;

namespace Jantzch.Server2.Application.Events.CreateEvent;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
        RuleFor(x => x.EventType).NotNull();
        RuleFor(x => x.User).NotNull();
        RuleFor(x => x.NotifyUser).NotNull();
    }
}