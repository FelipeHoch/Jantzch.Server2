using AutoMapper;
using Jantzch.Server2.Domain.Entities.ReportConfigurations;

namespace Jantzch.Server2.Application.ReportConfigurations;

public class MappingReportConfiguration : Profile
{
    public MappingReportConfiguration()
    {
        CreateMap<ReportConfiguration, ReportConfigurationResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MongoId));
    }
}
