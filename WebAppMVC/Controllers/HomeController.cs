using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using WebApp_DataAccess.Data;
using WebApp_DataAccess.Repository.IRepository;
using WebAppMVC_Models;
using WebAppMVC_Models.ViewModels;
using WebAppMVC_Utility;

namespace WebAppMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;


        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }
         
//---------------------------------------------------------------------------------------------------

        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel();
            homeViewModel.Products = productRepository.GetAll(includeProperties:"Category");//.Include(u => u.Category); //Eager loading - жадная загрузка.
            homeViewModel.Categories = categoryRepository.GetAll();

            string headers="";
            foreach (var i in HttpContext.Request.Headers)
            {
                headers = headers + $"\n{i.Key} = {i.Value}";
            }

            _logger.LogInformation($"\n---------------------------{HttpContext.Connection.RemoteIpAddress} {HttpContext.Connection.RemotePort}" +
                $"\nCookies count:{HttpContext.Request.Cookies.Count}" +
                $"\nHeaders count:{HttpContext.Request.Headers.Count}" +
                $"\n-----------------------------------------------------------" +
                $"{headers}" +
                $"\n-----------------------------------------------------------" +
                $"\n");


            var builder = LoggerFactory.Create(b => b.AddConsole());

            ILogger<string> logger1 = builder.CreateLogger<string>();
            logger1.LogInformation("testLogMessage");

            return View(homeViewModel);
        } 



//---------------------------------------------------------------------------------------------------


        public IActionResult Details(int id)
        {
            /*
            DetailsViewModel detailsViewModel = new DetailsViewModel();
            detailsViewModel.Product = productRepository.Find(id);
            detailsViewModel.Product.Category = categoryRepository.Find(detailsViewModel.Product.CategoryId);
            detailsViewModel.ExistsInCart = false;
            */
            DetailsViewModel detailsViewModel = new DetailsViewModel()
            {
                Product = productRepository.GetAll(includeProperties:"Category", filter: i => i.Id == id).FirstOrDefault(),
                //Product.Category = categoryRepository.Find(detailsViewModel.Product.CategoryId),
                ExistsInCart = false
            };



            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            var shoppingCart = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart);
            if (shoppingCart != null && shoppingCart.Count() > 0 )
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

            var shoppingCart = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart);
            if (shoppingCart != null && shoppingCart.Count() > 0 )
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

            var shoppingCart = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart);
            if (shoppingCart != null && shoppingCart.Count() > 0 )
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
