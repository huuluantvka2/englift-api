using EngLift.Data.Infrastructure.Interfaces;
using EngLift.DTO.Base;
using EngLift.DTO.Response;
using EngLift.DTO.User;
using EngLift.Model.Entities.Identity;
using EngLift.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EngLift.Service.Implements
{
    public class UserService : ServiceBase<UserService>, IUserService
    {
        public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger) : base(logger, unitOfWork)
        {

        }

        public async Task<DataList<UserItemDTO>> GetAllUser(BaseRequest request)
        {
            _logger.LogInformation($"UserService -> GetAllUser with query {JsonConvert.SerializeObject(request)}");
            var result = new DataList<UserItemDTO>();
            IQueryable<User> query = UnitOfWork.UsersRepo.GetAll().Where(x =>
                                                                            x.Deleted == false &&
                                                                            x.IsAdmin == false &&
                                                                            String.IsNullOrEmpty(request.Search) ? true :
                                                                                (
                                                                                x.FullName.ToLower().Contains(request.Search) ||
                                                                                x.Email.ToLower().Contains(request.Search)
                                                                                )
                                                                         );
            result.TotalRecord = query.Count();
            if (request.Sort != null)
            {
                switch (request.Sort)
                {
                    case 1: query = query.OrderBy(x => x.CreatedAt); break;
                    case 2: query = query.OrderByDescending(x => x.CreatedAt); break;
                }
            }
            else query = query.OrderByDescending(x => x.CreatedAt);
            var data = await query.Select(x => new UserItemDTO()
            {
                Id = x.Id,
                Active = x.Active,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                Deteted = x.Deleted,
                Email = x.Email,
                FullName = x.FullName,
                OAuthId = x.OAuthId,
                PhoneNumber = x.PhoneNumber,
                RefCode = x.RefCode,
                TypeLogin = x.TYPE_LOGIN,
                UpdatedAt = x.UpdatedAt,
                UpdatedBy = x.UpdatedBy,
            }).Skip((request.Page - 1) * request.Limit).Take(request.Limit).ToListAsync();

            result.Items = data;

            _logger.LogInformation($"UserService -> GetAllUser successfully ");
            return result;
        }
    }
}
