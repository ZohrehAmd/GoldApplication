using GoldApplication.WebAPI.Utilities.MessagesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Services.User
{
    public interface IUserRepository
    {
        ICollection<Models.User> GetAll();
        Models.User GetById(long id);
        Models.User GetByMobile(string mobile);
        bool IsExistMobileForCreate(string mobile);
        bool IsExistMobileForUpdate(long id ,string mobile);
        bool Create(Models.User user);
        bool Update(Models.User user);
        ActiveMessage Active(long id);
        bool Save();
        bool IsExistUserByMobile(string mobile);
        bool IsExistUserById(long id);
    }
}
