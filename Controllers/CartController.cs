using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;
using System.Text;
using WebApp_DataAccess.Data;
using WebAppMVC_Models;
using WebAppMVC_Models.ViewModels;
using WebAppMVC_Utility;

namespace WebAppMVC.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public ProductUserViewModel ProductUserViewModel { get; set; }
        
        public CartController(ApplicationDbContext _db, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            this._db = _db;
            this._webHostEnvironment = webHostEnvironment;
            this._emailSender = emailSender;
        }
//------------------------------//------------------------------//------------------------------//------------------------------//------------------------------
        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            IEnumerable<ShoppingCart> enumeratorShoppingCart = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart);

            if (enumeratorShoppingCart != null && enumeratorShoppingCart.Any())
            {
                shoppingCartList = (List<ShoppingCart>)enumeratorShoppingCart;
            }

            List<int> prodInCart = shoppingCartList.Select(p => p.ProductId).ToList();
            IEnumerable<ProductModel> prodList = _db.Product.Where(p => prodInCart.Contains(p.Id));

            return View(prodList);
        }

//------------------------------//------------------------------//------------------------------//------------------------------//------------------------------
        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            IEnumerable<ShoppingCart> enumeratorShoppingCart = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart);

            if (enumeratorShoppingCart != null && enumeratorShoppingCart.Any())
            {
                shoppingCartList = (List<ShoppingCart>)enumeratorShoppingCart;
            }

            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(a => a.ProductId == id));

            List<int> prodInCart = shoppingCartList.Select(p => p.ProductId).ToList();
            IEnumerable<ProductModel> prodList = _db.Product.Where(p => prodInCart.Contains(p.Id));
            
            HttpContext.Session.Set( WC.SessionCart, shoppingCartList );

            return RedirectToAction(nameof(Index));
        }
        //------------------------------//------------------------------//------------------------------//------------------------------//------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }

        //------------------------------//------------------------------//------------------------------//------------------------------//------------------------------
        public IActionResult Summary()
        {
            var claims = (ClaimsIdentity)User.Identity;
            var claim = claims.FindFirst(ClaimTypes.NameIdentifier);
            var userId = User.FindFirstValue(ClaimTypes.Name);


            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            IEnumerable<ShoppingCart> enumeratorShoppingCart = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart);

            if (enumeratorShoppingCart != null && enumeratorShoppingCart.Any())
            {
                shoppingCartList = (List<ShoppingCart>)enumeratorShoppingCart;
            }

            List<int> prodInCart = shoppingCartList.Select(p => p.ProductId).ToList();
            IEnumerable<ProductModel> prodList = _db.Product.Where(p => prodInCart.Contains(p.Id));

            ProductUserViewModel = new ProductUserViewModel();
            ProductUserViewModel.ApplicationUser = _db.ApplicationUser.FirstOrDefault(q => q.Id == claim.Value);
            ProductUserViewModel.ProductList = prodList.ToList();    

            return View(ProductUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserViewModel productUserViewModel)
        {
            var htmlTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "templates" + Path.DirectorySeparatorChar.ToString() + "OrderConfirmation.html";

            var subject = "Новый заказ";
            var htmlBody = "";

            using (StreamReader streamReader = new StreamReader(htmlTemplate))
            {
                htmlBody = streamReader.ReadToEnd();
            };
            //Name  : {0}
            //Email &nbsp;: {1}
            //Phone : {2}
            //Products: {3}            
            StringBuilder productSB = new StringBuilder();
            foreach (var item in productUserViewModel.ProductList)
            {
                productSB.AppendLine($" - Name :{item.Name} <span style='font-size:14px;'> (ID: {item.Id}) </span><br/>");
            }
            
            string messageBody = string.Format(htmlBody,
                productUserViewModel.ApplicationUser.FullName,
                productUserViewModel.ApplicationUser.Email,
                productUserViewModel.ApplicationUser.PhoneNumber,
                productSB.ToString());


            await _emailSender.SendEmailAsync(WC.AdminEmail, subject, messageBody);

            return RedirectToAction(nameof(OrderConfirmation));
        }   

        public IActionResult OrderConfirmation()
        {
           

            

            HttpContext.Session.Clear();
            return View();
        }   


    
    }
}
