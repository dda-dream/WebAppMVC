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



        //GET - для EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.MyDailyJournal.Find(id);
            if (obj == null) 
            { 
                return NotFound();
            }

            return View(obj);
        }

        //POST - для EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MyDailyJournal obj)
        {
            if (ModelState.IsValid)
            {
                _db.MyDailyJournal.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);   
        }


        //GET - для DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.MyDailyJournal.Find(id);
            if (obj == null) 
            { 
                return NotFound();
            }

            return View(obj);
        }

        //POST - для DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.MyDailyJournal.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.MyDailyJournal.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
