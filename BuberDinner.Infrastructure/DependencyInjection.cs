
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Services;
using BuberDinner.Infrastructure.Authentication;
using BuberDinner.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BuberDinner.Infrastructure;
public static class DependencyInjection{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
                        Microsoft.Extensions.Configuration.ConfigurationManager configuration)
    {   services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>(); 
        return services;
    }
}