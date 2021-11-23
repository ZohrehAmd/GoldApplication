using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Models
{
    public class ProductEvent
    {
        public ProductEvent()
        {

        }
        [Key]
        public long ProductEventId { get; set; }
        public long ProductId { get; set; }
        [Required]
        public long Geram { get; set; }
        [Required]
        public long GeramPrice { get; set; }
        [Required]
        [MaxLength(250)]
        public string Title { get; set; }
        [Required]
        [MaxLength(150)]
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DateEvent { get; set; }
        [Required]
        [MaxLength(8)]
        [MinLength(8)]
        public string StartTime { get; set; }
        [Required]
        [MaxLength(8)]
        [MinLength(8)]
        public string EndTime { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
