using LanguageExt.Common;

namespace shouldISkateToday.Services.Interfaces;

public interface IGithubService
{
    Task<Result<string>>GetUserById(string username);
}