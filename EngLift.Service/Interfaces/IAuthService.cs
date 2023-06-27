using EngLift.DTO.Response;
using EngLift.DTO.User;

namespace EngLift.Service.Interfaces
{
    public interface IAuthService
    {
        Task<LoginSuccessDTO> LoginAdmin(UserLoginDTO dto);
        Task<SingleId> CreateUser(UserSignUpDTO dto);
        Task<LoginSuccessDTO> LoginUser(UserLoginDTO dto);
        Task<LoginSuccessDTO> LoginSocial(UserLoginSocialDTO request);
    }
}
