using FluentValidation;

namespace Jantzch.Server2.Application.Events.EditEvent;

public class EditEventCommandValidator : AbstractValidator<EditEventCommand>
{
    public EditEventCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
        RuleFor(x => x.EventType).NotNull();
        RuleFor(x => x.User).NotNull();
    }
}