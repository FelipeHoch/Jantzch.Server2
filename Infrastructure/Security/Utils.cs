using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Jantzch.Server2.Infrastructure.Security;

public static class Utils
{
    public static T? DecodeBase64<T>(string base64EncodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        var decodedString = Encoding.UTF8.GetString(base64EncodedBytes);
        return JsonSerializer.Deserialize<T>(decodedString);
    }

    public static string ObjectToBase64<T>(T obj)
    {
        var jsonString = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
        });        
        var base64EncodedBytes = Encoding.UTF8.GetBytes(jsonString);
        return Convert.ToBase64String(base64EncodedBytes);
    }

    public static T? Base64ToObject<T>(string base64EncodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        var jsonString = Encoding.UTF8.GetString(base64EncodedBytes);
        return JsonSerializer.Deserialize<T>(jsonString);
    }
}
