using EngLift.Common;
using EngLift.Data.Infrastructure.Interfaces;
using EngLift.DTO.Response;
using EngLift.DTO.User;
using EngLift.Model.Entities.Identity;
using EngLift.Service.Extensions;
using EngLift.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace EngLift.Service.Implements
{
    public class UserService : ServiceBase<UserService>, IUserService
    {
        public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger) : base(logger, unitOfWork)
        {

        }

        public async Task<DataList<UserItemDTO>> GetAllUser(UserRequest request)
        {
            _logger.LogInformation($"UserService -> GetAllUser with query {JsonConvert.SerializeObject(request)}");
            var result = new DataList<UserItemDTO>();
            IQueryable<User> query = UnitOfWork.UsersRepo.GetAll()
                .Where(x =>
                    x.IsAdmin == false &&
                    x.Deleted == false &&
                    (request.Active != null ? x.Active : true) &&
                    (request.TypeLogin != null ? x.TYPE_LOGIN == request.TypeLogin : true) &&
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
                    case 3: query = query.OrderBy(x => x.Email); break;
                    case 4: query = query.OrderByDescending(x => x.Email); break;
                    case 5: query = query.OrderBy(x => x.FullName); break;
                    case 6: query = query.OrderByDescending(x => x.FullName); break;
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

        public async Task<UserItemDTO> GetUserById(Guid id)
        {
            _logger.LogInformation($"UserService -> GetUserById with Id {id}");
            var user = await UnitOfWork.UsersRepo.GetAll().Where(x => x.Id == id).Select(x => new UserItemDTO()
            {
                Id = x.Id,
                Active = x.Active,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                Deteted = x.Deleted,
                Email = x.Email,
                FullName = x.FullName,
                Avatar = x.Avatar,
                OAuthId = x.OAuthId,
                PhoneNumber = x.PhoneNumber,
                RefCode = x.RefCode,
                TypeLogin = x.TYPE_LOGIN,
                UpdatedAt = x.UpdatedAt,
                UpdatedBy = x.UpdatedBy,

            }).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.NOT_FOUND);
            }
            return user;
        }

        public async Task<SingleId> AdminUpdateUser(Guid Id, UserAdminUpdateDTO dto)
        {
            _logger.LogInformation($"UserService -> AdminUpdateUser with dto {JsonConvert.SerializeObject(dto)}");
            var user = UnitOfWork.UsersRepo.GetById(Id);
            if (user == null || user.IsAdmin == true)
            {
                throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.NOT_FOUND);
            }
            else if (user.Deleted == true)
            {
                throw new ServiceExeption(HttpStatusCode.BadRequest, ErrorMessage.USER_IS_DELETED);
            }
            user.RefCode = dto.RefCode;
            user.Active = dto.Active;
            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;

            UnitOfWork.UsersRepo.Update(user);
            await UnitOfWork.SaveChangesAsync();

            _logger.LogInformation($"UserService -> AdminUpdateUser successfully");
            return new SingleId() { Id = Id };
        }

        public async Task<SingleId> AdminDeleteUser(Guid Id)
        {
            _logger.LogInformation($"UserService -> AdminDeleteUser with id {JsonConvert.SerializeObject(Id)}");
            var user = UnitOfWork.UsersRepo.GetById(Id);
            if (user == null || user.IsAdmin == true)
            {
                throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.NOT_FOUND);
            }
            else if (user.Deleted == true)
            {
                throw new ServiceExeption(HttpStatusCode.BadRequest, ErrorMessage.USER_IS_DELETED);
            }
            user.Deleted = true;
            user.Active = false;

            UnitOfWork.UsersRepo.Update(user);
            await UnitOfWork.SaveChangesAsync();

            _logger.LogInformation($"UserService -> AdminDeleteUser successfully");
            return new SingleId() { Id = Id };
        }
    }
}
