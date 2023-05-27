using EngLift.Model.Abstracts;
using EngLift.Model.Entities.Identity;

namespace EngLift.DTO.User
{
    public class UserAdminSeedDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class UserLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginSuccessDTO
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public Guid UserId { get; set; }
    }

    public class UserSignUpDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string? RefCode { get; set; }
    }
    public class UserItemDTO : AuditBase
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public bool Active { get; set; }
        public bool Deteted { get; set; }
        public string Email { get; set; }
        public string? OAuthId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? RefCode { get; set; }
        public TYPE_LOGIN TypeLogin { get; set; }
    }

}
