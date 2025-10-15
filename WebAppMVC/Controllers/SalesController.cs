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


        public SalesController(ISalesTableRepository _salesTableRepository, ISalesLineRepository _salesLineRepository,
            IBrainTreeGate _brainTreeGate)
        {
            salesTableRepository = _salesTableRepository;
            salesLineRepository = _salesLineRepository;
            brainTreeGate = _brainTreeGate;
        }

        public IActionResult Index(string searchName=null, string searchEmail=null, string searchPhone=null, string Status=null)
        {
            SalesListVM salesListVM = new SalesListVM();

            salesListVM.SalesTable = salesTableRepository.GetAll
                (
                    filter: f =>   ( searchName  == null || f.FullName.Contains(searchName) )
                                && ( searchEmail == null || f.Email.Contains(searchEmail) )
                                && ( searchPhone == null || f.PhoneNumber.Contains(searchPhone) )
                                && ( Status      == null || f.OrderStatus.Contains(Status) )
                );
            salesListVM.StatusList = WC.listStatus.ToList().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = i,
                Value = i
            });

            return View(salesListVM);
        }


        public IActionResult Details(int id)
        {
            IEnumerable<SalesLine> salesLines;

            salesLines = salesLineRepository.GetAll(filter: x => x.Id == id, includeProperties:"Product");

            return View(salesLines);
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
