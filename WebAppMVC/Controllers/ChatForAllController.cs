using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppMVC.Controllers
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Title"] = "SignalR Chat";
        }
    }



    public class ChatForAllController : Controller
    {
        public IActionResult Index()
        {
            var r = new IndexModel ();
            

            return View();
        }

        public IActionResult Chat_New()
        {
            var r = new IndexModel ();
            

            return View();
        }



    }
}
