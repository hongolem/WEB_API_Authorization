using WebAuthority.InputModels;

namespace WebAuthority.Service
{
    public interface IAuthService
    {
        AuthenticationToken? Login(LoginRequest request);
        bool ValidateToken(string token);
    }
}
