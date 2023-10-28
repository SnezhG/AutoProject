using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using UserService.Models;
using UserService.Services;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoUserController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;

        public AutoUserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


/*        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole()
        {
            var roleName = new IdentityRole { Name = "dispatcher" };
            var result = await _roleManager.CreateAsync(roleName);

            if (result.Succeeded)
            {
                return Ok("Role created successfully");
            }

            return BadRequest("Something went wrong");
        }*/

        [HttpGet("GetEmployees")]
        public async Task<IActionResult> GetEmloyees() 
        {
            var admins = await _userManager.GetUsersInRoleAsync("admin");
            var dispatchers = await _userManager.GetUsersInRoleAsync("dispatcher");
            var users = admins.Concat(dispatchers);

            return Ok(users);

        }

        [HttpPut("ChangeUserRole")]
        public async Task<IActionResult> ChangeUserRole(int id) 
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                return NotFound("User not found");

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id) 
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                return NotFound("User not found");

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return Ok("User deleted successfully");

            return BadRequest("Failed to delete user");
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] RegistrationModel model) 
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model);

                if (result.IsSuccess) 
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid!");

        }
    }
}
