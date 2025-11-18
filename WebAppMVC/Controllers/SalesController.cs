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

        public IActionResult Index(string? searchName = null, string? searchEmail = null, string? searchPhone = null, string? Status = null)
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

            TempData[WC.Success] = "Заказ находится в процессе.";

            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult ShipOrder()
        {
            SalesTable salesTable = salesTableRepository.Find(this.SalesLineListVM.SalesTable.Id);
            salesTable.OrderStatus = WC.StatusShipped;
            salesTable.ShippingDate = DateTime.Now;
            salesTableRepository.Save();

            TempData[WC.Success] = "Заказ успешно отправлен.";

            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult CancelOrder(IFormCollection collection)
        {
            SalesTable salesTable = salesTableRepository.Find(this.SalesLineListVM.SalesTable.Id);

           
            var gateway = brainTreeGate.GetGateway();
            Transaction transaction = gateway.Transaction.Find(salesTable.TransactionId);

            if (transaction.Status == TransactionStatus.AUTHORIZED || transaction.Status == TransactionStatus.SUBMITTED_FOR_SETTLEMENT)
            {//не нужно возврашать деньги                
                Result<Transaction> resultVoid = gateway.Transaction.Void(salesTable.TransactionId);
            } 
            else
            {//нужно вернуть
                Result<Transaction> resultRefund = gateway.Transaction.Refund(salesTable.TransactionId);                
            }

            salesTable.OrderStatus = WC.StatusRefunded;
            salesTableRepository.Save();

            TempData[WC.Success] = "Оплата по заказу возвращена.";

            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult UpdateOrderDetails()
        {
            SalesTable salesTable = salesTableRepository.Find(this.SalesLineListVM.SalesTable.Id);

            salesTable.FullName = SalesLineListVM.SalesTable.FullName;
            salesTable.PhoneNumber = SalesLineListVM.SalesTable.PhoneNumber;
            salesTable.StreetAddress = SalesLineListVM.SalesTable.StreetAddress;
            salesTable.City = SalesLineListVM.SalesTable.City;
            salesTable.State = SalesLineListVM.SalesTable.State;
            salesTable.PostalCode = SalesLineListVM.SalesTable.PostalCode;
            salesTable.Email = SalesLineListVM.SalesTable.Email;

            salesTableRepository.Update(salesTable);
            salesTableRepository.Save();

            TempData[WC.Success] = "Заказ успешно обновлен.";

            return RedirectToAction("Details", "Sales", new { id = SalesLineListVM.SalesTable.Id } );
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
