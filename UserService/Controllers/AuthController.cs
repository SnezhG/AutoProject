using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.DTO;
using UserService.ServiceInterfaces;
using UserService.Services;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Registration")]

        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationDTO dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterUserAsync(dto);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid!");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO dto) 
        {
            if (ModelState.IsValid) 
            {
                var result = await _authService.LoginUserAsync(dto);

                if (result.IsSuccess)
                    return Ok(result);
                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid!");
        }

    }
}