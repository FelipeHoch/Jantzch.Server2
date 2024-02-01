using AutoMapper;
using Jantzch.Server2.Domain.Entities.Orders;

namespace Jantzch.Server2.Application.OrderReports;

public class MappingOrderReport : Profile
{    
    public MappingOrderReport()
    {
        CreateMap<OrderReport, OrderReportResponse>();
    }
}
