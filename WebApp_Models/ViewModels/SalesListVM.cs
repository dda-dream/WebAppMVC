using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp_Models.ViewModels
{
    public class SalesListVM
    {
        public IEnumerable<SalesTable> SalesTable { get; set;}
        public IEnumerable<SelectListItem> StatusList { get; set;}
        public string Status {get; set;}


    }
}
