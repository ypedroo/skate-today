using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using shouldISkateToday.Domain.Dtos;
using shouldISkateToday.Domain.Models;
using shouldISkateToday.Extensions;
using shouldISkateToday.Services.Interfaces;

namespace shouldISkateToday.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _service;
    private readonly IValidator<UserDto> _validator;
    private readonly ILogger<AuthController> _logger;


    public AuthController(IConfiguration configuration, IUserService service, IValidator<UserDto> validator,
        ILogger<AuthController> logger)
    {
        _configuration = configuration;
        _service = service;
        _validator = validator;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserDto request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            return BadRequest(new ValidationResult
            {
                Errors = validation.Errors
            });
        }

        var validUser = await _service.GetUser(request);
        if (!string.IsNullOrEmpty(validUser.UserName))
        {
            return BadRequest($"User already {request.Username} exists");
        }

        var newUser = new User();
        JwtExtensions.CreatePasswordHash(request.Password, out byte[]? passwordHash, out byte[]? passwordSalt);
        newUser.UserName = request.Username;
        newUser.PasswordHash = passwordHash;
        newUser.PasswordSalt = passwordSalt;

        var user = await _service.AddUser(newUser);

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserDto request)
    {
        var validUser = await _service.GetUser(request);
        if (string.IsNullOrEmpty(validUser.UserName))
        {
            return BadRequest("Wrong Password");
        }

        var token = GenerateAdminToken(validUser);
        var refreshToken = JwtExtensions.GenerateRefreshToken();
        await SetRefreshToken(refreshToken, validUser);
        var response =
            JwtExtensions.VerifyPasswordHash(request.Password, validUser.PasswordHash, validUser.PasswordSalt);
        _logger.LogInformation("User {User} logged in with {Response}", validUser.UserName, response);
        return response
            ? Ok(token)
            : BadRequest("Wrong Password");
    }


    [HttpPost("refresh-token")]
    public async Task<ActionResult<string>> RefreshToken(UserDto request)
    {
        var validUser = await _service.GetUser(request);
        var refreshToken = Request.Cookies["refreshToken"];
        if (!validUser.RefreshToken.Equals(refreshToken))
            return Unauthorized("Refresh Token is invalid");

        if (validUser.RefreshTokenExpires < DateTime.Now)
            return Unauthorized("Refresh Token has expired");

        var token = GenerateAdminToken(validUser);
        var newRefreshToken = JwtExtensions.GenerateRefreshToken();
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

        await _service.UpdateUserRefreshTokenUpdateUserRefreshToken(validUser);
    }

    private string GenerateAdminToken(User user)
    {
        try
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Role, "Admin"),
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: "shouldISkateToday",
                audience: "shouldISkateToday",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );
            _logger.LogInformation("Token generated: {Token}", token);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error generating token");
            Console.WriteLine(e);
            throw;
        }
    }
}