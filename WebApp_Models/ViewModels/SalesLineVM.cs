using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp_Models.ViewModels
{
    public class SalesLineListVM
    {
        public SalesTable SalesTable { get; set; }
        public IEnumerable<SalesLine> SalesLine { get; set;}

    }
}
