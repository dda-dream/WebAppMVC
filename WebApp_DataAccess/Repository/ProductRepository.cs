using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp_DataAccess.Data;
using WebApp_DataAccess.Repository.DTO;
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
            db.Product.Update(product);
        }

        IEnumerable<ProductSelector> IProductRepository.GetProductListByPattern(string pattern)
        {
            var retVal = db.Product
                    .Where(p => p.Name.Contains(pattern) || p.ShortDesc.Contains(pattern))
                    .Select(r => new ProductSelector() { Id = r.Id, Name = r.Name, ShortDesc = r.ShortDesc, Price = r.Price })
                    .ToList();
            ;

            return retVal;
        }
    }
}
