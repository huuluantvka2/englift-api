using EngLift.DTO.User;

namespace EngLift.Service.Interfaces
{
    public interface IAuthService
    {
        Task<LoginSuccessDTO> LoginAdmin(UserLoginDTO dto);
    }
}
