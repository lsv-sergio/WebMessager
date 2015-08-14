using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebMessenger.Models
{
    public class LoginModel
    {
        [Display(Name="Введите логин")]
        [Required]
        [StringLength(25,MinimumLength=3, ErrorMessage = "Логин должен быть больше 3 символов")]
        public string LoginName { get; set; }

        [Display(Name = "Введите пароль")]
        [Required]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }
    }
}