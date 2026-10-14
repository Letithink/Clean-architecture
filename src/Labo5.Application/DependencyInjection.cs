using Labo5.Application.UseCases.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Labo5.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<LoginUseCase>();
        services.AddScoped<RegisterUseCase>();
        return services;
    }
}
