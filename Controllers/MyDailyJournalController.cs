using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            IEnumerable<MyDailyJournalModel> objList = _db.MyDailyJournal;
            return View(objList);
        }
        
//---------------------------------------------------------------------------------------------------

        //GET - для CREATE
        public IActionResult Create()
        {
            var model = new MyDailyJournalModel
            {
                LogDate = DateTime.Now
            };
            return View(model);
        }

        //POST - для CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MyDailyJournalModel obj)
        {
            if (ModelState.IsValid)
            {
                var transactionId = _db.Database.BeginTransaction();

                obj.CreatedDateTime = DateTime.Now;
                obj.ModifiedDateTime = DateTime.Now;
                var newRecord = _db.MyDailyJournal.Add(obj);                
                _db.SaveChanges();

                LogTableModel logTable = new LogTableModel();
                logTable.CreatedDateTime = DateTime.Now;
                logTable.TypeStr = "insert";
                logTable.LogTableName = "MyDailyJournalModel";
                logTable.LogRecordId = newRecord.Entity.Id;
                logTable.Message = $"MyDailyJournalController.cs Record with ID:{newRecord.Entity.Id}  Text:{newRecord.Entity.Text}";
                _db.Add(logTable);
                _db.SaveChanges();

                transactionId.Commit();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

//---------------------------------------------------------------------------------------------------

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
        public IActionResult Edit(MyDailyJournalModel obj)
        {
            if (ModelState.IsValid)
            {
                var objPrev = _db.MyDailyJournal.AsNoTracking().FirstOrDefault(_ => _.Id == obj.Id);
                obj.CreatedDateTime = objPrev.CreatedDateTime;
                obj.ModifiedDateTime = DateTime.Now;
                _db.MyDailyJournal.Update(obj);
                _db.SaveChanges();

                LogTableModel logTable = new LogTableModel();
                logTable.CreatedDateTime = DateTime.Now;
                logTable.TypeStr = "modify";
                logTable.LogTableName = "MyDailyJournalModel";
                logTable.LogRecordId = obj.Id;
                logTable.Message = $"Record with ID:{obj.Id}  Text:{obj.Text}";
                _db.Add(logTable);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(obj);   
        }

//---------------------------------------------------------------------------------------------------

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

            LogTableModel logTable = new LogTableModel();
            logTable.CreatedDateTime = DateTime.Now;
            logTable.TypeStr = "delete";
            logTable.LogTableName = "MyDailyJournalModel";
            logTable.Message = $"Record with ID:{obj.Id}  Text:{obj.Text}";
            logTable.LogRecordId = obj.Id;
            _db.Add(logTable);
            _db.SaveChanges();



            return RedirectToAction("Index");
        }

//---------------------------------------------------------------------------------------------------

        //GET - для ShowMoreInfo
        public IActionResult ShowMoreInfo(int? id)
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


    }
}
