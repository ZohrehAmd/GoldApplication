using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Dtos.User
{
    public class LoginUserDto
    {
        [Display(Name = "شماره همراه")]
        [Required(ErrorMessage = " {0} نمیتواند خالی باشد")]
        [MaxLength(11, ErrorMessage = " {0} نمیتواند بیشتر از 11 حرف باشد")]
        [MinLength(11, ErrorMessage = " {0} نمیتواند کمتر از 11 حرف باشد")]
        public string Mobile { get; set; }
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = " {0} نمیتواند خالی باشد")]
        public string Password { get; set; }
    }
}
