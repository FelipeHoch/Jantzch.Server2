using FluentValidation;
using Jantzch.Server2.Application.Services.PropertyChecker;
using Jantzch.Server2.Domain.Entities.Users;
using Microsoft.AspNetCore.JsonPatch;

namespace Jantzch.Server2.Application.Users.EditUser;

public class EditUserCommandValidator
{
    public class CommandValidator : AbstractValidator<EditUserCommand.Command>
    {
        private readonly IPropertyCheckerService _propertyCheckerService;

        public CommandValidator(IPropertyCheckerService propertyCheckerService)
        {
            _propertyCheckerService = propertyCheckerService;

            RuleFor(x => x.Model).NotNull().Must(IsValidProperties);
            RuleFor(x => x.Id).NotNull();
        }

        private bool IsValidProperties(JsonPatchDocument<User> doc)
        {
            var path = doc.Operations.Select(x => x.path.Replace("/", "")).ToList();

            var fields = string.Join(",", path);

            return _propertyCheckerService.TypeHasProperties<User>(fields);
        }
    }   
}
