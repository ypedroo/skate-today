using Refit;
using shouldISkateToday.Clients.HttpResponse;

namespace shouldISkateToday.Clients.RequestInterface;

public interface IGithubRequests
{
    [Get("/users")]
    Task<GithubResponse> GetGitHubProfile();
}