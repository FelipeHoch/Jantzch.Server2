using FluentValidation;

namespace Jantzch.Server2.Application.Events.CreateEventType;

public class CreateEventTypeCommandValidator : AbstractValidator<CreateEventTypeCommand>
{
    public CreateEventTypeCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.HexColor).NotEmpty().MaximumLength(7);
    }
}
