using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Model.Manager
{
    public static class JwtHelpers
    {
        public static string CreateAccessToken(JwtSettings jwtSettings, string username, TimeSpan expiration)
        {
            var keyBytes = Encoding.UTF8.GetBytes(jwtSettings.IssuerSigningKey);
            var symmetricKey = new SymmetricSecurityKey(keyBytes);

            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim("Username", username),
                new Claim("aud", jwtSettings.ValidAudience)                       
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings.ValidIssuer,
                audience: jwtSettings.ValidAudience,
                claims: claims,
                expires: DateTime.Now.Add(expiration),
                signingCredentials: signingCredentials);

            var rawToken = new JwtSecurityTokenHandler().WriteToken(token);
            return rawToken;
        }

      
    }
}

