using shouldISkateToday.Clients.HttpResponse;

namespace shouldISkateToday.Services.Interfaces;

public interface IGithubService
{
    Task<GithubResponse> GetUserById(string username);
}