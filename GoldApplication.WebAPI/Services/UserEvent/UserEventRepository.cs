using GoldApplication.WebAPI.Contexts;
using GoldApplication.WebAPI.Dtos.UserEvent;
using GoldApplication.WebAPI.Models;
using GoldApplication.WebAPI.Utilities.Date;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Services.UserEvent
{
    public class UserEventRepository : IUserEventRepository
    {
        private readonly GoldApplicationContext _context;
        public UserEventRepository(GoldApplicationContext context)
        {
            _context = context;
        }

        public Models.UserEvent AddTimeToProductUser(long userId, Models.UserEvent userEvent)
        {
            userEvent.Time = TimeFormat.GetStringTime();
            userEvent.UserId = userId;
            return userEvent;
        }

        public bool Create(Models.UserEvent userEvent)
        {
            _context.UserEvents.Add(userEvent);
            return Save();
        }

        public bool Delete(Models.UserEvent userEvent)
        {
            _context.UserEvents.Remove(userEvent);
            return Save();
        }

        public ICollection<Models.UserEvent> GetAllForEvent(long eventId) =>
            _context.UserEvents.Include(ue => ue.User).
            Where(ue => ue.ProductEventId == eventId).ToList();

        public ICollection<Models.UserEvent> GetAllForUser(long userId) =>
            _context.UserEvents.Include(ue => ue.ProductEvent).ThenInclude(ue => ue.Product)
            .Where(ue => ue.UserId == userId).ToList();

        public Models.UserEvent GetById(long userEventId) =>
            _context.UserEvents.Include(ue => ue.ProductEvent).ThenInclude(ue => ue.Product)
            .Include(ue => ue.User).FirstOrDefault(ue => ue.UserEventId == userEventId);

        public bool HaveMojoodi(ProductEvent eventt, long geram)
        {
            long sum = _context.UserEvents.Where(u => u.ProductEventId == eventt.ProductEventId).Sum(u => u.Geram);
            if (sum + geram > eventt.Geram)
                return false;

            return true;
        }

        public bool HaveMojoodiForUpdate(ProductEvent eventt, UpdateUserEventDto update)
        {
            long sum = _context.UserEvents.Where(u => u.ProductEventId == eventt.ProductEventId && u.UserEventId != update.UserEventId).Sum(u => u.Geram);
            if (sum + update.Geram > eventt.Geram)
                return false;

            return true;
        }

        public bool IsExist(long userId, long productEventId) => 
            _context.UserEvents.Any(u => u.UserId == userId && u.ProductEventId == productEventId);

        public bool Save() =>
            _context.SaveChanges() >= 0 ? true : false;

        public bool Update(Models.UserEvent userEvent)
        {
            _context.UserEvents.Update(userEvent);
            return Save();
        }
    }
}
