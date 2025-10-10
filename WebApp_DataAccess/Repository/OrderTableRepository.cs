using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp_DataAccess.Data;
using WebApp_DataAccess.Repository.IRepository;
using WebApp_Models;
using WebAppMVC_Models;

namespace WebApp_DataAccess.Repository
{
    public class OrderTableRepository : Repository<OrderTable>, IOrderTableRepository
    {
        
        public OrderTableRepository(ApplicationDbContext _db) : base(_db)
        {
        }

        public void Update(OrderTable orderTable)
        {
            db.OrderTable.Update(orderTable);
        }
    }
}
 