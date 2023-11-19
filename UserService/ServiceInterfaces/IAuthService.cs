using UserService.DTO;
using UserService.Services;

namespace UserService.ServiceInterfaces;

public interface IAuthService
{
        Task<UserManagerResponce> RegisterUserAsync(RegistrationDTO dto);

        Task<UserManagerResponce> LoginUserAsync(LoginDTO dto);
}
    
