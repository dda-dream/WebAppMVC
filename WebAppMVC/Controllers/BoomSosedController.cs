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


        [HttpGet]
        public IActionResult Enabled()
        {
            int ret = Random.Shared.Next(0, 100);

            return Ok(ret);
        }


    }
}
