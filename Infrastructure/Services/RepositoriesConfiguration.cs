using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Jantzch.Server2.Application.Abstractions.Services;
using Jantzch.Server2.Domain.Entities.Services.Storage;
using Jantzch.Server2.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;

namespace Jantzch.Server2.Infrastructure.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IAnalyticService, AnalyticService>();

        services.Configure<S3Settings>(
               configuration.GetSection("S3Settings"));

        services.AddAWSService<IAmazonS3>(new AWSOptions
        {
            Credentials = new BasicAWSCredentials(configuration["S3Settings:AccessKey"], configuration["S3Settings:SecretKey"]),

            Region = RegionEndpoint.USEast2 // Replace with your desired region
        });

        services.AddScoped<IStorageService, StorageService>();

        return services;
    }
}