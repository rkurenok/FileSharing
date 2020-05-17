using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FileSharing.Models
{
    public class User
    {
        //[ScaffoldColumn(false)]
        public int Id { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Возраст")]
        public int Age { get; set; }

        [Display(Name = "Пол")]
        public int MaleId { get; set; }
        public Male Male { get; set; }

        [Display(Name = "Роль")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Male
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}