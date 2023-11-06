using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using UserService.Models;
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
        
        [HttpGet("GetEmployees")]
        public async Task<ActionResult<IEnumerable<AutoUserViewModel>>> GetEmployees()
        {
            var users = await _autoUserService.GetEmployees();
            if (users == null)
                return NotFound();
            return Ok(users);
        }

        [HttpPost("ChangeUserRole")]
        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRoleModel model)
        {
            var result = await _autoUserService.ChangeUserRole(model);
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
        public async Task<IActionResult> CreateUser([FromBody] RegistrationModel model) 
        {
            if (ModelState.IsValid)
            {
                var result = await _autoUserService.CreateUser(model);
                if (result.IsSuccess)
                    return Ok();
                return BadRequest();
            }

            return BadRequest("Some properties are not valid!");

        }
    }
}
