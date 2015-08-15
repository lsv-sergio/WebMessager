using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebMessenger.Models
{
    public class LoginModel
    {
        [Display(Name="Логин")]
        [Required]
        [StringLength(25,MinimumLength=3, ErrorMessage = "Логин должен быть больше 3 символов")]
        public string LoginName { get; set; }

        [Display(Name = "Пароль")]
        [Required]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }
    }
}