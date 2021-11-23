using GoldApplication.WebAPI.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Services.ProductUser
{
    public class ProductUserRepository : IProductUserRepository
    {
        private readonly GoldApplicationContext _context;
        public ProductUserRepository(GoldApplicationContext context)
        {
            _context = context;
        }

        public bool Create(Models.ProductUser productUser)
        {
            _context.ProductUsers.Add(productUser);
            return Save();
        }

        public bool Delete(Models.ProductUser productUser)
        {
            _context.ProductUsers.Remove(productUser);
            return Save();
        }

        public ICollection<Models.ProductUser> GetAllProductForUser(long userId) =>
            _context.ProductUsers.Include(pu => pu.User)
            .Include(pu => pu.Product).Where(pu => pu.UserId == userId).ToList();

        public ICollection<Models.ProductUser> GetAllUserForProduct(long productId) =>
            _context.ProductUsers.Include(pu => pu.User)
            .Include(pu => pu.Product).Where(pu => pu.ProductId == productId).ToList();

        public Models.ProductUser GetById(long id) =>
            _context.ProductUsers.Find(id);

        public Models.ProductUser GetProductUserForUser(long userId, long productId) =>
            _context.ProductUsers.Include(pu => pu.User)
            .Include(pu => pu.Product).FirstOrDefault(pu => pu.ProductId == productId &&
            pu.UserId == userId);

        public bool IsExist(long userId, long productId) =>
            _context.ProductUsers.Any(pu => pu.UserId == userId && pu.ProductId == productId);

        public bool IsExistProductUserById(long id) =>
            _context.ProductUsers.Any(pu => pu.ProductUserId == id);

        public bool Save() =>
            _context.SaveChanges() >= 0 ? true : false;

        public bool Update(Models.ProductUser productUser)
        {
            _context.ProductUsers.Update(productUser);
            return Save();
        }
    }
}
