using Braintree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using WebApp_DataAccess.Data;
using WebApp_DataAccess.Repository;
using WebApp_DataAccess.Repository.IRepository;
using WebApp_Models;
using WebApp_Utility.BrainTree;
using WebAppMVC_Models;
using WebAppMVC_Models.ViewModels;
using WebAppMVC_Utility;

namespace WebAppMVC.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;

        private readonly IProductRepository productRepository;
        private readonly IApplicationUserRepository applicationUserRepository;
        private readonly IOrderTableRepository orderTableRepository;
        private readonly IOrderLineRepository orderLineRepository;

        private readonly ISalesTableRepository salesTableRepository;
        private readonly ISalesLineRepository salesLineRepository;
        private readonly IBrainTreeGate brainTreeGate;

        [BindProperty]
        public ProductUserViewModel ProductUserViewModel { get; set; }


        public CartController(IApplicationUserRepository applicationUserRepository, IProductRepository productRepository,
                              IWebHostEnvironment webHostEnvironment, IEmailSender emailSender,
                              IOrderTableRepository orderTableRepository, IOrderLineRepository orderLineRepository,
                              ISalesTableRepository salesTableRepository, ISalesLineRepository salesLineRepository,
                              IBrainTreeGate brainTreeGate
                              )
        {
            this._webHostEnvironment = webHostEnvironment;
            this._emailSender = emailSender;
            this.productRepository = productRepository;
            this.applicationUserRepository = applicationUserRepository;
            this.orderTableRepository = orderTableRepository;
            this.orderLineRepository = orderLineRepository;
            this.salesTableRepository = salesTableRepository;
            this.salesLineRepository = salesLineRepository;
            this.brainTreeGate = brainTreeGate;
        }
