using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Jantzch.Server2.Infrastructure.Security;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(op => {
            op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(op =>
        {
            op.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                ValidAudiences = Environment.GetEnvironmentVariable("JWT_AUDIENCES").Split(","),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")))
            };

            op.Events = new JwtBearerEvents
            {
               OnMessageReceived = context =>
               {
                   var accessToken = context.Request.Query["access_token"];
                   var path = context.HttpContext.Request.Path;
                   if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/orderHub"))
                   {
                       context.Token = accessToken;
                   }
                   return Task.CompletedTask;
               }
            };
        });

        services.AddAuthorization(auth =>
        {
            auth.AddPolicy(JwtBearerDefaults.AuthenticationScheme,
                new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());
        });

        return services;
    }
}
