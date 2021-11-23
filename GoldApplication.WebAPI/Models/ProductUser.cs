using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Models
{
    public class ProductUser
    {
        public ProductUser()
        {

        }
        [Key]
        public long ProductUserId { get; set; }
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public long Geram { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
