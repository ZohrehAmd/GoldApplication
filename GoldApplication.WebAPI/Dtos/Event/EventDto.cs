using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Dtos
{
    public class EventDto
    {
        public long ProductEventId { get; set; }
        [Display(Name ="محصول")]
        public long ProductId { get; set; }
        [Display(Name = "وزن معامله ( گرم) ")]
        public long Geram { get; set; }
        [Display(Name = "قیمت معامله ( تومان) ")]
        public long GeramPrice { get; set; }
        [Display(Name = "عنوان معامله ")]
        public string Title { get; set; }
        [Display(Name = "وضعیت معامله ")]
        public string Status { get; set; }
        [Display(Name = "تاریخ ایجاد ")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "تاریخ معامله ")]
        public DateTime DateEvent { get; set; }
        [Display(Name = "ساعت شروع ")]
        public string StartTime { get; set; }
        [Display(Name = "ساعت پایان ")]
        public string EndTime { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public  ProductDto Product { get; set; }
    }
}
