using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Dtos.UserEvent
{
    public class CreateUserEventByUserDto
    {
        [Required]
        public long ProductEventId { get; set; }
        [Required]
        public long Geram { get; set; }
    }
}
