using System.Text.Json;
using LanguageExt.Common;
using Refit;
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

    public async Task<Result<string>> GetUserById(string username)
    {
        try
        {
           return await _request.GetGitHubProfile(username);
        }
        catch (Exception exception)
        {
            return new Result<string>(exception);
        }
    }
}