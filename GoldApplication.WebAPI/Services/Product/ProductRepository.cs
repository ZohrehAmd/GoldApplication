using GoldApplication.WebAPI.Contexts;
using GoldApplication.WebAPI.Utilities.MessagesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Services.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly GoldApplicationContext _context;
        public ProductRepository(GoldApplicationContext context)
        {
            _context = context;
        }
        public ActiveMessage ActiveProduct(long id)
        {
            var product = _context.Products.Find(id);
            if (product.IsActive)
            {
                product.IsActive = false;
                _context.Products.Update(product);
                if (Save())
                {
                    return new()
                    {
                        Message = "محصول با موفقیت غیر فعال شد .",
                        Result = true
                    };
                }

            return new()
            {
                Message = "عملیات نا موفق !!! مجددا تلاش کنید .",
                Result=false
            };

            }
            product.IsActive = true;
            _context.Products.Update(product);
            if (Save())
                return new()
                {
                    Message = "محصول با موفقیت  فعال شد .",
                    Result = true
                };

            return new()
            {
                Message = "عملیات نا موفق !!! مجددا تلاش کنید .",
                Result = false
            };
        }

        public bool Create(Models.Product create)
        {
            _context.Products.Add(create);
            return Save();
        }

        public ICollection<Models.Product> GetAll() =>
            _context.Products.ToList();

        public Models.Product GetByTitle(string title)=>
            _context.Products.FirstOrDefault(p => p.Title.ToLower().Trim() == title.ToLower().Trim());

        public Models.Product GetProductById(long id) =>
            _context.Products.FirstOrDefault(p => p.ProductId == id);

        public bool IsExistProductById(long id) =>
            _context.Products.Any(p => p.ProductId == id);

        public bool IsExistProductWithTitleForCreate(string title) =>
            _context.Products.Any(p => p.Title == title.ToLower().Trim());

        public bool IsExistProductWithTitleForUpdate(long productId, string title) =>
            _context.Products.Any(p => p.ProductId != productId && p.Title.ToLower().Trim() == title.ToLower().Trim());

        public bool Save() =>
            _context.SaveChanges() >= 0 ? true : false;

        public bool Update(Models.Product update)
        {
            _context.Products.Update(update);
            return Save();
        }
    }
}
