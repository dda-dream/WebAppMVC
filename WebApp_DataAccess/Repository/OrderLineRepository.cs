using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp_DataAccess.Data;
using WebApp_DataAccess.Repository.IRepository;
using WebApp_Models;
using WebAppMVC_Models;

namespace WebApp_DataAccess.Repository
{
    public class OrderLineRepository : Repository<OrderLine>, IOrderLineRepository
    {
        
        public OrderLineRepository(ApplicationDbContext _db) : base(_db)
        {
        }

        public void Update(OrderLine orderLine)
        {
            db.OrderLine.Update(orderLine);
        }
    }
}
