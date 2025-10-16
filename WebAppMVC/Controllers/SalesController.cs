using Braintree;
using Microsoft.AspNetCore.Mvc;
using WebApp_DataAccess.Repository;
using WebApp_DataAccess.Repository.IRepository;
using WebApp_Models;
using WebApp_Models.ViewModels;
using WebApp_Utility.BrainTree;
using WebAppMVC_Utility;

namespace WebAppMVC.Controllers
{
    public class SalesController : Controller
    {
        private readonly ISalesTableRepository salesTableRepository;
        private readonly ISalesLineRepository salesLineRepository;
        private readonly IBrainTreeGate brainTreeGate;

        [BindProperty]
        public SalesLineListVM SalesLineListVM { get; set; }

        public SalesController(ISalesTableRepository _salesTableRepository, ISalesLineRepository _salesLineRepository,
            IBrainTreeGate _brainTreeGate)
        {
            salesTableRepository = _salesTableRepository;
            salesLineRepository = _salesLineRepository;
            brainTreeGate = _brainTreeGate;
        }

        public IActionResult Index(string searchName = null, string searchEmail = null, string searchPhone = null, string Status = null)
        {
            SalesTableListVM salesTableListVM = new SalesTableListVM();

            salesTableListVM.SalesTable = salesTableRepository.GetAll
                (
                    filter: f => (
                        (searchName == null || f.FullName.Contains(searchName))
                     && (searchEmail == null || f.Email.Contains(searchEmail))
                     && (searchPhone == null || f.PhoneNumber.Contains(searchPhone))
                     && (Status == null || f.OrderStatus.Contains(Status))
                                ));
            salesTableListVM.StatusList = WC.listStatus.ToList().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = i,
                Value = i
            });

            return View(salesTableListVM);
        }


        public IActionResult Details(int id)
        {
            SalesLineListVM = new SalesLineListVM()
            {
                SalesTable = salesTableRepository.Find(id),
                SalesLine = salesLineRepository.GetAll(filter: x => x.SalesId == id, includeProperties: "Product"),
            };

            return View(SalesLineListVM);
        }



        [HttpPost]
        public IActionResult StartProcessing()
        {
            SalesTable salesTable = salesTableRepository.Find(this.SalesLineListVM.SalesTable.Id);
            salesTable.OrderStatus = WC.StatusInProcess;
            salesTableRepository.Save();
            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult ShipOrder()
        {
            SalesTable salesTable = salesTableRepository.Find(this.SalesLineListVM.SalesTable.Id);
            salesTable.OrderStatus = WC.StatusShipped;
            salesTable.ShippingDate = DateTime.Now;
            salesTableRepository.Save();
            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult CancelOrder()
        {
            SalesTable salesTable = salesTableRepository.Find(this.SalesLineListVM.SalesTable.Id);
            salesTable.OrderStatus = WC.StatusCancelled;
            salesTable.ShippingDate = DateTime.Now;
            salesTableRepository.Save();


            var request = new TransactionRequest.
            {
                Amount = Convert.ToDecimal(salesTable.FinalOrderTotal),
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
            }
            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult UpdateOrderDetails(int id)
        {


            return View();
        }



        #region API CALLS
        [HttpGet]
        public IActionResult GetOrdersList()
        {
            var ot = salesTableRepository.GetAll();
            JsonResult o = Json(ot);

            var ret = Json(new { data = ot });
            return ret;
        }
        #endregion
    }
}
