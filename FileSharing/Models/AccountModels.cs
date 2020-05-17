using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FileSharing.Models
{
    public class LoginModel
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9._-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Пароль")]
        [MinLength(5, ErrorMessage = "Пароль должен содержать минимум 5 символов")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Возраст")]
        [Range(3, 122, ErrorMessage = "Некорректные данные")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Пол")]
        public string Male { get; set; }
    }
}