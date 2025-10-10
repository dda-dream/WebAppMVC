using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp_Models.ViewModels
{
    public class OrderVM
    {
        public OrderTable orderTable { get; set; }
        public IEnumerable<OrderLine> orderLine_Enumerator { get; set; }
        
    }
}
