using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAuthority.InputModels;
using WebAuthority.Service;

namespace WebAuthority.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthService _as;
        private ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger)
        {
            _as = authService;
            _logger = logger;
        }

        [HttpPost("/login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try 
            {
                var token = _as.Login(request);
                return Ok(token);
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPost("/introspection")]
        public IActionResult Introspection([FromBody] IntrospectionRequest request)
        {
            if (!_as.ValidateToken(request.Token))
            {
                return Unauthorized(); // může vracet i třeba {active: false}
            }
            return Ok(); // může vracet i rozbalený token
        }
    }
}
