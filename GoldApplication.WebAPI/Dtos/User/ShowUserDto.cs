using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Dtos.User
{
    public class ShowUserDto
    {
        public long UserId { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime RegisterDate { get; set; }

    }
}
