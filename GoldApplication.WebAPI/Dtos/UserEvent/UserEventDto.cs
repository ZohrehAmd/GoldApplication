using GoldApplication.WebAPI.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Dtos.UserEvent
{
    public class UserEventDto
    {
        public long UserEventId { get; set; }
        public long UserId { get; set; }
        public long ProductEventId { get; set; }
        public long Geram { get; set; }
        public string Time { get; set; }
        public ShowUserDto User { get; set; }
        public EventDto ProductEvent { get; set; }
    }
}
