using Bulkybook.DataAccess;
using Bulkybook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

using MessagePack.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop.Implementation;

namespace BulkyApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfwork _unitofwrok;

        public CategoryController(IUnitOfwork db)

        {
            _unitofwrok = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> dbvalues = _unitofwrok.Category.GetAll();
            return View(dbvalues);
        }
        public IActionResult Create()
        {
            //    IEnumerable<Category> dbvalues = _unitofwrok.Categories;
            return View();
        }
        public IActionResult Search()
        {
            IEnumerable<Category> dbvalues = _unitofwrok.Category.GetAll();
            return View(dbvalues);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {

                _unitofwrok.Category.Add(obj);
                _unitofwrok.save();
                TempData["Sucess"] = "Data Saved Sucessfully";
                return RedirectToAction("Search");
            }
            return View(obj);
        }


        public IActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var readfromdb = _unitofwrok.Category.GetFirstOrDefault(x => x.Id == Id);
            if (readfromdb == null)
            {
                return NotFound();
            }

            return View(readfromdb);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {

                _unitofwrok.Category.Update(obj);

                _unitofwrok.save();
                TempData["Sucess"] = "Data Edited Sucessfully";
                return RedirectToAction("Search");
            }
            return View(obj);
        }

        public IActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var readfromdb = _unitofwrok.Category.GetFirstOrDefault(x => x.Id == Id);
            if (readfromdb != null)
                _unitofwrok.Category.Remove(readfromdb);
            TempData["Sucess"] = "Data Deleted Sucessfully";
            _unitofwrok.save();


            return RedirectToAction("Search");
        }



    }
}
