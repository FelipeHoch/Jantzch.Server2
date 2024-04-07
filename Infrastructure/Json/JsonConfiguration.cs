using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Jantzch.Server2.Infrastructure.Json;

public static class ServiceCollectionExtensions
{
    public static IMvcBuilder AddCustomJsonOptions(this IMvcBuilder builder)
    {
        return builder
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(new LowerCaseJsonNamingPolicy()));                
            })
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                opt.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                {
                    NamingStrategy = new Newtonsoft.Json.Serialization.CamelCaseNamingStrategy()
                };
                opt.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                opt.SerializerSettings.Formatting = Formatting.Indented;
            });
    }
}
