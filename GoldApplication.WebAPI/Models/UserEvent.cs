using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Models
{
    public class UserEvent
    {
        public UserEvent()
        {

        }
        [Key]
        public long UserEventId { get; set; }
        public long UserId { get; set; }
        public long ProductEventId { get; set; }
        public long Geram { get; set; }
        [Required]
        [MaxLength(8)]
        [MinLength(8)]
        public string Time { get; set; }


        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("ProductEventId")]
        public virtual ProductEvent ProductEvent { get; set; }
    }
}
