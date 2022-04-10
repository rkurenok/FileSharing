﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FileSharing.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать символов не менее: {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение нового пароля")]
        [Compare("NewPassword", ErrorMessage = "Новый пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }
    }

    public class EditAccount
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Возраст")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Пол")]
        public string Male { get; set; }
    }
}