using EngLift.Common;
using EngLift.Data.Infrastructure.Interfaces;
using EngLift.DTO.Response;
using EngLift.DTO.User;
using EngLift.Model.Entities.Identity;
using EngLift.Service.Extensions;
using EngLift.Service.Interfaces;
using FirebaseAdmin.Auth;
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
            bool exist = UnitOfWork.UsersRepo.GetAll().Any(x => x.Email == Email && x.TYPE_LOGIN == TYPE_LOGIN.SYSTEM);
            if (exist)
            {
                _logger.LogWarning($"AuthService -> CreateUser ->${ErrorMessage.ALREADY_EMAIL}");
                throw new ServiceExeption(HttpStatusCode.Conflict, ErrorMessage.ALREADY_EMAIL);
            }
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

            UnitOfWork.UsersRepo.Insert(user);

            await UnitOfWork.SaveChangesAsync();
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
            result.Avatar = user.Avatar;
            result.FullName = user.FullName;
            result.Email = user.Email;
            _logger.LogInformation($"AuthService -> LoginUser with data successfully");
            return result;
        }

        public async Task<LoginSuccessDTO> LoginSocial(UserLoginSocialDTO request)
        {
            try
            {
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(request.AccessToken);
                if (request.TypeLogin == TYPE_LOGIN.GOOGLE)
                {
                    string Uid = decodedToken.Claims.GetValueOrDefault("user_id").ToString();
                    if (Uid != request.Uid)
                    {
                        _logger.LogError($"AuthService -> LoginSocial ->Not found");
                        throw new ServiceExeption(HttpStatusCode.BadRequest, ErrorMessage.BAD_TOKEN_OR_EXPIRE);
                    }
                    var userDb = await UnitOfWork.UsersRepo.GetAll().FirstOrDefaultAsync(x => x.UserName == Uid);
                    if (userDb == null)
                    {
                        string Email = decodedToken.Claims.GetValueOrDefault("email").ToString();
                        string Picture = decodedToken.Claims.GetValueOrDefault("picture").ToString();
                        string Name = decodedToken.Claims.GetValueOrDefault("name").ToString();
                        User user = new User()
                        {
                            Id = Guid.NewGuid(),
                            Email = Email,
                            FullName = Name,
                            Avatar = Picture,
                            Active = true,
                            EmailConfirmed = true,
                            Deleted = false,
                            NormalizedEmail = Email,
                            UserName = Uid,
                            PhoneNumber = "",
                            RefCode = "",
                            OAuthId = Uid,
                            TYPE_LOGIN = TYPE_LOGIN.GOOGLE,
                            NormalizedUserName = Uid,
                            CreatedAt = DateTime.UtcNow,
                        };
                        var passwordHasher = new PasswordHasher<User>();
                        user.PasswordHash = passwordHasher.HashPassword(user, CommonFunc.GenerateRandomPassword(12));
                        await _userManager.CreateAsync(user);

                        var result = _jwtService.CreateToken(user, null);
                        _logger.LogInformation($"AuthService -> Signin with data successfully");
                        return result;
                    }
                    else
                    {
                        var result = _jwtService.CreateToken(userDb, null);
                        _logger.LogInformation($"AuthService -> LoginUser with data successfully");
                        return result;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"AuthService -> LoginUser ->${ErrorMessage.BAD_TOKEN_OR_EXPIRE}");
                throw new ServiceExeption(HttpStatusCode.BadRequest, ErrorMessage.BAD_TOKEN_OR_EXPIRE);
            }
        }
    }
}
