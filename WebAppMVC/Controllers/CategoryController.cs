using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WebApp_DataAccess.Data;
using WebApp_DataAccess.Repository.IRepository;
using WebAppMVC_Models;
using WebAppMVC_Utility;

namespace WebAppMVC.Controllers
{
    [Authorize(Roles=WC.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            IEnumerable<CategoryModel> objList = categoryRepository.GetAll();
            return View(objList);
        }

//---------------------------------------------------------------------------------------------------
        
        //GET - для CREATE
        public IActionResult Create()
        {
            return View();
        }

        //POST - для CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryModel obj)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Add(obj);
                categoryRepository.Save();
                TempData[WC.Success] = "Операция выполнена успешно!";
                return RedirectToAction("Index");
            }

            TempData[WC.Error] = "Ошибка создания категории!";
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

            var obj = categoryRepository.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST - для EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryModel obj)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Update(obj);
                categoryRepository.Save();
                TempData[WC.Success] = "Операция выполнена успешно!";
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

            var obj = categoryRepository.Find(id.GetValueOrDefault());
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
            var obj = categoryRepository.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            categoryRepository.Remove(obj);
            categoryRepository.Save();
            TempData[WC.Success] = "Операция выполнена успешно!";
            return RedirectToAction("Index");
        }

//---------------------------------------------------------------------------------------------------


    }
}
