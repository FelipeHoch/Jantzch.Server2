using FluentValidation;

namespace Jantzch.Server2;

public class CreateEventToManyUsersCommandValidator : AbstractValidator<CreateEventToManyUsersCommand>
{
    public CreateEventToManyUsersCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
        RuleFor(x => x.EventType).NotNull();
        RuleFor(x => x.Users).NotNull().NotEmpty();
        RuleFor(x => x.NotifyUser).NotNull();
    }    
}
