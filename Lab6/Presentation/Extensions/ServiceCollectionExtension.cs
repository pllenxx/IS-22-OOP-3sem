using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using DataAccess.Enums;

namespace Presentation.Extensions;

internal static class ServiceCollectionExtension
{
    public static IServiceCollection AddCooliesAuthentication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(2);
                options.SlidingExpiration = true;
            });
        return serviceCollection;
    }

    public static IServiceCollection AddRoles(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            options.AddPolicy("Supervisor", policy =>
            {
                EmployeeType[] allowedRole = { EmployeeType.Supervisor };
                policy
                    .RequireClaim(ClaimTypes.Role, allowedRole.ToString())
                    .RequireAuthenticatedUser()
                    .Build();
            });

            options.AddPolicy("Subordinate", policy =>
            {
                EmployeeType[] allowedRole = { EmployeeType.Subordinate };
                policy
                    .RequireClaim(ClaimTypes.Role, allowedRole.ToString())
                    .RequireAuthenticatedUser()
                    .Build();
            });
        });
    }
}