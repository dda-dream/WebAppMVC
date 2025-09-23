using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebAppMVC.Models.ViewModels
{
    public class ProductViewModel
    {
        public ProductModel Product { get; set; }

        public IEnumerable<SelectListItem> CategorySelectList { get; set; }

    }
}
