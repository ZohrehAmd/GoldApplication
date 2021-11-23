using GoldApplication.WebAPI.Models;
using GoldApplication.WebAPI.Utilities.MessagesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Services.Event
{
    public interface IEventRepository
    {
        ICollection<ProductEvent> GetAll();
        ICollection<ProductEvent> GetAllEventForProduct(long id);
        ProductEvent GetById(long id);
        bool IsExistById(long id);
        bool Create(ProductEvent productEvent);
        bool Update(ProductEvent productEvent);
        ActiveMessage ActiveEvent(long id);
        bool Save();
    }
}
