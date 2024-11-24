using LanguageExt.Common;
using shouldISkateToday.Data.Repositories.Interfaces;
using shouldISkateToday.Domain.Dtos;
using shouldISkateToday.Domain.Models;
using shouldISkateToday.Services.Interfaces;

namespace shouldISkateToday.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _dbContext;

    public UserService(IUserRepository dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> GetUser(UserDto request)
    {
        var user = await _dbContext.GetUser(request.Username);
        var validUser = new User();
        user.IfSucc(userFound =>
        {
            validUser.Id = userFound.Id;
            validUser.UserName = userFound.UserName;
            validUser.PasswordHash = userFound.PasswordHash;
            validUser.PasswordSalt = userFound.PasswordSalt;
            validUser.RefreshToken = userFound.RefreshToken;
            validUser.RefreshTokenCreated = userFound.RefreshTokenCreated;
            validUser.RefreshTokenExpires = userFound.RefreshTokenExpires;
        });
        return validUser;
    }

    public async Task<Result<User>> GetUserByUserName(string username)
    {
        var user = await _dbContext.GetUser(username);
        var validUser = new User();
        user.IfSucc(userFound =>
        {
            validUser.Id = userFound.Id;
            validUser.UserName = userFound.UserName;
        });
        return validUser;
    }


    public async Task<Result<User>> AddUser(User user) => await _dbContext.AddUser(user);


    public async Task<Result<string>> UpdateUserRefreshTokenUpdateUserRefreshToken(User validUser) =>
        await _dbContext.UpdateUserRefreshTokenUpdateUserRefreshToken(validUser);
}