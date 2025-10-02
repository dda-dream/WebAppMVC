namespace WebAppMVC_Models.ViewModels
{
    public class ProductUserViewModel
    {
        public ProductUserViewModel()
        {
            ProductList = new List<ProductModel>();
        }
        public IList<ProductModel> ProductList { get; set; }
        public ApplicationUser  ApplicationUser { get; set; }
    }
}
