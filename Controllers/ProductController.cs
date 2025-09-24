using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using WebAppMVC.Data;
using WebAppMVC.Models;
using WebAppMVC.Models.ViewModels;
using static System.Net.WebRequestMethods;

namespace WebAppMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
//---------------------------------------------------------------------------------------------------
        public IActionResult Index()
        {            
            IEnumerable<ProductModel> products = _db.Product;

            foreach (ProductModel obj in products)
            {
                obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
            }
            
            return View(products);
        }
        
//---------------------------------------------------------------------------------------------------

        //GET - для UPDATEORINSERT
        public IActionResult UpdateOrInsert(int? id)
        {
            /*
            IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            });
            ViewBag.CategoryDropDown = CategoryDropDown;
            var list = CategoryDropDown.ToList();
            ViewBag.CategoryDropDown_List = list;
            ViewBag.CategoryDropDown_List1 = CategoryDropDown.ToList();

            ViewData["CategoryDropDown"] = CategoryDropDown;
            TempData["CategoryDropDown"] = CategoryDropDown;
            */

            //ProductModel productModel = new ProductModel();
            ProductViewModel productViewModel = new ProductViewModel();
            productViewModel.Product = new ProductModel();
            productViewModel.CategorySelectList = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            });

            if (id == null)
            { //do CREATE
                return View(productViewModel);
            }
            else
            { //do UPDATE
                productViewModel.Product = _db.Product.Find(id);
                if (productViewModel.Product == null)
                {
                    return NotFound();
                }

                return View(productViewModel);
            }
        }

        //POST - для UPDATEORINSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrInsert(ProductViewModel productViewModel)
        {
            ModelState.Remove("CategorySelectList");
            ModelState.Remove("Product.Image");
            ModelState.Remove("Product.Category");

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productViewModel.Product.Id == 0)
                { //Create
                    string uploadPath = webRootPath + WebConstants.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string fileExt = Path.GetExtension(files[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName + fileExt), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productViewModel.Product.Image = fileName + fileExt;

                    _db.Product.Add(productViewModel.Product);
                }
                else
                { // Update                    
                    var productModel = _db.Product.Find(productViewModel.Product.Id);

                    productModel. Name = productViewModel.Product.Name;
                    productModel.Price = productViewModel.Product.Price;
                    productModel.Description = productViewModel.Product.Description;
                    productModel.CategoryId = productViewModel.Product.CategoryId;

                    if (files.Count > 0)
                    {
                        string uploadPath = webRootPath + WebConstants.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string fileExt = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(uploadPath, productModel.Image);

                        if (System.IO.File.Exists(oldFile))
                            System.IO.File.Delete(oldFile);

                        using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName + fileExt), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productModel.Image = fileName + fileExt;
                    }

                    _db.Product.Update(productModel);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            productViewModel.CategorySelectList = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            });

            return View(productViewModel);   
        }

//---------------------------------------------------------------------------------------------------

        //GET - для DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }


            var product = _db.Product.Find(id);
            if (product == null) 
            { 
                return NotFound();
            }

            product.Category = _db.Category.FirstOrDefault(u => u.Id == product.CategoryId);

            var productViewModel= new ProductViewModel();
            productViewModel.Product = product;

            return View(productViewModel);
        }

        //POST - для DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var product = _db.Product.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            string webRootPath = _webHostEnvironment.WebRootPath;
            string uploadPath = webRootPath + WebConstants.ImagePath;
            var oldFile = Path.Combine(uploadPath, product.Image);

            if (System.IO.File.Exists(oldFile))
                System.IO.File.Delete(oldFile);

            _db.Product.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }



    }
}
