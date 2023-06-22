using EngLift.Common;
using EngLift.Data.Infrastructure.Interfaces;
using EngLift.DTO.Response;
using EngLift.DTO.User;
using EngLift.Model.Entities.Identity;
using EngLift.Service.Extensions;
using EngLift.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace EngLift.Service.Implements
{
    public class AuthService : ServiceBase<AuthService>, IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtService _jwtService;
        public AuthService(ILogger<AuthService> logger, UserManager<User> userManager, IUnitOfWork unitOfWork, JwtService jwtService) : base(logger, unitOfWork)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }


        public async Task<LoginSuccessDTO> LoginAdmin(UserLoginDTO dto)
        {
            _logger.LogInformation($"AuthService -> LoginAdmin with data {JsonConvert.SerializeObject(dto)}");
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || user.Active == false || user.IsAdmin == false)
            {
                _logger.LogWarning($"AuthService -> LoginAdmin ->Not found");
                throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.NOT_FOUND);
            }

            var isPassWordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPassWordValid)
            {
                _logger.LogWarning($"AuthService -> LoginAdmin ->${ErrorMessage.INCORRECT_PASSWORD}");
                throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.INCORRECT_PASSWORD);
            }
            List<string> Roles = new List<string>();
            var roleNames = await UnitOfWork.UserRolesRepo.GetAll().Where(x => x.UserId == user.Id).Include(x => x.Role).Select(x => x.Role.Name).ToListAsync();
            var result = _jwtService.CreateToken(user, roleNames);
            _logger.LogInformation($"AuthService -> LoginAdmin with data successfully");
            return result;
        }

        public async Task<SingleId> CreateUser(UserSignUpDTO dto)
        {
            _logger.LogInformation($"AuthService -> CreateUser with data {JsonConvert.SerializeObject(dto)}");

            var Email = dto.Email.ToLower();
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Email = Email,
                FullName = dto.FullName,
                Active = true,
                EmailConfirmed = true,
                Deleted = false,
                NormalizedEmail = Email,
                UserName = Email,
                PhoneNumber = "",
                RefCode = dto.RefCode,
                TYPE_LOGIN = TYPE_LOGIN.SYSTEM,
                NormalizedUserName = Email,
                CreatedAt = DateTime.UtcNow,
            };
            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, dto.Password);

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogWarning($"AuthService -> LoginAdmin ->${ErrorMessage.INCORRECT_PASSWORD}");
                throw new ServiceExeption(HttpStatusCode.BadRequest, result.Errors.FirstOrDefault().Description);
            }

            _logger.LogInformation($"AuthService -> CreateUser with data successfully");
            return new SingleId() { Id = user.Id };
        }

        public async Task<LoginSuccessDTO> LoginUser(UserLoginDTO dto)
        {
            _logger.LogInformation($"AuthService -> LoginUser with data {JsonConvert.SerializeObject(dto)}");
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || user.Active == false || user.Deleted == true)
            {
                _logger.LogWarning($"AuthService -> LoginUser ->Not found");
                throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.INVALID_EMAIL);
            }

            var isPassWordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPassWordValid)
            {
                _logger.LogWarning($"AuthService -> LoginUser ->${ErrorMessage.INCORRECT_PASSWORD}");
                throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.INCORRECT_PASSWORD);
            }

            var result = _jwtService.CreateToken(user, null);
            _logger.LogInformation($"AuthService -> LoginUser with data successfully");
            return result;
        }
    }
}
