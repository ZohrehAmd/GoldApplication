using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldApplication.WebAPI.Models;

namespace GoldApplication.WebAPI.Services.ProductUser
{
    public interface IProductUserRepository
    {
        ICollection<Models.ProductUser> GetAllUserForProduct(long productId);
        ICollection<Models.ProductUser> GetAllProductForUser(long userId);
        Models.ProductUser GetProductUserForUser(long userId, long productId);
        bool Create(Models.ProductUser productUser);
        bool Update(Models.ProductUser productUser);
        bool Delete(Models.ProductUser productUser);
        bool IsExist(long userId, long productId);
        bool Save();
        bool IsExistProductUserById(long id);
        Models.ProductUser GetById(long id);
    }
}
