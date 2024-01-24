using System.Text;
using System.Text.Json;

namespace Jantzch.Server2.Infrastructure.Security;

public static class Utils
{
    public static T? DecodeBase64<T>(string base64EncodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        var decodedString = Encoding.UTF8.GetString(base64EncodedBytes);
        return JsonSerializer.Deserialize<T>(decodedString);
    }
}
