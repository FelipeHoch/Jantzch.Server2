using AutoMapper;
using Jantzch.Server2.Application.OrderReports.CreateManualReport;
using Jantzch.Server2.Domain.Entities.Orders;

namespace Jantzch.Server2.Application.OrderReports;

public class MappingOrderReport : Profile
{    
    public MappingOrderReport()
    {
        CreateMap<OrderReport, OrderReportResponse>();
        CreateMap<CreateManualReportCommand, OrderExport>()
            .ForMember(dest => dest.Materials, opt => opt.MapFrom(src => src.MaterialsUsed))
            .ForMember(dest => dest.ManPower, opt => opt.MapFrom(src => src.Value))
            .AfterMap(
            (src, dest) =>
            {
                dest.CalculateTotalMaterialCost();
            });
    }
}
