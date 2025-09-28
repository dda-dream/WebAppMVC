namespace WebAppMVC.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<ProductModel> Products { get; set; }
        public IEnumerable<CategoryModel> Categories { get; set; }


        public ProductModel Product;
        public CategoryModel Category;

    }
}
