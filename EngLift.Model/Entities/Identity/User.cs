﻿using EngLift.Model.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngLift.Model.Entities.Identity
{
    public enum TYPE_LOGIN
    {
        SYSTEM = 1,
        GOOGLE = 2,
    }

    [Table("Users")]
    public class User : IdentityUser<Guid>, IEntity<Guid>, IAudit
    {
        [Required]
        public string FullName { get; set; }
        [MaxLength(12)]
        public string? RefCode { get; set; }
        public TYPE_LOGIN TYPE_LOGIN { get; set; } = TYPE_LOGIN.SYSTEM;
        public string? OAuthId { get; set; } //google/facebook...
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsAdmin { get; set; } = false;
        public bool Active { get; set; } = true;
        public bool Deleted { get; set; } = false;
    }
}
