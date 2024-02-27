using FluentValidation;

namespace Jantzch.Server2.Application.Events.EditEventType;

public class EditEventTypeCommandValidator : AbstractValidator<EditEventTypeCommand>
{
    public EditEventTypeCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.HexColor).NotEmpty().MaximumLength(10);
    }
}
