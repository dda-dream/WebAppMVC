using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using WebApp_DataAccess.Data;
using WebApp_DataAccess.Repository;
using WebApp_DataAccess.Repository.IRepository;
using WebAppMVC_Models;
using WebAppMVC_Utility;

namespace WebAppMVC.Controllers
{
    public class MyDailyJournalController : Controller
    {
        int forTestTransientOrScooped = 0;
        private readonly IMyDailyJournalRepository repository;
        private readonly IMyDailyJournalRepository repository_forTest_LifeTime;


        public MyDailyJournalController( IMyDailyJournalRepository repository)
        {
            this.repository = repository;
        }   


//---------------------------------------------------------------------------------------------------
        public IActionResult Index()
        {
            forTestTransientOrScooped++;
            repository.DoIncrement_forTestTransientOrScooped();

            

            IEnumerable<MyDailyJournalModel> objList = repository.GetAll();
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
                repository.Add(obj);
                repository.Save();
                
                TempData[WC.Success] = "Запись создана!";
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

            var obj = repository.Find(id.GetValueOrDefault());
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
                repository.Update(obj);
                repository.Save();

                TempData[WC.Success] = "Запись изменена!";
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

            var obj = repository.Find(id.GetValueOrDefault());
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
            var obj = repository.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            repository.Remove(obj);
            repository.Save();

            TempData[WC.Success] = "Запись удалена!";
            return RedirectToAction("Index");
        }

//---------------------------------------------------------------------------------------------------

        public class DataToView
        {
            public MyDailyJournalModel _MyDailyJournalModel;
            public IEnumerable<LogTableModel> _LogTableModelEnumerator;
        }
        //GET - для ShowMoreInfo
        public IActionResult ShowMoreInfo(int? id)
        {
            var d = new DataToView();

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = repository.Find(id.GetValueOrDefault());
            if (obj == null) 
            { 
                return NotFound();
            }

            var LogTableList = repository.GetLogForId(id);

            d._MyDailyJournalModel = obj;
            d._LogTableModelEnumerator = LogTableList;

            return View(d);
        }


    }
}
