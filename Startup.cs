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

namespace Jantzch.Server2;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var conf = MongoClientSettings.FromConnectionString(Environment.GetEnvironmentVariable("MONGODB_URI") ?? "mongodb://localhost:27017");

        conf.ApplicationName = "JANTZCH";

        var mongoClient = new MongoClient(conf);

        var database = mongoClient.GetDatabase("jantzch");

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

        services.AddTransient<IPropertyCheckerService, PropertyCheckerService>();

        services.AddTransient<IPaginationService, PaginationService>();

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
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseMvc();

        app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());


        app.ApplicationServices.GetRequiredService<ILoggerFactory>();
    }
}
