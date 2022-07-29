using LanguageExt.Common;
using shouldISkateToday.Domain.Dtos;
using shouldISkateToday.Domain.Models;

namespace shouldISkateToday.Services.Interfaces;

public interface IUserService
{
     Task<User> GetUser(UserDto request);
     Task<Result<User>> AddUser(User user);
     Task<Result<string>> UpdateUserRefreshTokenUpdateUserRefreshToken(User validUser);
}