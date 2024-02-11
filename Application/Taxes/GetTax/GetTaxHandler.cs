using AutoMapper;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Domain.Entities.Taxes;
using Jantzch.Server2.Domain.Entities.Taxes.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Dynamic;

namespace Jantzch.Server2.Application.Taxes.GetTax;

public class GetTaxHandler
{
    public class Handler : IRequestHandler<TaxQuery, ExpandoObject>
    {
        private readonly ITaxesRepository _taxesRepository;

        private readonly IMapper _mapper;

        private readonly IDataShapingService _dataShapingService;

        public Handler(IMapper mapper, IDataShapingService dataShapingService, ITaxesRepository taxesRepository)
        {
            _mapper = mapper;
            _dataShapingService = dataShapingService;
            _taxesRepository = taxesRepository;
        }

        public async Task<ExpandoObject> Handle(TaxQuery request, CancellationToken cancellationToken)
        {
            var tax = await _taxesRepository.GetByIdAsync(new ObjectId(request.Id), cancellationToken);

            if (tax is null)
            {
                throw new RestException(System.Net.HttpStatusCode.NotFound, new { message = TaxErrorMessages.NOT_FOUND });
            }

            var taxResponse = _mapper.Map<TaxResponse>(tax);

            var taxShaped = _dataShapingService.ShapeData(taxResponse, request.Fields);

            return taxShaped;
        }
    }
}
