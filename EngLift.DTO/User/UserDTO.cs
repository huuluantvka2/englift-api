﻿using EngLift.DTO.Base;
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
        public string? Avatar { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
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
        public string? Avatar { get; set; }
        public bool Active { get; set; }
        public bool Deteted { get; set; }
        public string Email { get; set; }
        public string? OAuthId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? RefCode { get; set; }
        public TYPE_LOGIN TypeLogin { get; set; }
        public int TotalDateStudied { get; set; } = 0;
        public DateTime? LastTimeStudy { get; set; }
        public int TotalWords { get; set; } = 0;
        public int DateTimeOffset { get; set; } = -420;
        public string? Address { get; set; }
        public int? TimeRemind { get; set; } = 20;
        public bool? IsNotify { get; set; }
        public string? Introduce { get; set; }
        public bool? Gender { set; get; }
    }

    public class UserAdminUpdateDTO
    {
        public string FullName { get; set; }
        public bool Active { get; set; }
        public string? RefCode { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class UserRequest : BaseRequest
    {
        public bool? Active { get; set; }
        public TYPE_LOGIN? TypeLogin { get; set; }
    }

    public class UserLoginSocialDTO
    {
        public string AccessToken { get; set; }
        public string Uid { get; set; }
        public TYPE_LOGIN TypeLogin { get; set; }
    }

    public class UserUpdateDto
    {
        public string FullName { get; set; }
        public string? Address { get; set; }
        public int TimeRemind { get; set; } = 20;
        public string? PhoneNumber { get; set; }
        public bool? IsNotify { get; set; }
        public string? RefCode { get; set; }
        public string? Introduce { get; set; }
        public bool? Gender { set; get; }
    }


    public class ReportWordDto
    {
        public int OffsetTime { get; set; }
        public int Days { get; set; }
    }
    public class ReportWordData
    {
        public List<int> Datas { get; set; }
        public List<string> Categories { get; set; }
        public int? TotalLessons { get; set; }
        public int? TotalWords { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastTimeStudy { get; set; }
    }
    public class TestDto
    {
    }
}
