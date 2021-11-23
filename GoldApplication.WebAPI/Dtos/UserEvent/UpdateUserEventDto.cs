using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Dtos.UserEvent
{
    public class UpdateUserEventDto
    {
        [Required(ErrorMessage = "آی دی خرید وارد کنید")]
        public long UserEventId { get; set; }
        [Required(ErrorMessage = "آی دی کاربر وارد کنید")]
        public long UserId { get; set; }
        [Required(ErrorMessage = "آی معامله وارد کنید")]
        public long ProductEventId { get; set; }
        [Required(ErrorMessage = "مقدار خرید به گرم را وارد کنید")]
        public long Geram { get; set; }
    }
}
