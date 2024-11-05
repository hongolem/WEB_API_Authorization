using System.Security.Claims;
using WebAuthority.InputModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WebAuthority.Service
{
    public class AuthService : IAuthService
    {
        private AuthCredentials[] validCredentials = [ new("ja","beruska"), new("x","mravenec")];

        private List<string> validTokens = new List<string>();
        public AuthenticationToken? Login(LoginRequest request)
        {
            AuthCredentials current = new(request.Username, request.Password);
            if (!validCredentials.Contains(current))
            {
                throw new AuthenticationException();
            }
            var token = CreateAuthenticationToken(request);
            validTokens.Add(token);
            return new AuthenticationToken { Name = "authentication_token", Value = token };
        }

        public bool ValidateToken(string token)
        {
            if (!validTokens.Contains(token))
            {
                return false;
            }
            var handler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes("supersecretkeywhichshouldbenevershared");
            try
            {
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = "authority_server",
                    ValidAudience = "authority_client"
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
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
            var tokenKey = Encoding.UTF8.GetBytes("supersecretkeywhichshouldbenevershared");
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

    public class AuthenticationToken
    {
        public required string Name { get; set; }
        public required string Value { get; set; }
    }

    public class AuthenticationException: Exception
    {

    }

    public struct AuthCredentials
    {
        public AuthCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
