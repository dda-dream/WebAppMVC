using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp_DataAccess.Data;
using WebApp_DataAccess.Repository.IRepository;
using WebApp_Models;
using WebAppMVC_Models;

namespace WebApp_DataAccess.Repository
{
    public class SalesLineRepository : Repository<SalesLine>, ISalesLineRepository
    {
        
        public SalesLineRepository(ApplicationDbContext _db) : base(_db)
        {
        }

        public void Update(SalesLine salesLine)
        {
            db.SalesLine.Update(salesLine);
        }
    }
}
