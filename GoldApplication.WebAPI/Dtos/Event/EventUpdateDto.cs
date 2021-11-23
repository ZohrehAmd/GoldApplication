using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Dtos
{
    public class EventUpdateDto
    {
        public long ProductEventId { get; set; }
        [Display(Name = "محصول")]
        [Required(ErrorMessage = "محصول را انتخاب کنید .")]
        public long ProductId { get; set; }
        [Display(Name = "وزن معامله ( گرم) ")]
        [Required(ErrorMessage = "{0} را وارد کنید .")]
        public long Geram { get; set; }
        [Display(Name = "قیمت معامله ( تومان) ")]
        [Required(ErrorMessage = "{0} را وارد کنید .")]
        public long GeramPrice { get; set; }
        [Display(Name = "عنوان معامله ")]
        [Required(ErrorMessage = "{0} را وارد کنید .")]
        [MaxLength(250)]
        public string Title { get; set; }
        [Display(Name = "وضعیت معامله ")]
        [Required(ErrorMessage = "{0} را وارد کنید .")]
        [MaxLength(150)]
        public string Status { get; set; }
        [Required(ErrorMessage = "تاریخ ایجاد را وارد کنید .")]
        public DateTime CreateDate { get; set; }
        [Required(ErrorMessage = "تاریخ معامله را وارد کنید .")]
        public DateTime DateEvent { get; set; }
        [Required(ErrorMessage = "ساعت شروع معامله را وارد کنید .")]
        [MaxLength(8, ErrorMessage = " {0} نمیتواند بیشتر از 8 حرف باشد")]
        [MinLength(8, ErrorMessage = " {0} نمیتواند کمتر از 8 حرف باشد")]
        public string StartTime { get; set; }
        [Required(ErrorMessage = "ساعت پایان معامله را وارد کنید .")]
        [MaxLength(8, ErrorMessage = " {0} نمیتواند بیشتر از 8 حرف باشد")]
        [MinLength(8, ErrorMessage = " {0} نمیتواند کمتر از 8 حرف باشد")]
        public string EndTime { get; set; }
        [Required]
        public bool IsActive { get; set; }

    }
}
