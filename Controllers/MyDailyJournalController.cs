using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Data;
using WebAppMVC.Models;

namespace WebAppMVC.Controllers
{
    public class MyDailyJournalController : Controller
    {
        private readonly ApplicationDbContext _db;
        public MyDailyJournalController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<MyDailyJournal> objList = _db.MyDailyJournal;
            return View(objList);
        }
        
        //GET - для CREATE
        public IActionResult Create()
        {
            return View();
        }

        //POST - для CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MyDailyJournal obj)
        {
            _db.MyDailyJournal.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
