using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAuthority.InputModels;

namespace WebAuthority.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService _authenticationService;
        private ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = _authenticationService.Login(request);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }
    }
}
