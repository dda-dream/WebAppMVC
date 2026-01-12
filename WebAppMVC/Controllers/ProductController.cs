using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using WebApp_DataAccess.Data;
using WebApp_DataAccess.Repository;
using WebApp_DataAccess.Repository.DTO;
using WebApp_DataAccess.Repository.IRepository;
using WebAppMVC_Models;
using WebAppMVC_Models.ViewModels;
using WebAppMVC_Utility;
using static System.Net.WebRequestMethods;


namespace WebAppMVC.Controllers
{ 
    [Authorize(Roles=WC.AdminRole)]
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
        }
//---------------------------------------------------------------------------------------------------
        public IActionResult Index()
        {            
            
            IEnumerable<ProductModel> products = productRepository.GetAll(includeProperties:"Category");
            
            return View(products);
        }




        public IActionResult ProductSelector()
        {
            List<ProductModel> allProducts = productRepository.GetAll().ToList();

            return View(allProducts);
        }


        [HttpGet]
        public JsonResult ProductSelector_GetData(string searchPattern)
        {
            var allProducts = productRepository.GetProductListByPattern(searchPattern);
            var result = allProducts.ToJson();
            
            Console.Write(result.ToString());
            
            return Json(allProducts);
        }



        //---------------------------------------------------------------------------------------------------

        //GET - для UPDATEORINSERT
        public IActionResult UpdateOrInsert(int? id)
        {
            ProductViewModel productViewModel = new ProductViewModel();
            productViewModel.Product = new ProductModel();
            productViewModel.CategorySelectList = productRepository.GetCategoryDropDownList();
            

            if (id == null)
            { //do CREATE
                return View(productViewModel);
            }
            else
            { //do UPDATE
                productViewModel.Product = productRepository.Find(id.GetValueOrDefault());
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
                { //Creating
                    string uploadPath = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string fileExt = Path.GetExtension(files[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName + fileExt), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productViewModel.Product.Image = fileName + fileExt;

                    productRepository.Add(productViewModel.Product);
                }
                else
                { // Updating                    
                    var productModel = productRepository.Find(productViewModel.Product.Id);

                    productModel.Name = productViewModel.Product.Name;
                    productModel.Price = productViewModel.Product.Price;
                    productModel.Description = productViewModel.Product.Description;
                    productModel.CategoryId = productViewModel.Product.CategoryId;
                    productModel.ShortDesc = productViewModel.Product.ShortDesc;

                    if (files.Count > 0)
                    {
                        string uploadPath = webRootPath + WC.ImagePath;
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

                    productRepository.Update(productModel);
                }
                productRepository.Save();
                return RedirectToAction("Index");
            }
            productViewModel.CategorySelectList = productRepository.GetCategoryDropDownList();

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


            var product = productRepository.FirstOrDefault(u => u.Id == id, includeProperties: "Category");

            if (product == null) 
            { 
                return NotFound();
            }

            var productViewModel= new ProductViewModel();
            productViewModel.Product = product;

            return View(productViewModel);
        }

        //POST - для DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var product = productRepository.Find(id.GetValueOrDefault());
            if (product == null)
            {
                return NotFound();
            }

            string webRootPath = _webHostEnvironment.WebRootPath;
            string uploadPath = webRootPath + WC.ImagePath;
            var oldFile = Path.Combine(uploadPath, product.Image);

            if (System.IO.File.Exists(oldFile))
                System.IO.File.Delete(oldFile);

            productRepository.Remove(product);
            productRepository.Save();

            return RedirectToAction("Index");
        }



    }
}
