using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using shouldISkateToday.Data.Repositories.Interfaces;
using shouldISkateToday.Domain.Dtos;
using shouldISkateToday.Domain.Models;

namespace shouldISkateToday.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _dbContext;

    public AuthController(IConfiguration configuration, IUserRepository dbContext)
    {
        _configuration = configuration;
        _dbContext = dbContext;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserDto request)
    {
        var newUser = new User();
        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
        newUser.UserName = request.Username;
        newUser.PasswordHash = passwordHash;
        newUser.PasswordSalt = passwordSalt;

        var user = await _dbContext.AddUser(newUser);

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserDto request)
    {
        var validUser = await GetUser(request);
        if (string.IsNullOrEmpty(validUser.UserName))
        {
            return BadRequest("Wrong Password");
        }
        var token = GenerateAdminToken(validUser);
        var refreshToken = GenerateRefreshToken();
        await SetRefreshToken(refreshToken, validUser);
        return VerifyPasswordHash(request.Password, validUser.PasswordHash, validUser.PasswordSalt)
            ? Ok(token)
            : BadRequest("Wrong Password");
    }

    private async Task<User> GetUser(UserDto request)
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

    [HttpPost("refresh-token")]
    public async Task<ActionResult<string>> RefreshToken(UserDto request)
    {
        var validUser = await GetUser(request);
        var refreshToken = Request.Cookies["refreshToken"];
        if (!validUser.RefreshToken.Equals(refreshToken))
            return Unauthorized("Refresh Token is invalid");

        if (validUser.RefreshTokenExpires < DateTime.Now)
            return Unauthorized("Refresh Token has expired");

        var token = GenerateAdminToken(validUser);
        var newRefreshToken = GenerateRefreshToken();
        await SetRefreshToken(newRefreshToken, validUser);

        return Ok(token);
    }

    private async Task SetRefreshToken(RefreshToken refreshToken, User validUser)
    {
        var cookieOptions = new CookieOptions
        {
            Expires = refreshToken.Expires,
            HttpOnly = true
        };
        Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

        validUser.RefreshToken = refreshToken.Token;
        validUser.RefreshTokenCreated = refreshToken.Created;
        validUser.RefreshTokenExpires = refreshToken.Expires;
        
        await _dbContext.UpdateUserRefreshTokenUpdateUserRefreshToken(validUser);
    }

    private RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow
        };
        return refreshToken;
    }

    // TODO - add control error handling here
    private string GenerateAdminToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Role, "Admin"),
        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            issuer: "shouldISkateToday",
            audience: "shouldISkateToday",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    private bool VerifyPasswordHash(string requestPassword, byte[] userPasswordHash, byte[] userPasswordSalt)
    {
        using var hmac = new HMACSHA512(userPasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(requestPassword));
        return computedHash.SequenceEqual(userPasswordHash);
    }


    // TODO - implement hashing logic in a different class
    // TODO - Apply validators to the new password and username
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }
}