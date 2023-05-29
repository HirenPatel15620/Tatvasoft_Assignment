using CI_Platform.Entities.Auth;
using CI_Platform.Entities.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
namespace CI_Platform.Auth
{
    public static class JwtTokenHelper
    {
        public static string GenerateToken(JwtSetting jwtSetting, User user)
        {
            if (jwtSetting == null)
                return string.Empty;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.FirstName+" "+user.LastName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("Avatar", user.Avatar??"user1.png"),
                new Claim("UserId", JsonSerializer.Serialize(user.UserId))// Additional Claims
            };

            var token = new JwtSecurityToken(
            jwtSetting.Issuer,
            jwtSetting.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(15), // Default 5 mins, max 1 day
            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}