using FluentValidation;
using Jantzch.Server2.Application.Users.Models;
using Jantzch.Server2.Infrastructure.Security;

namespace Jantzch.Server2.Application.Users.CreateUser;

public class CreateUserCommandValidator
{
    public class UserFromIdpDtoValidator : AbstractValidator<UserFromIdpDto>
    {
        public UserFromIdpDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
            RuleFor(x => x.Provider).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Role).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        }
    }

    public class UserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public UserCommandValidator()
        {
            RuleFor(x => x.Data).NotEmpty().Must(BeAValidObject);
        }

        private bool BeAValidObject(string data)
        {
            try
            {
                var decodedObject = Utils.DecodeBase64<UserFromIdpDto>(data);     
                
                return new UserFromIdpDtoValidator().Validate(decodedObject).IsValid;
            }
            catch
            {                
                return false;
            }
        }
    }
}
