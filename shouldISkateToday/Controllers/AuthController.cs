using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using shouldISkateToday.Domain.Dtos;
using shouldISkateToday.Domain.Models;

namespace shouldISkateToday.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public static User user = new();

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserDto request)
    {
        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
        user.UserName = request.Username;
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        return Ok(user);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserDto request)
    {
        if (user.UserName != request.Username) return BadRequest("Login Failed");
        return VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt)
            ? Ok( GenerateToken(user))
            : BadRequest("Wrong Password");
    }

    // TODO - add control error handling here
    private string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
        
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
        using var hmac = new System.Security.Cryptography.HMACSHA512(userPasswordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(requestPassword));
        return computedHash.SequenceEqual(userPasswordHash);
    }


    // TODO - implement hashing logic in a differente class
    // TODO - Apply validators to the new password and username
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }
}