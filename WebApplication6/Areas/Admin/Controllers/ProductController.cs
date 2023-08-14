using Bulkybook.DataAccess;
using Bulkybook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop.Implementation;
using System.Security.Policy;

namespace BulkyApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfwork _unitofwrok;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfwork db, IWebHostEnvironment hostEnvironment)

        {
            _unitofwrok = db;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            //    IEnumerable<Product> dbvalues = _unitofwrok.Categories;
            return View();
        }
        public IActionResult Search()
        {
            IEnumerable<Product> dbvalues = _unitofwrok.Product.GetAll();
            return View(dbvalues);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid)
            {

                _unitofwrok.Product.Add(obj);
                _unitofwrok.save();
                TempData["Sucess"] = "Data Saved Sucessfully";
                return RedirectToAction("Search");
            }
            return View(obj);
        }


        public IActionResult Upsert(int? Id)
        {
            ProductVM productVM = new ()
            {
                product = new(),
                CategoryList = _unitofwrok.Category.GetAll().Select(
                x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                CoverTypelist = _unitofwrok.CoverType.GetAll().Select(
                x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.ID.ToString()
                }),


        };

            if (Id == null || Id==0)
            {
               // ViewBag.categorylist = CategoryList;
                return View(productVM);
            }

            else
            {
                productVM.product = _unitofwrok.Product.GetFirstOrDefault(x => x.Id == Id);
                return View(productVM);
            }
            
        }
        
        


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _hostEnvironment.WebRootPath;
                if (file !=null)
                { 
                   string filename =  Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwrootPath, @"Images\Product");
                    var extension = Path.GetExtension(file.FileName);

                //C: \Users\HHH\source\repos\WebApplication6\WebApplication6\wwwroot\Images\Product\98815631 - 8aaa - 4642 - 9cc2 - 87f4c265e6f4.jfif
                    if (obj.product.ImageUrl is not null)
                    {
                        var oldpath = Path.Combine(wwwrootPath, obj.product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldpath))
                        {
                            System.IO.File.Delete(oldpath);
                        }
                    }


                    using (var filestreams = new FileStream(Path.Combine(uploads,filename+extension),FileMode.Create)  )
                        {
                        file.CopyTo(filestreams);
                    }
                    obj.product.ImageUrl = @"\Images\Product\" + filename + extension;
                }
                if (obj.product.Id ==0)
                {
                    _unitofwrok.Product.Add(obj.product);
                }
                else
                {
                    _unitofwrok.Product.Update(obj.product);
                }
                //_unitofwrok.Product.Add(obj.product);

                _unitofwrok.save();
                //TempData["Sucess"] = "Data Saved Sucessfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _unitofwrok.Product.GetAll();
            return Json(new { data = products });
        }
        [HttpDelete]
        public IActionResult  Delete(int? Id)
        {
            ProductVM productVM = new();


            string wwwrootPath = _hostEnvironment.WebRootPath;
            if (Id != null || Id != 0)
            {
                productVM.product = _unitofwrok.Product.GetFirstOrDefault(x => x.Id == Id);
                if (productVM.product.ImageUrl is not null)
                {
                    var oldpath = Path.Combine(wwwrootPath, productVM.product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldpath))
                    {
                        System.IO.File.Delete(oldpath);
                    }
                }

                _unitofwrok.Product.Remove(productVM.product);
                _unitofwrok.save();
                return Json(new { success= true, message = "Data Deleted Sucessfully" });
                //return View(productVM);
            }
            return Json(new { success = true, message = "Data Deleted Sucessfully" });


        }

    }
}
