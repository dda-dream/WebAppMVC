using Microsoft.AspNetCore.Mvc;

namespace WebAppMVC_Models.ViewModels
{
    public class DetailsViewModel
    {
        public DetailsViewModel()
        {
            Product = new ProductModel();
        }

        public ProductModel Product { get; set; }
        public bool ExistsInCart { get; set; }


    }
}
