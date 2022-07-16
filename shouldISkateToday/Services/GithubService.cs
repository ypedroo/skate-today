using shouldISkateToday.Clients.HttpResponse;
using shouldISkateToday.Clients.RequestInterface;
using shouldISkateToday.Services.Interfaces;

namespace shouldISkateToday.Services;

public class GithubService : IGithubService
{
    private readonly IGithubRequests _request;

    public GithubService(IGithubRequests request)
    {
        _request = request;
    }
    public async Task<GithubResponse> GetUserById(string username)
    {
        try
        {
            var response = await _request.GetGitHubProfile();

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}