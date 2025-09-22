using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebAppMVC.Data;
using WebAppMVC.Models;

namespace WebAppMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<ProductModel> objList = _db.Product;

            foreach (ProductModel obj in objList)
            {
                obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
            }

            return View(objList);
        }
        
//---------------------------------------------------------------------------------------------------

        //GET - для UPDATEORINSERT
        public IActionResult UpdateOrInsert(int? id)
        {
            IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            });
            ViewBag.CategoryDropDown = CategoryDropDown;
            var list = CategoryDropDown.ToList();
            ViewBag.CategoryDropDown_List = list;
            ViewData["CategoryDropDown"] = CategoryDropDown;
            //TempData["CategoryDropDown"] = CategoryDropDown;


            ProductModel productModel = new ProductModel();
            if (id == null)
            { //do CREATE
                return View(productModel);
            }
            else
            { //do UPDATE
                productModel = _db.Product.Find(id);
                if (productModel == null)
                {
                    return NotFound();
                }

                return View(productModel);
            }
        }

        //POST - для UPDATEORINSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrInsert(ProductModel obj)
        {
            if (ModelState.IsValid)
            {
                _db.Product.Add(obj);
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

            var obj = _db.Category.Find(id);
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
            var obj = _db.Category.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Category.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }



    }
}
