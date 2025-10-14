using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WebApp_DataAccess.Repository.IRepository;
using WebApp_Models;
using WebApp_Models.ViewModels;
using WebAppMVC_Models;
using WebAppMVC_Utility;

namespace WebAppMVC.Controllers
{
    public class OrderController : Controller
    {
        IOrderTableRepository orderTableRepository;
        IOrderLineRepository orderLineRepository;

        public OrderController(IOrderTableRepository orderTableRepository, IOrderLineRepository orderLineRepository)
        {
            this.orderTableRepository = orderTableRepository;
            this.orderLineRepository = orderLineRepository;
        }

        [Authorize(Roles = WC.AdminRole)]
        public IActionResult Index()
        {
            IEnumerable<OrderTable> ot = orderTableRepository.GetAll();

            return View(ot);
        }

        public IActionResult Details( int? id )
        {
            OrderVM orderVM = new OrderVM();
            
            orderVM.orderTable = orderTableRepository.Find(id ?? 0);
            orderVM.orderLine_Enumerator = orderLineRepository.GetAll(l => l.OrderId == id, includeProperties:"Product");

            return View(orderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(OrderVM order)
        { 
            OrderVM orderVM = new OrderVM();
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            
            orderVM.orderTable = orderTableRepository.Find(order.orderTable.Id);

            foreach (var i in orderLineRepository.GetAll(l => l.OrderId == orderVM.orderTable.Id))
            {
                ShoppingCart shoppingCart = new ShoppingCart();
                shoppingCart.ProductId = i.ProductId;
                shoppingCartList.Add(shoppingCart);
            }
            HttpContext.Session.Clear();
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            HttpContext.Session.Set<int>(WC.SessionOrderId, order.orderTable.Id);

            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public IActionResult Delete(OrderVM order)
        {
            var orderTable = orderTableRepository.Find(order.orderTable.Id);
            var orderLineRange = orderLineRepository.GetAll(filter: l => l.OrderId == orderTable.Id);

            orderTableRepository.Remove(orderTable);
            orderLineRepository.RemoveRange(orderLineRange);

            orderTableRepository.Save();
            orderLineRepository.Save();

            return RedirectToAction(nameof(Index));
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetOrdersList()
        {
            var ot = orderTableRepository.GetAll();
            JsonResult o = Json(ot);

            var ret = Json(new { data = ot });
            return ret;
        }
        #endregion
    }
}
