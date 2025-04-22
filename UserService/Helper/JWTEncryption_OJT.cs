using Microsoft.IdentityModel.Tokens;
using Model.Manager;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Models.Response;

namespace UserService.Helper
{
    public class JWTEncryption_OJT
    {
        public static string JWTEncryption(JwtSettings jwtSettings, UserData_WithoutPassword user, TimeSpan expiration)
        {
            var keyBytes = Encoding.UTF8.GetBytes(jwtSettings.IssuerSigningKey);
            var symmetricKey = new SymmetricSecurityKey(keyBytes);

            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                
                new Claim("Id", user.Id.ToString()),
                
                new Claim("LastName", user.LastName),
                new Claim("FirstName", user.FirstName),
                new Claim("MiddleName", user.MiddleName),
                new Claim("Email", user.Email),
                new Claim("Phone", user.Phone),
                new Claim("Address", user.Address),
                new Claim("Username", user.Username),
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

        public static UserData_WithoutPassword JWTDecryption(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);


            var user = new UserData_WithoutPassword
            {
                Id = int.Parse(jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value ?? "0"),
                LastName = jwtToken.Claims.FirstOrDefault(c => c.Type == "LastName")?.Value,
                FirstName = jwtToken.Claims.FirstOrDefault(c => c.Type == "FirstName")?.Value,
                MiddleName = jwtToken.Claims.FirstOrDefault(c => c.Type == "MiddleName")?.Value,
                Email = jwtToken.Claims.FirstOrDefault(c => c.Type == "Email")?.Value,
                Phone = jwtToken.Claims.FirstOrDefault(c => c.Type == "Phone")?.Value,
                Address = jwtToken.Claims.FirstOrDefault(c => c.Type == "Address")?.Value,
                Username = jwtToken.Claims.FirstOrDefault(c => c.Type == "Username")?.Value
            };

            return user;
        }




    }
}
