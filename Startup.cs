using FluentValidation.AspNetCore;
using FluentValidation;
using System.Reflection;
using Jantzch.Server2.Infraestructure;
using MediatR;

namespace Jantzch.Server2;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMediatR(
            config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
        );

        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<Startup>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddCors();

        services
           .AddMvc(opt =>
           {
               opt.Filters.Add(typeof(ValidatorActionFilter));
               opt.EnableEndpointRouting = false;
           })
           .AddJsonOptions(
               opt =>
                   opt.JsonSerializerOptions.DefaultIgnoreCondition = System
                       .Text
                       .Json
                       .Serialization
                       .JsonIgnoreCondition
                       .WhenWritingNull
           );

        services.AddMongoRepository(new MongoRepositoryOptions
        {
            ClientName = "JANTZCH",
            ConnectionString = Environment.GetEnvironmentVariable("MONGODB_URI") ?? "mongodb://localhost:27017"
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMvc();

        app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    }
}
