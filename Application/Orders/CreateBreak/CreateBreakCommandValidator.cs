using FluentValidation;
using MongoDB.Bson;

namespace Jantzch.Server2.Application.Orders.CreateBreak;

public class CreateBreakCommandValidator : AbstractValidator<CreateBreakCommand.Command>
{
    public CreateBreakCommandValidator()
    {
        RuleFor(x => x.Descriptive).NotNull().MaximumLength(500).WithMessage("Descrição acima de 500 carácteres");

        RuleFor(x => x.Id).NotNull().Length(24).WithMessage("Id inválido");
    }
}
