using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Models;

namespace UserService.Services;

public interface IUserService
{
        Task<UserManagerResponce> RegisterUserAsync(RegistrationModel model);

        Task<UserManagerResponce> LoginUserAsync(LoginModel model);
}
    
