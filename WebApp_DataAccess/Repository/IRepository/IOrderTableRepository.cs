using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp_Models;
using WebAppMVC_Models;

namespace WebApp_DataAccess.Repository.IRepository
{
    public interface IOrderTableRepository : IRepository<OrderTable>
    {
        void Update(OrderTable orderTable);
    }
}
