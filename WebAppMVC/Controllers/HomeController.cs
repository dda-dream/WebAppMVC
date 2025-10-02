using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using WebApp_DataAccess.Data;
using WebAppMVC_Models;
using WebAppMVC_Models.ViewModels;
using WebAppMVC_Utility;

namespace WebAppMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }
         
//---------------------------------------------------------------------------------------------------

        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel();
            homeViewModel.Products = _db.Product;//.Include(u => u.Category); //Eager loading - жадная загрузка.
            homeViewModel.Categories = _db.Category;

            return View(homeViewModel);
        } 

//---------------------------------------------------------------------------------------------------


        public IActionResult Details(int id)
        {   
            DetailsViewModel detailsViewModel = new DetailsViewModel(); 
            detailsViewModel.Product = _db.Product.Where(_ => _.Id == id).First();
            detailsViewModel.Product.Category = _db.Category.Where(_ => _.Id == detailsViewModel.Product.CategoryId).First();
            detailsViewModel.ExistsInCart = false;

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null 
             && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0
                )
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
                if (shoppingCartList.Find(n => n.ProductId == id) != null)
                    detailsViewModel.ExistsInCart = true;
            }

            return View(detailsViewModel);
        } 


        [HttpPost, ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {   
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();


            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null 
             && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0
                )
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            shoppingCartList.Add(new ShoppingCart{ ProductId = id });
            HttpContext.Session.Set( WC.SessionCart, shoppingCartList );

            return RedirectToAction(nameof(Index));
        } 

//---------------------------------------------------------------------------------------------------

        public IActionResult RemoveFromCart(int id)
        {   
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null 
             && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0
                )
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            var itemToRemove = shoppingCartList.Find(n => n.ProductId == id);
            
            shoppingCartList.Remove(itemToRemove);
            HttpContext.Session.Set( WC.SessionCart, shoppingCartList );

            return RedirectToAction(nameof(Index));
        } 

//---------------------------------------------------------------------------------------------------

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
