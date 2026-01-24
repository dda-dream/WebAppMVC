using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAppMVC.Controllers
{
    public class BoomSosedController : Controller
    {
        

        public IActionResult Index()
        {
            return View();
        }


    }
}
