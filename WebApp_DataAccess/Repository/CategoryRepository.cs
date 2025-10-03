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
    public class CategoryRepository : Repository<CategoryModel>, ICategoryRepository
    {
        //private readonly ApplicationDbContext db;
        public CategoryRepository(ApplicationDbContext _db) : base(_db)
        {
            //db = _db;
        }

        public void Update(CategoryModel category)
        {
            var objFromDb = db.Category.FirstOrDefault(c => c.Id == category.Id);
            //var objFromDb1 = base.FirstOrDefault(c => c.Id == category.Id); // почему не работает??? см. видео. 023 из второй части.

            if (objFromDb != null)
            {
                objFromDb.Name = category.Name;
                objFromDb.DisplayOrder = category.DisplayOrder;
            }
        }
    }
}
