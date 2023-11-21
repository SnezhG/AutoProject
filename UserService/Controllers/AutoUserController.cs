using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.DTO;
using UserService.ServiceInterfaces;
using UserService.Services;

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

        [HttpGet("GetUsers")]
        public async Task<ActionResult<IEnumerable<AutoUserDTO>>> GetUsers()
        {
            var users = await _autoUserService.GetUsers();
            if (users == null)
                return NotFound();
            return Ok(users);
        }
        
        [HttpGet("GetRoles")]
        public async Task<ActionResult<List<IdentityRole>>> GetRoles()
        {
            var allRoles = await _autoUserService.GetRoles();
            if (allRoles == null)
                return NotFound();
            return Ok(allRoles);
        }
        
        [HttpGet("GetUser/{id}")]
        public async Task<ActionResult<ChangeUserRoleDTO>> GetUser(string id)
        {
            var user = await _autoUserService.GetUser(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost("ChangeUserRole")]
        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRoleDTO dto)
        {
            var result = await _autoUserService.ChangeUserRole(dto);
            if (result.IsSuccess)
                return Ok();
            return BadRequest();
        }

        [HttpDelete("{id}")]
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
