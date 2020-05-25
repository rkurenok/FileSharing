using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FileSharing.Models
{
    public class UserContext : DbContext
    {
        public UserContext() : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<File> Files { get; set; }

        public DbSet<FileAccess> FileAccesses { get; set; }
        public DbSet<FileRetentionPeriod> FileRetentionPeriods { get; set; }
    }

    public class UserDbInitializer : DropCreateDatabaseAlways<UserContext>
    {
        protected override void Seed(UserContext db)
        {
            db.Roles.Add(new Role { Id = 1, Name = "admin" });
            db.Roles.Add(new Role { Id = 2, Name = "user" });

            db.Users.Add(new User
            {
                Id = 1,
                Email = "somemail@gmail.com",
                Login = "somemail@gmail.com",
                Password = "123456",
                Age = 21,
                Gender = "Мужской",
                RoleId = 1
            });

            db.FileAccesses.Add(new FileAccess { Id = 1, Name = "private" });
            db.FileAccesses.Add(new FileAccess { Id = 2, Name = "public" });

            db.FileRetentionPeriods.Add(new FileRetentionPeriod { Id = 1, Value = 10 });
            db.FileRetentionPeriods.Add(new FileRetentionPeriod { Id = 2, Value = 30 });
            db.FileRetentionPeriods.Add(new FileRetentionPeriod { Id = 3, Value = 60 });

            base.Seed(db);
        }
    }
}