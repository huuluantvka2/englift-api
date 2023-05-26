using EngLift.Common;
using EngLift.Data.Infrastructure.Interfaces;
using EngLift.DTO.User;
using EngLift.Model.Entities.Identity;
using EngLift.Service.Extensions;
using EngLift.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
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

            var result = _jwtService.CreateToken(user);
            _logger.LogInformation($"AuthService -> LoginAdmin with data successfully");
            return result;
        }
    }
}
