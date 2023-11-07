using Microsoft.AspNetCore.Mvc;
using UserService.DTO;
using UserService.ServiceInterfaces;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoUserController : ControllerBase
    {
        
        private readonly IAutoUser _autoUserService;

        public AutoUserController(IAutoUser autoUserService)
        {
            _autoUserService = autoUserService;
        }
        
        [HttpGet("GetEmployees")]
        public async Task<ActionResult<IEnumerable<AutoUserDTO>>> GetEmployees()
        {
            var users = await _autoUserService.GetEmployees();
            if (users == null)
                return NotFound();
            return Ok(users);
        }

        [HttpPost("ChangeUserRole")]
        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRoleDTO dto)
        {
            var result = await _autoUserService.ChangeUserRole(dto);
            if (result.IsSuccess)
                return Ok();
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id) 
        {
            var result = await _autoUserService.DeleteUser(id);
            if (result.IsSuccess)
                return Ok();
            return BadRequest();

        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] RegistrationDTO dto) 
        {
            if (ModelState.IsValid)
            {
                var result = await _autoUserService.CreateUser(dto);
                if (result.IsSuccess)
                    return Ok();
                return BadRequest();
            }

            return BadRequest("Some properties are not valid!");

        }
    }
}
