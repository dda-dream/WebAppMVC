using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using WebAppMVC.Data;
using WebAppMVC.Models;
using WebAppMVC.Utility;

namespace WebAppMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CartController(ApplicationDbContext _db)
        {
            this._db = _db;

        }

        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            var session = HttpContext.Session;
            //var session_cart_serialized = HttpContext.Session.Get(WebConstants.SessionCart);
            //var d = JsonSerializer.Deserialize<IEnumerable>(session_cart_serialized);


            IEnumerable<ShoppingCart> enumeratorShoppingCart = session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart);

            if (enumeratorShoppingCart != null && enumeratorShoppingCart.Any() )
            {
                shoppingCartList = (List<ShoppingCart>)enumeratorShoppingCart;
            }

            List<int> prodInCart = shoppingCartList.Select(p => p.ProductId).ToList();
            IEnumerable<ProductModel> prodList = _db.Product.Where(p => prodInCart.Contains(p.Id)); 


            return View(prodList);
        }
    }
}
