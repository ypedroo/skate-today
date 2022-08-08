using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using shouldISkateToday.Data.Contexts;
using shouldISkateToday.Data.Repositories.Interfaces;
using shouldISkateToday.Domain.Models;

namespace shouldISkateToday.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserContext _context;

    public UserRepository(UserContext context)
    {
        _context = context;
    }

    public async Task<Result<User>> AddUser(User user)
    {
        try
        {
            var createdUser = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return createdUser.Entity;
        }
        catch (Exception exception)
        {
            return new Result<User>(exception);
        }
    }

    public async Task<Result<User>> GetUser(string userId)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userId);

            return user ?? new Result<User>();
        }
        catch (Exception exception)
        {
            return new Result<User>(exception);
        }
    }

    public async Task<Result<string>> UpdateUserRefreshTokenUpdateUserRefreshToken(User validUser)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == validUser.Id);
            if (user == null)
            {
                var error = new KeyNotFoundException("User Not Provided");
                return new Result<string>(error);
            }

            user.RefreshToken = validUser.RefreshToken;
            user.RefreshTokenExpires = validUser.RefreshTokenExpires;
            user.RefreshTokenCreated = validUser.RefreshTokenCreated;
            await _context.SaveChangesAsync();
            return user.UserName;
        }
        catch (Exception exception)
        {
            return new Result<string>(exception);
        }
    }
}