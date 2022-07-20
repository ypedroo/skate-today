using Refit;
using shouldISkateToday.Clients.RequestInterface;
using shouldISkateToday.Services;
using shouldISkateToday.Services.Interfaces;

namespace shouldISkateToday.Infra;

public static class DependencyContainer
{
    public static IServiceCollection RegisterDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IGoogleMapService, GoogleMapService>();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddMvc().AddJsonOptions(options => { options.JsonSerializerOptions.IncludeFields = true; });
        services.AddRefitClient<IGoogleMapsRequests>().ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri(configuration.GetValue<string>("GOOGLE_MAPS_PLACE_URL"));
        });
        return services;
    }
}