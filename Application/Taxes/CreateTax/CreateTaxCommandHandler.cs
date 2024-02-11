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
        private readonly ITaxesRepository _TaxRepository;

        public Handler(ITaxesRepository TaxRepository)
        {
            _TaxRepository = TaxRepository;
        }

        public async Task<Tax> Handle(CreateTaxCommand request, CancellationToken cancellationToken)
        {
            var lastTax = await _TaxRepository.LastTaxInsertedAsync(cancellationToken);

            var code = 0;

            if (lastTax != null)
                code = lastTax.Code + 1;

            var tax = new Tax
            {
                Name = request.Name,
                Type = request.Type,
                // TODO: Get user from token
                CreatedBy = "Mock",
                Value = request.Value,
                Code = code,
            };

            await _TaxRepository.AddAsync(tax, cancellationToken);

            await _TaxRepository.SaveChangesAsync(cancellationToken);

            return tax;
        }
    }
}
