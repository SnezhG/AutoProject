using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using UserService.Models;
using UserService.DTO;

namespace UserService.Services;

    public partial class AutoUserAuthService : IUserService
    {
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;

        public AutoUserAuthService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;

        }

        
        public async Task<UserManagerResponce> RegisterUserAsync(RegistrationDTO dto)
        {
            if (dto == null)
                throw new NullReferenceException("Registration model is null!");
            if (dto.Password != dto.ConfirmPassword)
                return new UserManagerResponce
                {
                    Message = "Passwords doesnt match!",
                    IsSuccess = false
                };
            var identityUser = new IdentityUser
            {
                Email = dto.Email,
                UserName = dto.Email,
            };

            var result = await _userManager.CreateAsync(identityUser, dto.Password);

            if (result.Succeeded)
            {
                if (dto.UserRole == null)
                {
                    await _userManager.AddToRoleAsync(identityUser, "client");
                }
                else
                {
                    await _userManager.AddToRoleAsync(identityUser, dto.UserRole);
                }

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

        public async Task<UserManagerResponce> LoginUserAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return new UserManagerResponce
                {
                    Message = "User not found!",
                    IsSuccess = false
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!result)
            {
                return new UserManagerResponce
                {
                    Message = "Invalid password!",
                    IsSuccess = false
                };
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim("Email", user.Email),
                new Claim("ClientId", user.Id)
            };
            foreach (var userRole in userRoles) 
            {
                authClaims.Add(new Claim("Role", userRole));
            }
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTAuth:SecretKey"]));
            var token = new JwtSecurityToken
                (
                    expires: DateTime.Now.AddMinutes(5),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponce
            {
                Message = tokenString,
                IsSuccess = true,
                Token = tokenString,
                Role = userRoles[0]
            };
        }
    }