using EngLift.DTO.Response;
using EngLift.DTO.User;

namespace EngLift.Service.Interfaces
{
    public interface IUserService
    {
        Task<UserItemDTO> GetUserById(Guid id);
        Task<DataList<UserItemDTO>> GetAllUser(UserRequest request);
        Task<SingleId> AdminDeleteUser(Guid Id);
        Task<SingleId> AdminUpdateUser(Guid Id, UserAdminUpdateDTO dto);
    }
}
