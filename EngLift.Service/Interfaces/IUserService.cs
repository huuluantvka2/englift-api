using EngLift.DTO.Base;
using EngLift.DTO.Response;
using EngLift.DTO.User;

namespace EngLift.Service.Interfaces
{
    public interface IUserService
    {
        Task<DataList<UserItemDTO>> GetAllUser(BaseRequest request);
    }
}
