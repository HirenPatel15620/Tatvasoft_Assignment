using CI.Models;
using CI.Models.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace CI_platform.helper
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
                            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                                  new Claim(ClaimTypes.Email, user.Email),
                                  new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                                  new Claim("UserId", JsonSerializer.Serialize(user.UserId))// Additional Claims

                               };

            var token = new JwtSecurityToken(
            jwtSetting.Issuer,
            jwtSetting.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(15), // Default 5 mins, max 1 day
            signingCredentials: credentials);
            //await HttpContext.SignInAsync(jwtSetting, token);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static ClaimsPrincipal? ValidateJwtToken(string token)
        {
            try
            {
                // Create the token validation parameters
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,// Validate the token's signature
                    //ValidAudience = JwtSetting.Audience,
                    //ValidIssuer = JwtSetting.Issuer,
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSetting.Key)), // Set the secret key used to sign the token
                };

                // Create a token handler
                var tokenHandler = new JwtSecurityTokenHandler();

                // Validate the token
                //ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validateToken);
                // Token is valid
                //if(claimsPrincipal == null)
                return claimsPrincipal;
            }
            catch (SecurityTokenException)
            {
                // Token validation failed
                return null;
            }
        }
    }
}
