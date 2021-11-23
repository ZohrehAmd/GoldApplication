using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Models
{
    public class Product
    {
        public Product()
        {

        }
        [Key]
        public long ProductId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [Required]
        [MaxLength(60)]
        public string ImageName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }


    }
}