//------------------------------//------------------------------//------------------------------//------------------------------//------------------------------
        public IActionResult Index() // GET
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            IEnumerable<ShoppingCart> enumeratorShoppingCart = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart);

            if (enumeratorShoppingCart != null && enumeratorShoppingCart.Any())
            {
                shoppingCartList = (List<ShoppingCart>)enumeratorShoppingCart;
            }

            List<int> prodInCart = shoppingCartList.Select(p => p.ProductId).ToList();
            IEnumerable<ProductModel> prodList = productRepository.GetAll(p => prodInCart.Contains(p.Id));

            foreach (ProductModel prod in prodList)
            {
                prod.TempQty = shoppingCartList.Where(p => p.ProductId == prod.Id).ToList().FirstOrDefault().Qty;
            }

            return View(prodList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult Index(IEnumerable<ProductModel> products) //POST
        {
            var shoppingCart = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart);

            foreach (var i in products)
            {
                var s = shoppingCart.Where(c => c.ProductId == i.Id).FirstOrDefault();
                s.Qty = i.TempQty;
            }

            HttpContext.Session.Set(WC.SessionCart, shoppingCart);
            return RedirectToAction(nameof(Summary));
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
            IEnumerable<ProductModel> prodList = productRepository.GetAll(p => prodInCart.Contains(p.Id));
            
            HttpContext.Session.Set( WC.SessionCart, shoppingCartList );

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary() //GET
        {
            var claims = (ClaimsIdentity)User.Identity;
            var claim = claims.FindFirst(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>(); 
            IEnumerable<ShoppingCart> enumeratorShoppingCart = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart);

            int isSessionOrderId=0;
            var key = HttpContext.Session.Keys.Where(s => s == WC.SessionOrderId);
            var key1 = key.ToList();
            var keyAny = key.Any();

            if (HttpContext.Session.Keys.Where(s => s == WC.SessionOrderId).Any())
                isSessionOrderId = HttpContext.Session.Get<int>(WC.SessionOrderId);

            if (enumeratorShoppingCart != null && enumeratorShoppingCart.Any())
            {
                shoppingCartList = (List<ShoppingCart>)enumeratorShoppingCart;
            }

            List<int> prodInCart = shoppingCartList.Select(p => p.ProductId).ToList();
            IEnumerable<ProductModel> prodList = productRepository.GetAll(p => prodInCart.Contains(p.Id));

            ProductUserViewModel = new ProductUserViewModel();
            ProductUserViewModel.ApplicationUser = applicationUserRepository.FirstOrDefault(q => q.Id == claim.Value);

            foreach (var i in prodList)
            {
                i.TempQty = shoppingCartList.Where(c => c.ProductId == i.Id).FirstOrDefault().Qty;
            }
            ProductUserViewModel.ProductList = prodList.ToList();
            

            if (User.IsInRole(WC.AdminRole))
            {
                if (isSessionOrderId == 0)
                {
                    ProductUserViewModel.ApplicationUser.FullName = "";
                    ProductUserViewModel.ApplicationUser.PhoneNumber = "";
                    ProductUserViewModel.ApplicationUser.Email = "";
                }

                var gateway = brainTreeGate.GetGateway();
                var clientToken = gateway.ClientToken.Generate();
                ViewBag.ClientToken = clientToken;
            }
            return View(ProductUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(IFormCollection collection, ProductUserViewModel productUserViewModel)
        {
            var claims = (ClaimsIdentity)User.Identity;
            var claims_1 = User.Identity;
            var claim = claims.FindFirst(ClaimTypes.NameIdentifier);
            var user_nameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (User.IsInRole(WC.AdminRole))
            {
                double finalOrderTotal = productUserViewModel.ProductList.Sum(i => i.Price * i.TempQty);

                SalesTable salesTable = new SalesTable()
                {
                    CreatedBy = applicationUserRepository.FirstOrDefault(filter: q => q.Id == user_nameIdentifier),
                    CreatedByUserId = claim.Value,
                    FinalOrderTotal = finalOrderTotal,
                    PhoneNumber = productUserViewModel.ApplicationUser.PhoneNumber,
                    StreetAddress = productUserViewModel.ApplicationUser.StreetAddress,
                    City = productUserViewModel.ApplicationUser.City,
                    State = productUserViewModel.ApplicationUser.State,
                    PostalCode = productUserViewModel.ApplicationUser.PostalCode,
                    FullName = productUserViewModel.ApplicationUser.FullName,
                    Email = productUserViewModel.ApplicationUser.Email,
                    OrderDate = DateTime.Now,
                    OrderStatus = WC.StatusPending,
                };

                salesTableRepository.Add(salesTable);
                salesTableRepository.Save();

                foreach (var i in productUserViewModel.ProductList)
                {
                    SalesLine salesLine = new SalesLine()
                    {
                        SalesTable = salesTable,
                        ProductId = i.Id,
                        Qty = i.TempQty,
                        Price = i.Price,
                    };
                    salesLineRepository.Add(salesLine);
                }
                salesLineRepository.Save();



                string nonceFromTheClient = collection["payment_method_nonce"];

                var request = new TransactionRequest
                {
                    Amount = Convert.ToDecimal(finalOrderTotal),
                    PaymentMethodNonce = nonceFromTheClient,
                    OrderId = salesTable.Id.ToString(),
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true
                    }
                };

                var gateway = brainTreeGate.GetGateway();
                Result<Transaction> result = gateway.Transaction.Sale(request);

                if (result.Target != null)
                {
                    if (result.Target.ProcessorResponseText == "Approved")
                    {
                        salesTable.TransactionId = result.Target.Id;
                        salesTable.OrderStatus = WC.StatusApproved;
                    }
                    else
                    {
                        salesTable.OrderStatus = WC.StatusCancelled;
                    }
                }
                else
                {
                    salesTable.TransactionId = result.Message;
                    salesTable.OrderStatus = WC.StatusCancelled;
                }
                
                salesTableRepository.Save();

                return RedirectToAction(nameof(OrderConfirmation), new { id = salesTable.Id });
            }
            else
            {

                var htmlTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                    + "templates" + Path.DirectorySeparatorChar.ToString() + "OrderConfirmation.html";

                var subject = "Новый заказ";
                var htmlBody = "";

                using (StreamReader streamReader = new StreamReader(htmlTemplate))
                {
                    htmlBody = streamReader.ReadToEnd();
                }
                ;
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

                OrderTable orderTable = new OrderTable()
                {
                    ApplicationUserId = claim.Value,
                    OrderDate = DateTime.Now,
                    PhoneNumber = productUserViewModel.ApplicationUser.PhoneNumber,
                    FullName = productUserViewModel.ApplicationUser.FullName,
                    Email = productUserViewModel.ApplicationUser.Email,
                };

                orderTableRepository.Add(orderTable);
                orderTableRepository.Save();

                foreach (var item in productUserViewModel.ProductList)
                {
                    OrderLine orderLine = new OrderLine()
                    {
                        OrderId = orderTable.Id,
                        ProductId = item.Id
                    };
                    orderLineRepository.Add(orderLine);
                }
                orderLineRepository.Save();
            }

            return RedirectToAction(nameof(OrderConfirmation));
        }   

        public IActionResult OrderConfirmation(int id=0)
        {
            SalesTable salesTable = salesTableRepository.Find(id);

            TempData[WC.Success] = "Операция выполнена успешно!";
            HttpContext.Session.Clear();

            return View(salesTable);
        }   


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCart(IEnumerable<ProductModel> products)
        {
            var shoppingCart = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart);

            foreach (var i in products)
            {
                var s = shoppingCart.Where(c => c.ProductId == i.Id).FirstOrDefault();
                s.Qty = i.TempQty;
            }

            HttpContext.Session.Set(WC.SessionCart, shoppingCart); 

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Clear()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(nameof(Index), "Home");
        }

    
    }
}
