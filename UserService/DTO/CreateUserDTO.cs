using System.ComponentModel.DataAnnotations;

namespace UserService.DTO;

public class CreateUserDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }

    public string? UserRole { get ; set; }
}