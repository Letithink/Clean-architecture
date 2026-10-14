using Labo5.Application.Ports;
using Labo5.Domain.Ports;
using Labo5.Infrastructure.Adapters;
using Labo5.Infrastructure.Data;
using Labo5.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Labo5.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<Lab05Context>(options =>
            options.UseMySql(
                configuration.GetConnectionString("DefaultConnection"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

        return services;
    }
}
