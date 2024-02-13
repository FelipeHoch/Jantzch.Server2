using Jantzch.Server2.Application.Abstractions.Jwt;
using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using Jantzch.Server2.Domain.Entities.Taxes;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using System.Net;

namespace Jantzch.Server2.Application.Taxes.CreateTax;

public class CreateTaxCommandHandler
{
    public class Handler : IRequestHandler<CreateTaxCommand, Tax>
    {
        private readonly ITaxesRepository _taxRepository;

        private readonly IJwtService _jwtService;

        public Handler(ITaxesRepository TaxRepository, IJwtService jwtService)
        {
            _taxRepository = TaxRepository;

            _jwtService = jwtService;
        }

        public async Task<Tax> Handle(CreateTaxCommand request, CancellationToken cancellationToken)
        {
            var lastTax = await _taxRepository.LastTaxInsertedAsync(cancellationToken);

            var code = 0;

            if (lastTax != null)
                code = lastTax.Code + 1;

            var tax = new Tax
            {
                Name = request.Name,
                Type = request.Type,
                CreatedBy = _jwtService.GetNameFromToken(),
                Value = request.Value,
                Code = code,
            };

            await _taxRepository.AddAsync(tax, cancellationToken);

            await _taxRepository.SaveChangesAsync(cancellationToken);

            return tax;
        }
    }
}
