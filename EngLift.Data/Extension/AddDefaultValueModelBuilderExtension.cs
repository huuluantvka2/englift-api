using EngLift.Model.Entities;
using EngLift.Model.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace EngLift.Data.Extension
{
    public static class AddDefaultValueModelBuilderExtension
    {
        public static ModelBuilder AddDefaultValueModelBuilder(this ModelBuilder builder)
        {
            builder.Entity<User>().Property(p => p.Active).HasDefaultValue(true);
            builder.Entity<User>().Property(p => p.Deleted).HasDefaultValue(false);

            builder.Entity<Lesson>().Property(p => p.Active).HasDefaultValue(true);
            builder.Entity<Word>().Property(p => p.Active).HasDefaultValue(true);


            return builder;
        }
    }
}
