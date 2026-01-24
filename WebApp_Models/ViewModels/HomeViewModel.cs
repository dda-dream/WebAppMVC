namespace WebAppMVC_Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<ProductModel> Products { get; set; }
        public IEnumerable<CategoryModel> Categories { get; set; }


        public ProductModel Product { get; set; }
        public CategoryModel Category { get; set; }

    }
}
