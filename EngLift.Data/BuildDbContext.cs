﻿using EngLift.Model.Entities;
using EngLift.Model.Entities.Identity;
using EngLift.Model.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EngLift.Data
{
    public class BuildDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;
        public BuildDbContext(DbContextOptions<BuildDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #region Dbset
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<LessonWord> LessonWords { get; set; }

        #endregion

        #region override method
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var entityTypes = builder.Model.GetEntityTypes();
            foreach (var entityType in entityTypes)
                builder.Entity(entityType.ClrType)
                       .ToTable(entityType.GetTableName().Replace("AspNet", ""));

            builder.Entity<LessonWord>().ToTable("LessonWords").HasKey(k => new { k.LessonId, k.WordId });
        }

        public override int SaveChanges()
        {
            BeforeSaving();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            BeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void BeforeSaving()
        {
            var entities = ChangeTracker.Entries();
            foreach (var entity in entities)
            {
                if (entity.Entity is IAudit && _httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User != null)
                {
                    IAudit audit = (IAudit)entity.Entity;
                    string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var now = DateTime.UtcNow;
                    switch (entity.State)
                    {
                        case EntityState.Added:
                            audit.CreatedAt = now;
                            audit.CreatedBy = userId;
                            audit.UpdatedAt = now;
                            audit.UpdatedBy = userId;
                            break;
                        case EntityState.Modified:
                            audit.UpdatedAt = now;
                            audit.UpdatedBy = userId;
                            break;
                    }
                }
            }
        }
        #endregion
    }

}
