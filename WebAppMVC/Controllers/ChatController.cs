using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WebApp_DataAccess.Repository;
using WebApp_DataAccess.Repository.IRepository;
using WebAppMVC_Models;

namespace WebAppMVC.Controllers
{

    [Authorize]
    public class ChatController : Controller
    {
        readonly IApplicationUserRepository applicationUserRepository;
        readonly IChatRepository chatRepository;

        public ChatController(IApplicationUserRepository applicationUserRepository, IChatRepository chatRepository)
        {
            this.applicationUserRepository = applicationUserRepository;
            this.chatRepository = chatRepository;
        }

        public IActionResult Index()
        {
            var userClaim = User.Claims.FirstOrDefault();
            var user = applicationUserRepository.FirstOrDefault(q => q.Id == userClaim.Value);


            return View(user);
        }

        public IActionResult Chat_New()
        {
            var userClaim = User.Claims.FirstOrDefault();
            var user = applicationUserRepository.FirstOrDefault(q => q.Id == userClaim.Value);
            

            return View(user);
        }



    }
}
