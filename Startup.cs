using FluentValidation;
using FluentValidation.AspNetCore;
using Jantzch.Server2.Application.Abstractions.Configuration;
using Jantzch.Server2.Application.Abstractions.Excel;
using Jantzch.Server2.Application.Abstractions.Google;
using Jantzch.Server2.Application.Abstractions.Jwt;
using Jantzch.Server2.Application.Orders;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Application.Services.Pagination;
using Jantzch.Server2.Application.Services.PropertyChecker;
using Jantzch.Server2.Domain.Entities.Clients.Services;
using Jantzch.Server2.Infraestructure;
using Jantzch.Server2.Infraestructure.Errors;
using Jantzch.Server2.Infraestructure.Services.PropertyChecker;
using Jantzch.Server2.Infrastructure;
using Jantzch.Server2.Infrastructure.Configuration;
using Jantzch.Server2.Infrastructure.Excel;
using Jantzch.Server2.Infrastructure.Google;
using Jantzch.Server2.Infrastructure.Json;
using Jantzch.Server2.Infrastructure.Jwt;
using Jantzch.Server2.Infrastructure.MongoDb;
using Jantzch.Server2.Infrastructure.Repositories;
using Jantzch.Server2.Infrastructure.Security;
using Jantzch.Server2.Infrastructure.Services;
using MediatR;
using System.Reflection;


namespace Jantzch.Server2;

public class Startup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureMongoDB();        

        services.AddMediatR(
            config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
        );

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(DBContextTransactionPipelineBehavior<,>)
        );

        services.AddJwtAuthentication();

        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<Startup>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();        

        services.AddLocalization(x => x.ResourcesPath = "Resources");

        services.AddSignalR();

        services.AddRepositories();

        services.AddServices(configuration);

        services.AddTransient<IClientService, ClientService>();

        services.AddTransient<IPropertyCheckerService, PropertyCheckerService>();

        services.AddTransient<IPaginationService, PaginationService>();

        services.AddTransient<IDataShapingService, DataShapingService>();

        services.AddTransient<IExcelService, ClosedXmlService>();

        services.AddSingleton<IConfigurationService, ConfigurationService>();

        services.AddScoped<IJwtService, JwtService>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services
           .AddMvc(opt =>
           {
               opt.Filters.Add(typeof(ValidatorActionFilter));
               opt.EnableEndpointRouting = false;

               opt.InputFormatters.Insert(0, JsonPatchFormatter.GetJsonPatchInputFormatter());
           })
           .AddCustomJsonOptions();
            

        services.AddHttpClient<IGoogleMapsService, GoogleMapsService>((provider, httpClinet) => 
        {
            httpClinet.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/");
        });

        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseCors(opt =>
            {
                opt
                .WithOrigins("http://localhost:8080", "http://localhost:5016", "http://192.168.100.10:5016")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .SetIsOriginAllowed(x => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithExposedHeaders("X-Pagination");
            });
        }
        else
        {
            app.UseCors(opt =>
            {
                opt
                .WithOrigins(Environment.GetEnvironmentVariable("CLIENT_DOMAIN"), Environment.GetEnvironmentVariable("LOGIN_DOMAIN"))
                 .SetIsOriginAllowedToAllowWildcardSubdomains()
                .SetIsOriginAllowed(x => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithExposedHeaders("X-Pagination");
            });
        }        

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHsts();

        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseMvc();        

        app.UseSwagger();

        // Enable middleware to serve swagger-ui assets(HTML, JS, CSS etc.)
        app.UseSwaggerUI();        

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<OrderHub>("/orderhub")
            .RequireAuthorization();
            // outras rotas...
        });


        app.ApplicationServices.GetRequiredService<ILoggerFactory>();
    }
}
