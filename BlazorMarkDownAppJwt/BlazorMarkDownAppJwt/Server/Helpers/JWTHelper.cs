using BlazorMarkDownAppJwt.Server.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorMarkDownAppJwt.Server.Helpers
{
    public static class JWTHelper
    {
        public static string? CreateJWT(User user)
        {
            var secretkey = GetSecretKey();
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, user.Email)
            };

                var token = new JwtSecurityToken(issuer: "domain.com", audience: "domain.com", claims: claims, expires: DateTime.Now.AddMinutes(10), signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            return null;
        }

        public static SymmetricSecurityKey GetSecretKey()
        {
            return new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("THIS IS THE SECRET KEY"));
        }
    }
}
