using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Dtos.User
{
    public class RegisterUserDto
    {
        [Display(Name = "نام")]
        [Required(ErrorMessage = " {0} نمیتواند خالی باشد")]
        [MaxLength(150, ErrorMessage = " {0} نمیتواند بیشتر از 150 حرف باشد")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = " {0} نمیتواند خالی باشد")]
        [MaxLength(150, ErrorMessage = " {0} نمیتواند بیشتر از 150 حرف باشد")]
        public string LastName { get; set; }
        [Display(Name = "شماره همراه")]
        [Required(ErrorMessage = " {0} نمیتواند خالی باشد")]
        [MaxLength(11, ErrorMessage = " {0} نمیتواند بیشتر از 11 حرف باشد")]
        [MinLength(11, ErrorMessage = " {0} نمیتواند کمتر از 11 حرف باشد")]
        public string Mobile { get; set; }
        [Display(Name = "کبمه عبور")]
        [Required(ErrorMessage = " {0} نمیتواند خالی باشد")]
        public string Password { get; set; }
        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = " {0} نمیتواند خالی باشد")]
        [Compare("Password", ErrorMessage ="کلمات عبور هم خوانی ندارند")]
        public string RePassword { get; set; }
    }
}
