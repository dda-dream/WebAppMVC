using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp_DataAccess.Data;
using WebApp_DataAccess.Repository.IRepository;
using WebAppMVC_Models;

namespace WebApp_DataAccess.Repository
{
    public class ProductRepository : Repository<ProductModel>, IProductRepository
    {
        
        public ProductRepository(ApplicationDbContext _db) : base(_db)
        {
        }

        public IEnumerable<SelectListItem> GetCategoryDropDownList()
        {
            IQueryable<SelectListItem> retVal = db.Category.Select(i => new SelectListItem
                                                {
                                                    Text = i.Name,
                                                    Value = i.Id.ToString(),
                                                });
            return retVal;
        }
        public void Update(ProductModel product)
        {
            db.Update(product);
            /*
            var obj = db.Product.FirstOrDefault(c => c.Id == product.Id);

            if (obj != null)
            {
                obj.ShortDesc = product.ShortDesc;
                obj.Category = product.Category;
                obj.CategoryId = product.CategoryId;
                obj.Price   = product.Price;
                obj.Description = product.Description;
                obj.Image = product.Image;
                obj.Name = product.Name;
            }
            */
        }
    }
}
