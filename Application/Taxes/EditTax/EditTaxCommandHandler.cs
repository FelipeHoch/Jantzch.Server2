using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using Jantzch.Server2.Domain.Entities.Taxes;
using Jantzch.Server2.Domain.Entities.Taxes.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Application.Taxes.EditTax;

public class EditTaxCommandHandler
{
    public class Handler : IRequestHandler<EditTaxCommand.Command, Tax>
    {
        private readonly ITaxesRepository _taxRepository;

        public Handler(ITaxesRepository taxRepository)
        {
            _taxRepository = taxRepository;
        }

        public async Task<Tax> Handle(EditTaxCommand.Command request, CancellationToken cancellationToken)
        {
            var tax = await _taxRepository.GetByIdAsync(new ObjectId(request.Id), cancellationToken);

            if (tax is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { message = TaxErrorMessages.NOT_FOUND });
            }

            tax.Name = request.Model.Name;
            tax.Type = request.Model.Type;
            tax.Value = request.Model.Value;

            await _taxRepository.UpdateAsync(tax);

            await _taxRepository.SaveChangesAsync(cancellationToken);

            return tax;
        }
    }
}
