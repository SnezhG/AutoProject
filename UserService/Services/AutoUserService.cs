﻿using Microsoft.AspNetCore.Identity;
using UserService.DTO;
using UserService.ServiceInterfaces;

namespace UserService.Services;

public class AutoUserService : IAutoUser
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUserService _userService;

    public AutoUserService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IUserService userService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userService = userService;
    }

    public async Task<IEnumerable<AutoUserDTO>> GetEmployees()
    {
        var admins = await _userManager.GetUsersInRoleAsync("admin");
        var dispatchers = await _userManager.GetUsersInRoleAsync("dispatcher");
        var temp = admins.Concat(dispatchers);

        var users = new List<AutoUserDTO>();

        foreach (var user in temp)
        {
            var role = await _userManager.GetRolesAsync(user);
            var userViewModel = new AutoUserDTO
            {
                Email = user.Email,
                Role = role[0]
            };
        }

        return users;
    }

    public async Task<UserManagerResponce> ChangeUserRole(ChangeUserRoleDTO dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId);

        if (user == null)
            return new UserManagerResponce
            {
                IsSuccess = false
            };

        var currRole = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currRole);
        await _userManager.AddToRoleAsync(user, dto.NewRole);
        return new UserManagerResponce
        {
            IsSuccess = true
        };
    }

    public async Task<UserManagerResponce> DeleteUser(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
            return new UserManagerResponce
            {
                IsSuccess = false
            };

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
            return new UserManagerResponce
            {
                IsSuccess = true
            };

        return new UserManagerResponce
        {
            IsSuccess = false
        };
    }

    public async Task<UserManagerResponce> CreateUser(RegistrationDTO dto)
    {
        var result = await _userService.RegisterUserAsync(dto);

        return result;
    }
}