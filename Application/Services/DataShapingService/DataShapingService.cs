using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Services.DataShapingService.Constants;
using Jantzch.Server2.Application.Services.PropertyChecker;
using Jantzch.Server2.Infraestructure.Errors;
using System.Dynamic;
using System.Net;

namespace Jantzch.Server2.Application.Services.DataShapingService;

public interface IDataShapingService
{
    IEnumerable<ExpandoObject> ShapeDataList<T>(List<T> data, string fields);

    ExpandoObject ShapeData<T>(T data, string fields);
}

public class DataShapingService : IDataShapingService
{
    private readonly IPropertyCheckerService _propertyCheckerService;

    public DataShapingService(IPropertyCheckerService propertyCheckerService)
    {
        _propertyCheckerService = propertyCheckerService;
    }

    public IEnumerable<ExpandoObject> ShapeDataList<T>(List<T> data, string fields)
    {
        if (!_propertyCheckerService.TypeHasProperties<T>(fields))
        {
            throw new RestException(HttpStatusCode.BadRequest, new { message = DataShapingErrorMessages.INVALID_FIELDS });
        }

        return data.ShapeData<T>(fields);
    }

    public ExpandoObject ShapeData<T>(T data, string fields)
    {
        if (!_propertyCheckerService.TypeHasProperties<T>(fields))
        {
            throw new RestException(HttpStatusCode.BadRequest, new { message = DataShapingErrorMessages.INVALID_FIELDS });
        }

        return data.ShapeData(fields);
    }
}