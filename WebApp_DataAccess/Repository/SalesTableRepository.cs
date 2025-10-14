using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp_DataAccess.Data;
using WebApp_DataAccess.Repository.IRepository;
using WebApp_Models;
using WebAppMVC_Models;

namespace WebApp_DataAccess.Repository
{
    public class SalesTableRepository : Repository<SalesTable>, ISalesTableRepository
    {
        
        public SalesTableRepository(ApplicationDbContext _db) : base(_db)
        {
        }

        public void Update(SalesTable salesTable)
        {
            db.SalesTable.Update(salesTable);
        }
    }
}
 