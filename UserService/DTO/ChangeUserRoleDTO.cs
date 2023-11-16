using Microsoft.AspNetCore.Identity;

namespace UserService.DTO;

public class ChangeUserRoleDTO
{
    public string UserId { get; set; }
    public string UserEmail { get; set; }
    public List<IdentityRole> AllRoles { get; set; }
    public string UserRole { get; set; }
}