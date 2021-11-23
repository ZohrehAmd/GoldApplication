using GoldApplication.WebAPI.Models;
using GoldApplication.WebAPI.Utilities.MessagesAPI;
using System.Collections.Generic;

namespace GoldApplication.WebAPI.Services.Product
{
    public interface IProductRepository
    {
        ICollection<Models.Product> GetAll();
        Models.Product GetProductById(long id);
        bool Save();
        bool Create(Models.Product create);
        bool IsExistProductWithTitleForCreate(string title);
        Models.Product GetByTitle(string title);
        bool IsExistProductWithTitleForUpdate(long productId, string title);
        bool Update(Models.Product update);
        bool IsExistProductById(long id);
        ActiveMessage ActiveProduct(long id);
    }
}
