using GoldApplication.WebAPI.Contexts;
using GoldApplication.WebAPI.Utilities.MessagesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Services.User
{
    public class UserRepository : IUserRepository
    {
        private readonly GoldApplicationContext _context;
        public UserRepository(GoldApplicationContext context)
        {
            _context = context;
        }

        public ActiveMessage Active(long id)
        {
            var user = _context.Users.Find(id);
            if(user.IsActive)
            {
                user.IsActive = false;
                if(Update(user))
                {
                    return new()
                    {
                        Message="کاربر با موفقیت غیر فعال شد و دسترسی ندارد .",
                        Result=true
                    };
                }
                return new()
                {
                    Message = "خطای سیستمی !!! لطفا مجددا تلاش کنید  .",
                    Result = false
                };
            }
            user.IsActive = true;
            if (Update(user))
            {
                return new()
                {
                    Message = "کاربر با موفقیت  فعال شد و به اپلیکیشن دسترسی دارد .",
                    Result = true
                };
            }
            return new()
            {
                Message = "خطای سیستمی !!! لطفا مجددا تلاش کنید  .",
                Result = false
            };
        }

        public bool Create(Models.User user)
        {
            _context.Users.Add(user);
            return Save();
        }

        public ICollection<Models.User> GetAll() =>
            _context.Users.ToList();

        public Models.User GetById(long id) =>
            _context.Users.FirstOrDefault(u => u.UserId == id);

        public Models.User GetByMobile(string mobile) =>
            _context.Users.SingleOrDefault(u => u.Mobile == mobile.Trim().ToLower());

        public bool IsExistMobileForCreate(string mobile) =>
            _context.Users.Any(u => u.Mobile == mobile.Trim().ToLower());

        public bool IsExistMobileForUpdate(long id, string mobile) =>
            _context.Users.Any(u => u.UserId != id && u.Mobile == mobile.Trim().ToLower());

        public bool IsExistUserById(long id) =>
            _context.Users.Any(u => u.UserId == id);

        public bool IsExistUserByMobile(string mobile) =>
            _context.Users.Any(u => u.Mobile == mobile.Trim().ToLower());

        public bool Save()=>
        _context.SaveChanges() >= 0 ? true : false;

        public bool Update(Models.User user)
        {
            _context.Users.Update(user);
            return Save();
        }
    }
}
