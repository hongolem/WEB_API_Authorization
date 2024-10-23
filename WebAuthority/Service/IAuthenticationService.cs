using WebAuthority.InputModels;

namespace WebAuthority.Service
{
    public interface IAuthenticationService
    {
        string Login(LoginRequest request);
    }
}
