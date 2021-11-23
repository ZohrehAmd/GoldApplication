using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Dtos.Product
{
    public class CreateUserProductDto
    {
        [Required(ErrorMessage = "لطفا کاربر را مشخص کنید .")]
        public long UserId { get; set; }
        [Required(ErrorMessage = "لطفا محصول را مشخص کنید .")]
        public long ProductId { get; set; }
        [Required(ErrorMessage = "لطفا وزن را بر اساس گرم مشخص کنید .")]
        public long Geram { get; set; }
    }
}
