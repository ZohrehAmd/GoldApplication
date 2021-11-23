using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Dtos.UserEvent
{
    public class UpdateUserEventByAdminDto
    {
        [Required(ErrorMessage = "آی دی خرید وارد کنید")]
        public long UserEventId { get; set; }
        [Required(ErrorMessage = "مقدار خرید به گرم را وارد کنید")]
        public long Geram { get; set; }
    }
}
