using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Refit;
using shouldISkateToday.Clients.RequestInterface;
using shouldISkateToday.Data.Contexts;
using shouldISkateToday.Data.Repositories;
using shouldISkateToday.Data.Repositories.Interfaces;
using shouldISkateToday.Services;
using shouldISkateToday.Services.Interfaces;

namespace shouldISkateToday.Infra;

public static class DependencyContainer
{
    public static IServiceCollection RegisterDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IGoogleMapService, GoogleMapService>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IUserFavoritesService, UserFavoriteServices>();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddMvc().AddJsonOptions(options => { options.JsonSerializerOptions.IncludeFields = true; });
        services.AddRefitClient<IGoogleMapsRequests>().ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri(configuration.GetValue<string>("GOOGLE_MAPS_PLACE_URL"));
        });
        var connectionString = configuration["dbContextSettings:ConnectionString"];
        services.AddDbContext<UserContext>(options =>
            options.UseNpgsql(connectionString)
        );
        return services;
    }
}