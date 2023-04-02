using MessagePack.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop.Implementation;
using WebApplication6.Models;
using WebApplication6.NewFolder1;

namespace BulkyApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DBContextsol _db;

        public CategoryController(DBContextsol db)

        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> dbvalues = _db.Categories;
            return View(dbvalues);
        }
        public IActionResult Create()
        {
        //    IEnumerable<Category> dbvalues = _db.Categories;
            return View();
        }
        public IActionResult Search()
        {
            IEnumerable<Category> dbvalues = _db.Categories;
                return View(dbvalues);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {

                _db.Categories.Add(obj);
                //IEnumerable<Category> dbvalues = _db.Categories;
                // if(dbvalues.Where(x=>x.Name == obj.Name).Count()>0)
                //{
                //    ModelState.AddModelError("name", "Name already exist");
                //}
                //else { 
                _db.SaveChanges();
                TempData["Sucess"] = "Data Saved Sucessfully";
                return RedirectToAction("Search");
            }
            return View(obj);
        }


        public IActionResult Edit(Guid? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var readfromdb = _db.Categories.Find(Id);
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

                _db.Categories.Update(obj);
                
                _db.SaveChanges();
                TempData["Sucess"] = "Data Edited Sucessfully";
                return RedirectToAction("Search");
            }
            return View(obj);
        }

        public IActionResult Delete(Guid? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var readfromdb = _db.Categories.Find(Id);
            if(readfromdb !=null)
                _db.Categories.Remove(readfromdb);
                TempData["Sucess"] = "Data Deleted Sucessfully";
                _db.SaveChanges();

                        
            return RedirectToAction("Search");
        }



    }
}
