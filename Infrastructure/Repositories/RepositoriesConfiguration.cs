using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Clients.Deals;
using Jantzch.Server2.Domain.Entities.Events;
using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Domain.Entities.ReportConfigurations;
using Jantzch.Server2.Domain.Entities.Taxes;
using Jantzch.Server2.Domain.Entities.Users;

namespace Jantzch.Server2.Infrastructure.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IMaterialsRepository, MaterialsRepository>();
        services.AddScoped<IGroupsMaterialRepository, GroupsMaterialRepository>();
        services.AddScoped<ITaxesRepository, TaxesRepository>();
        services.AddScoped<IReportConfigurationRepository, ReportConfigurationRepository>();
        services.AddScoped<IClientsRepository, ClientsRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderReportRepository, OrderReportRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IEventTypeRepository, EventTypeRepository>();
        services.AddScoped<IPotentialOrderRepository, PotentialOrderRepository>();
        services.AddScoped<IDealRepository, DealRepository>();

        return services;
    }
}