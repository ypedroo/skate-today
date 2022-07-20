using Refit;

namespace shouldISkateToday.Clients.RequestInterface;

public interface IGoogleMapsRequests
{
    [Get("/json")]
    Task<string> GetSkateParks(string query, string key);
}