using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TrainineeAPI.Models;

public class JWTService : IJWTService
{
    private readonly IConfiguration _configuration;
    public JWTService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateToken(User userBody)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name,userBody.Username),
            new Claim(ClaimTypes.Role,userBody.Role),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:Expiry"])),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}