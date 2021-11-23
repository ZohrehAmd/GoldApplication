using GoldApplication.WebAPI.Contexts;
using GoldApplication.WebAPI.Models;
using GoldApplication.WebAPI.Utilities.MessagesAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Services.Event
{
    public class EventRepository : IEventRepository
    {
        private readonly GoldApplicationContext _context;
        public EventRepository(GoldApplicationContext context)
        {
            _context = context;
        }

        public ActiveMessage ActiveEvent(long id)
        {
            var productEvent = _context.ProductEvents.Find(id);
            if(productEvent.IsActive)
            {
                productEvent.IsActive = false;
               if(Update(productEvent))
                {
                    return new()
                    {
                        Message = "معامله با موفقیت غیر فعال شد .",
                        Result = true
                    };
                }
                return new()
                {
                    Message = "خطای سیستمی !!! مجددا تلاش کنید .",
                    Result = false
                };
            }
            productEvent.IsActive = true;
            if (Update(productEvent))
            {
                return new()
                {
                    Message = "معامله با موفقیت فعال شد .",
                    Result = true
                };
             }
            return new()
            {
                Message = "خطای سیستمی !!! مجددا تلاش کنید .",
                Result = false
            };
        }

        public bool Create(ProductEvent productEvent)
        {
            _context.ProductEvents.Add(productEvent);
            return Save();
        }

        public ICollection<ProductEvent> GetAll()=>
              _context.ProductEvents.Include(p => p.Product).ToList();

        public ICollection<ProductEvent> GetAllEventForProduct(long id) =>
            _context.ProductEvents.Include(p=>p.Product).Where(p => p.ProductId == id).ToList();

        public ProductEvent GetById(long id) =>
            _context.ProductEvents.Include(e=>e.Product).FirstOrDefault(p => p.ProductEventId == id);

        public bool IsExistById(long id) =>
            _context.ProductEvents.Any(p => p.ProductEventId == id);

        public bool Save() =>
            _context.SaveChanges() >= 0 ? true : false;

        public bool Update(ProductEvent productEvent)
        {
            _context.ProductEvents.Update(productEvent);
            return Save();
        }
    }
}
