using UserService.DTO;

namespace UserService.Services;

public interface IUserService
{
        Task<UserManagerResponce> RegisterUserAsync(RegistrationDTO dto);

        Task<UserManagerResponce> LoginUserAsync(LoginDTO dto);
}
    
