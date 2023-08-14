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
    public class CompanyController : Controller
    {
        private readonly IUnitOfwork _unitofwrok;

        public CompanyController(IUnitOfwork db)

        {
            _unitofwrok = db;
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
            IEnumerable<CompanyModel> dbvalues = _unitofwrok.Company.GetAll();
            return View(dbvalues);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(CompanyModel obj)
        {
            if (ModelState.IsValid)
            {

                _unitofwrok.Company.Add(obj);
                _unitofwrok.save();
                TempData["Sucess"] = "Data Saved Sucessfully";
                return RedirectToAction("Search");
            }
            return View(obj);
        }


        public IActionResult Upsert(int? Id)
        {

            CompanyModel Company = new();
            if (Id == null || Id==0)
            {
               // ViewBag.categorylist = CategoryList;
                return View(Company);
            }

            else
            {
                 Company = _unitofwrok.Company.GetFirstOrDefault(x => x.Id == Id);
                return View(Company);
            }
            
        }
        
        


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CompanyModel obj)
        {
            if (ModelState.IsValid)
            {
                
                if (obj.Id ==0)
                {
                    _unitofwrok.Company.Add(obj);
                }
                else
                {
                    _unitofwrok.Company.Update(obj);
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
            var Comapany = _unitofwrok.Company.GetAll();
            return Json(new { data = Comapany });
        }
        [HttpDelete]
        public IActionResult  Delete(int? Id)
        {

            if (Id != null || Id != 0)
            {
                var a  = _unitofwrok.Company.GetFirstOrDefault(x => x.Id == Id);
          
                _unitofwrok.Company.Remove(a);
                _unitofwrok.save();
                return Json(new { success= true, message = "Data Deleted Sucessfully" });
                //return View(productVM);
            }
            return Json(new { success = true, message = "Data Deleted Sucessfully" });


        }

    }
}
