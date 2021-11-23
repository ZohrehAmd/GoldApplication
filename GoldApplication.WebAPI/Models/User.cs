using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Models
{
    public class User
    {
        public User()
        {

        }
        [Key]
        public long UserId { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        [MaxLength(150)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(150)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        public string Mobile { get; set; }
        [Required]
        [MaxLength(60)]
        public string Avatar { get; set; }
        public string Description { get; set; }
        [Required]
        public string Password { get; set; }
        [NotMapped]
        public string Token { get; set; }
        [Required]
        public string RegisterCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime RegisterDate { get; set; }

    }
}
