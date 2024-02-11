using Jantzch.Server2.Application.Abstractions.Configuration;

namespace Jantzch.Server2.Infrastructure.Configuration;

public class ConfigurationService : IConfigurationService
{
    public string GetRedirectAuth()
    {
        return Environment.GetEnvironmentVariable("REDIRECT_AUTH") + "/#/auth/";
    }
}
