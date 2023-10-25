using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using UserService.Models;

namespace UserService.Services
{
    public interface IUserService
    {
        Task<UserManagerResponce> RegisterUserAsync(RegistrationModel model);
    }

    public class AutoUserService : IUserService
    {
        private UserManager<IdentityUser> _userManager;

        public AutoUserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
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
    }
}