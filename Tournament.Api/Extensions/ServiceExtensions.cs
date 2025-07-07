using Service.Contracts;
using Tournament.Core.Interfaces;
using Tournament.Data.Repositories;
using Tournament.Services;

namespace Tournament.Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(builder =>
        {
            builder.AddPolicy("AllowAll", p =>
            p.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
        });

    }

    public static void ConfigureServiceLayerServices(this IServiceCollection services)
    {
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<ITournamentDetailsService, TournamentDetailsService>();
            services.AddScoped<IGameService, GameService>();

            services.AddLazy<ITournamentDetailsService>();
            services.AddLazy<IGameService>();
        }
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITournamentDetailsRepository, TournamentDetailsRepository>();
        services.AddScoped<IGameRepository, GameRepository>();

        services.AddLazy<ITournamentDetailsRepository>();
        services.AddLazy<IGameRepository>();
    }

}


public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLazy<TService>(this IServiceCollection services) where TService : class
    {
        return services.AddScoped(provider => new Lazy<TService>(() => provider.GetRequiredService<TService>()));
    }
}

