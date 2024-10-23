using System.Security.Claims;
using WebAuthority.InputModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WebAuthority.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        public string Login(LoginRequest request)
        {
            if (request.Username == "ja" && request.Password == "beruska")
            {
                return CreateAuthenticationToken(request);
            }
            return null;
        }

        private string CreateAuthenticationToken(LoginRequest request)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(JwtRegisteredClaimNames.Sub,"1") // user id
            };
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes("supersecretkey");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = "authority_server",
                Audience = "authority_client",
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenhandler.CreateToken(tokenDescriptor);
            return tokenhandler.WriteToken(token);
        }
    }

    class AuthenticationToken
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
