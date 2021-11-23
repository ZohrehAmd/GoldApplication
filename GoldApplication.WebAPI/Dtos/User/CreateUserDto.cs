using GoldApplication.WebAPI.Utilities.SMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Dtos.User
{
    public class CreateUserDto
    {
        public string Role { get; set; }
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
        [Display(Name = "نام تصویر")]
        [Required(ErrorMessage = " {0} نمیتواند خالی باشد")]
        [MaxLength(60, ErrorMessage = " {0} نمیتواند بیشتر از 60 حرف باشد")]
        public string Avatar { get; set; }
        [Display(Name = " توضیحات")]
        public string Description { get; set; }
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = " {0} نمیتواند خالی باشد")]
        public string Password { get; set; }
        public string RegisterCode { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime RegisterDate { get; private set; }
        public CreateUserDto()
        {
            RegisterDate = DateTime.Now;
            IsActive = false;
            RegisterCode = RandomCode.GetNewRandom();
            Avatar = "Default.jpg";
        }
    }
}
