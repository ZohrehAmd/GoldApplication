using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Dtos
{
    public class UserDto
    {
        public long UserId { get; set; }
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
        [Display(Name = "توضیح")]
        public string Description { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTime RegisterDate { get; set; }

    }
}
