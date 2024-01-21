﻿using FluentValidation.AspNetCore;
using FluentValidation;
using System.Reflection;
using Jantzch.Server2.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Jantzch.Server2.Infraestructure.Errors;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Infrastructure.Repositories;
using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using Jantzch.Server2.Application.Services.PropertyChecker;
using Jantzch.Server2.Application.Services.Pagination;
using Jantzch.Server2.Infraestructure.Services.PropertyChecker;
using Jantzch.Server2.Application.Services.DataShapingService;
using Microsoft.AspNetCore.Builder;
using Jantzch.Server2.Domain.Entities.Taxes;
using Jantzch.Server2.Domain.Entities.ReportConfigurations;
using Jantzch.Server2.Infrastructure.Google;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Application.Abstractions.Google;

namespace Jantzch.Server2;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var conf = MongoClientSettings.FromConnectionString(Environment.GetEnvironmentVariable("MONGODB_URI") ?? "mongodb://localhost:27017");

        conf.ApplicationName = "JANTZCH";

        var mongoClient = new MongoClient(conf);

        var database = mongoClient.GetDatabase("jantzch");

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

        services.AddTransient<IPropertyCheckerService, PropertyCheckerService>();

        services.AddTransient<IPaginationService, PaginationService>();

        services.AddTransient<IDataShapingService, DataShapingService>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

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
