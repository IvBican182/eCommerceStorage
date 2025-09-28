
using Microsoft.AspNetCore.Mvc;
using StorageApi.Core.ModelsDTO;
using StorageApi.Services;

namespace StorageApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {

            var authResponse = await _authService.Login(loginDto.Email, loginDto.Password);

            if (authResponse.Success != true)
            {
                return Unauthorized(new { error = authResponse.Error });
            }

            return Ok(new { token = authResponse.Token });
            
        }

    }
}