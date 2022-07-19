using Refit;

namespace shouldISkateToday.Clients.RequestInterface;

public interface IGithubRequests
{
    [Get("/users/{username}")]
    [Headers("Authorization: token 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", "Accept: application/vnd.github.v3+json",
        "User-Agent: demo")]
    Task<string> GetGitHubProfile(string username);
}