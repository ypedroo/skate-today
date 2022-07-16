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
        services.AddTransient<IGithubService, GithubService>();
        services.AddRefitClient<IGithubRequests>().ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri(configuration.GetValue<string>("GIT_HUB_API_URL"));
        });
        // services.AddRefitClient<IGithubRequests>().ConfigureHttpClient(c =>
        // {
        //     c.BaseAddress = new Uri(configuration.GetValue<string>("STORM_GLASS_URL"));
        // });
        return services;
    }
}