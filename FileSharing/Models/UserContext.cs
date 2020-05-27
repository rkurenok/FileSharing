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
        public DbSet<FileUniqueKey> FileUniqueKeys { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Constant> Constants { get; set; }
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

            db.FileRetentionPeriods.Add(new FileRetentionPeriod { Id = 1, Value = 3 }); // 3 дня
            db.FileRetentionPeriods.Add(new FileRetentionPeriod { Id = 2, Value = 7 }); // 7 дней 
            db.FileRetentionPeriods.Add(new FileRetentionPeriod { Id = 3, Value = 30 }); // 30 дней

            db.Constants.Add(new Constant { Id = 1, Name = "Salt", Value = "test" });

            db.Categories.Add(new Category { Id = 1, Name = "Изображения", FilesExtension = ".jpg .psd .bmp .jpeg .jp2 .j2k .jpf .jpm .jpg2 .j2c .jpc .jxr .hdp .wdp .gif .eps .png .pict .pcx .ico .crd .ai .raw .svg .webp" });
            db.Categories.Add(new Category { Id = 1, Name = "Аудио", FilesExtension = ".aif .cda .mid .midi .mp3 .mpa .ogg .wav .wma .wpl" });
            db.Categories.Add(new Category { Id = 1, Name = "Видео", FilesExtension = ".3g2 .3gp .avi .flv .h264 .m4v .mkv .mov .mp4 .mpg .mpeg .rm .swf .vob .wmv" });
            db.Categories.Add(new Category { Id = 1, Name = "Текстовые", FilesExtension = ".doc .docx .odt .pdf .rtf .tex .txt .wpd" });
            db.Categories.Add(new Category { Id = 1, Name = "Таблицы", FilesExtension = ".ods .xls .xlsm .xlsx" });
            db.Categories.Add(new Category { Id = 1, Name = "Архивы", FilesExtension = ".7z .arj .deb .pkg .rar .rpm .tar .gz .z .zip" });
            db.Categories.Add(new Category { Id = 1, Name = "Презентации", FilesExtension = ".key .odp .pps .ppt .pptx" });
            db.Categories.Add(new Category { Id = 1, Name = "Others", FilesExtension = "" });

            base.Seed(db);
        }
    }
}