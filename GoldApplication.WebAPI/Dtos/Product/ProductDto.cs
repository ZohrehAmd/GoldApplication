using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Dtos
{
    public class ProductDto
    {
        public long ProductId { get; set; }
        [Display(Name = "عنوان محصول")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد .")]
        [MaxLength(200, ErrorMessage = " {0} نمیتواند بیشتر از 200 حرف باشد")]
        public string Title { get; set; }
        [Display(Name = "نام تصویر محصول")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد .")]
        [MaxLength(60, ErrorMessage = " {0} نمیتواند بیشتر از 60 حرف باشد")]
        public string ImageName { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
    }
}
