using Jantzch.Server2.Domain.Entities.Taxes;
using Jantzch.Server2.Domain.Entities.Taxes.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Application.Taxes.DeleteTax;

public class DeleteTaxCommandHandler
{
    public class Handler : IRequestHandler<DeleteTaxCommand>
    {
        private readonly ITaxesRepository _taxesRepository;

        public Handler(ITaxesRepository taxesRepository)
        {
            _taxesRepository = taxesRepository;
        }

        public async Task Handle(DeleteTaxCommand request, CancellationToken cancellationToken)
        {
            var tax = await _taxesRepository.GetByIdAsync(ObjectId.Parse(request.Id), cancellationToken);

            if (tax is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { message = TaxErrorMessages.NOT_FOUND });
            }

            await _taxesRepository.DeleteAsync(tax);

            await _taxesRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
