using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp_DataAccess.Repository.DTO;
using WebAppMVC_Models;

namespace WebApp_DataAccess.Repository.IRepository
{



    public interface IProductRepository : IRepository<ProductModel>
    {
        void Update(ProductModel product);
        public IEnumerable<SelectListItem> GetCategoryDropDownList();
        public IEnumerable<ProductSelector> GetProductListByPattern(string pattern);
    }


}
