using GoldApplication.WebAPI.Dtos.UserEvent;
using GoldApplication.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Services.UserEvent
{
    public interface IUserEventRepository
    {
        ICollection<Models.UserEvent> GetAllForEvent(long eventId);
        ICollection<Models.UserEvent> GetAllForUser(long userId);
        Models.UserEvent GetById(long userEventId);
        bool Create(Models.UserEvent userEvent);
        bool Update(Models.UserEvent userEvent);
        bool Delete(Models.UserEvent userEvent);
        bool Save();
        Models.UserEvent AddTimeToProductUser(long userId, Models.UserEvent userEvent);
        bool IsExist(long userId, long productEventId);
        bool HaveMojoodi(ProductEvent eventt, long geram);
        bool HaveMojoodiForUpdate(ProductEvent eventt, UpdateUserEventDto update);
    }
}
