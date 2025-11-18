using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using WebApp_DataAccess.Data;
using WebApp_DataAccess.Repository;
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
        private readonly IApplicationUserRepository applicationUserRepository;


        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ICategoryRepository categoryRepository,
            IApplicationUserRepository applicationUserRepository )
        {
            _logger = logger;
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.applicationUserRepository = applicationUserRepository;
        }
         
//---------------------------------------------------------------------------------------------------

        
        [HttpGet("{id}")]
        public IActionResult Index(int id)
        {
            //FileStream fs = new FileStream("C:\\Temp\\test.txt", FileMode.Open, FileAccess.Read);
            return Ok();
        }

        [HttpGet("Error404/{Id}")]
        public IActionResult Error404(int Id)
        {
            //FileStream fs = new FileStream("C:\\Temp\\test.txt", FileMode.Open, FileAccess.Read);
            return Ok();
        }

        
        [HttpGet]
        public IActionResult Index()
        {
            string headers="";
            foreach (var i in HttpContext.Request.Headers)
            {
                headers = headers + $"\n<br>{i.Key} ===> {i.Value}";
            }

            _logger.LogInformation(
                $"\n<br>---------------------------{HttpContext.Connection.RemoteIpAddress} {HttpContext.Connection.RemotePort}" +
                $"\n<br>Cookies count:{HttpContext.Request.Cookies.Count}" +
                $"\n<br>Headers count:{HttpContext.Request.Headers.Count}" +
                $"\n<br>-----------------------------------------------------------" +
                $"<br>{headers}" +
                $"<br>\n-----------------------------------------------------------" +
                $"<br>\n");


            var builder = LoggerFactory.Create(b => b.AddConsole());

            ILogger<string> logger1 = builder.CreateLogger<string>();
            logger1.LogInformation("testLogMessage");



            //test
            var claim = User.Claims.FirstOrDefault();
            var userid = "70f806fa-3869-4eef-a9f2-6c0d3a54554e";//claim.Value; Admin Admin
            var msgCount = applicationUserRepository.GetUserChatMessagesCount(userid);
            var msgCountByDay = applicationUserRepository.GetUserChatMessagesCountByDay(userid);
            //test

            List<string> sList = new List<string>();
            sList.Add("--- :HEADERS: ---");
            sList.Add($"{headers}");
            sList.Add("<br><br>--- :msgCountByDay: ---");
            foreach(var i in msgCountByDay)
            {
                sList.Add($"<br>{i.Key} - {i.Value}");
            }
            sList.Add("--- :END: ---");
            return View(sList);
        } 



        [HttpGet("Shop")]
        public IActionResult Shop()
        {
            var homeViewModel = new HomeViewModel();
            homeViewModel.Products = productRepository.GetAll(includeProperties:"Category");//Eager loading - жадная загрузка.
            homeViewModel.Categories = categoryRepository.GetAll();

            return View(homeViewModel);
        } 



//---------------------------------------------------------------------------------------------------


        [HttpGet("Details")]
        public IActionResult Details(int id)
        {

            DetailsViewModel detailsViewModel = new DetailsViewModel()
            {
                Product = productRepository.GetAll(includeProperties:"Category", filter: i => i.Id == id).FirstOrDefault(),
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
        public IActionResult DetailsPost(int id, DetailsViewModel dVM)
        {
            //if (ModelState.IsValid)
            {
                List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

                var shoppingCart = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart);
                if (shoppingCart != null && shoppingCart.Count() > 0)
                {
                    shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
                }
                shoppingCartList.Add(new ShoppingCart { 
                                        ProductId = id, 
                                        Qty = dVM.Product.TempQty });
                HttpContext.Session.Set(WC.SessionCart, shoppingCartList);

                return RedirectToAction(nameof(Index));
            }

            TempData.Add(WC.Error, "Ошибка при добавлении в корзину.");
            return View();
        } 

//---------------------------------------------------------------------------------------------------
        [HttpGet("RemoveFromCart")]
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
        [HttpGet("Error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
