using FluentValidation;
using FluentValidation.AspNetCore;
using Jantzch.Server2.Application.Abstractions.Google;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Application.Services.Pagination;
using Jantzch.Server2.Application.Services.PropertyChecker;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Domain.Entities.ReportConfigurations;
using Jantzch.Server2.Domain.Entities.Taxes;
using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Infraestructure;
using Jantzch.Server2.Infraestructure.Errors;
using Jantzch.Server2.Infraestructure.Services.PropertyChecker;
using Jantzch.Server2.Infrastructure;
using Jantzch.Server2.Infrastructure.Google;
using Jantzch.Server2.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jantzch.Server2;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var conf = MongoClientSettings.FromConnectionString(Environment.GetEnvironmentVariable("MONGODB_URI") ?? "mongodb://localhost:27017");

        conf.ApplicationName = "JANTZCH";

        var mongoClient = new MongoClient(conf);

        var database = mongoClient.GetDatabase("jantzch");

        var pack = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCaseElementNameConvention", pack, t => true);        

        // Is necessary, because the mongo provider to ef core, doesn't nested objects
        services.AddSingleton(database);

        services.AddDbContext<JantzchContext>(opt => opt.UseMongoDB(mongoClient, database.DatabaseNamespace.DatabaseName));

        services.AddMediatR(
            config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
        );

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(DBContextTransactionPipelineBehavior<,>)
        );

        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<Startup>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddCors();

        services.AddLocalization(x => x.ResourcesPath = "Resources");

        services.AddScoped<IMaterialsRepository, MaterialsRepository>();

        services.AddScoped<IGroupsMaterialRepository, GroupsMaterialRepository>();

        services.AddScoped<ITaxesRepository, TaxesRepository>();

        services.AddScoped<IReportConfigurationRepository, ReportConfigurationRepository>();

        services.AddScoped<IClientsRepository, ClientsRepository>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddScoped<IOrderReportRepository, OrderReportRepository>();

        services.AddTransient<IPropertyCheckerService, PropertyCheckerService>();

        services.AddTransient<IPaginationService, PaginationService>();

        services.AddTransient<IDataShapingService, DataShapingService>();        

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services
           .AddMvc(opt =>
           {
               opt.Filters.Add(typeof(ValidatorActionFilter));
               opt.EnableEndpointRouting = false;

               opt.InputFormatters.Insert(0, JsonPatchFormatter.GetJsonPatchInputFormatter());
           })
           .AddJsonOptions(
                opt =>
                {
                    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

                    opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(new LowerCaseJsonNamingPolicy()));
                })
           .AddNewtonsoftJson();
            

        services.AddHttpClient<IGoogleMapsService, GoogleMapsService>((provider, httpClinet) => 
        {
            httpClinet.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/");
        });

        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseMvc();

        app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

        app.UseSwagger();

        // Enable middleware to serve swagger-ui assets(HTML, JS, CSS etc.)
        app.UseSwaggerUI();


        app.ApplicationServices.GetRequiredService<ILoggerFactory>();
    }
}
