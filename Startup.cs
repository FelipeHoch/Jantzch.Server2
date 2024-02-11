using FluentValidation;
using FluentValidation.AspNetCore;
using Jantzch.Server2.Application.Abstractions.Configuration;
using Jantzch.Server2.Application.Abstractions.Google;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Application.Services.Pagination;
using Jantzch.Server2.Application.Services.PropertyChecker;
using Jantzch.Server2.Infraestructure;
using Jantzch.Server2.Infraestructure.Errors;
using Jantzch.Server2.Infraestructure.Services.PropertyChecker;
using Jantzch.Server2.Infrastructure;
using Jantzch.Server2.Infrastructure.Configuration;
using Jantzch.Server2.Infrastructure.Google;
using Jantzch.Server2.Infrastructure.Json;
using Jantzch.Server2.Infrastructure.MongoDb;
using Jantzch.Server2.Infrastructure.Repositories;
using Jantzch.Server2.Infrastructure.Security;
using MediatR;
using System.Reflection;

namespace Jantzch.Server2;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
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

        services.AddRepositories();

        services.AddTransient<IPropertyCheckerService, PropertyCheckerService>();

        services.AddTransient<IPaginationService, PaginationService>();

        services.AddTransient<IDataShapingService, DataShapingService>();

        services.AddSingleton<IConfigurationService, ConfigurationService>();

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
        app.UseCors(builder =>
               builder
               .AllowAnyHeader()
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .WithExposedHeaders("X-Pagination")
               );

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHsts();

        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseMvc();        

        app.UseSwagger();

        // Enable middleware to serve swagger-ui assets(HTML, JS, CSS etc.)
        app.UseSwaggerUI();


        app.ApplicationServices.GetRequiredService<ILoggerFactory>();
    }
}
