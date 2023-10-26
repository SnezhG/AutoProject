using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Models;

namespace UserService.Services
{
    public interface IUserService
    {
        Task<UserManagerResponce> RegisterUserAsync(RegistrationModel model);

        Task<UserManagerResponce> LoginUserAsync(LoginModel model);
    }

    public class AutoUserService : IUserService
    {
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;

        public AutoUserService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;

        }

        public async Task<UserManagerResponce> RegisterUserAsync(RegistrationModel model)
        {
            if (model == null)
                throw new NullReferenceException("Registration model is null!");
            if (model.Password != model.ConfirmPassword)
                return new UserManagerResponce
                {
                    Message = "Passwords doesnt match!",
                    IsSuccess = false
                };
            var identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(identityUser, model.Password);

            if (result.Succeeded)
            {
                return new UserManagerResponce
                {
                    Message = "User created!",
                    IsSuccess = true
                };
            }

            return new UserManagerResponce
            {
                Message = "User is not created!",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<UserManagerResponce> LoginUserAsync(LoginModel model) 
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null) 
            {
                return new UserManagerResponce
                {
                    Message = "User not found!",
                    IsSuccess = false
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result) 
            {
                return new UserManagerResponce
                {
                    Message = "Invalid password!",
                    IsSuccess = false
                };
            }

            var claims = new[] 
            {
                new Claim("Email", model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponce
            {
                Message = tokenString,
                IsSuccess = true,
                ExpireDate = token.ValidTo
            };
        }
    }
}