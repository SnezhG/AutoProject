﻿using Microsoft.AspNetCore.Identity;
using UserService.DTO;
using UserService.Services;
namespace UserService.ServiceInterfaces;

public interface IAutoUser
{
    Task<IEnumerable<AutoUserDTO>> GetEmployees();
    Task<UserManagerResponce> ChangeUserRole(ChangeUserRoleDTO dto);
    Task<UserManagerResponce> DeleteUser(int id);
    Task<UserManagerResponce> CreateUser(RegistrationDTO dto);
}