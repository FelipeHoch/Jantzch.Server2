using Jantzch.Server2.Application.Abstractions.Services;

namespace Jantzch.Server2.Infrastructure.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IAnalyticService, AnalyticService>();

        return services;
    }
}