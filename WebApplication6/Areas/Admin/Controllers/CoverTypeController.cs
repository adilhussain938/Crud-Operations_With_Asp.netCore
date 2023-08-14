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
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfwork _unitofwrok;

        public CoverTypeController(IUnitOfwork db)

        {
            _unitofwrok = db;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> dbvalues = _unitofwrok.CoverType.GetAll();
            return View(dbvalues);
        }
        public IActionResult Create()
        {
            //    IEnumerable<CoverType> dbvalues = _unitofwrok.Categories;
            return View();
        }
        public IActionResult Search()
        {
            IEnumerable<CoverType> dbvalues = _unitofwrok.CoverType.GetAll();
            return View(dbvalues);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            if (ModelState.IsValid)
            {

                _unitofwrok.CoverType.Add(obj);
                _unitofwrok.save();
                TempData["Sucess"] = "Data Saved Sucessfully";
                return RedirectToAction("Search");
            }
            return View(obj);
        }


        public IActionResult Edit(int Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var readfromdb = _unitofwrok.CoverType.GetFirstOrDefault(x => x.ID == Id);
            if (readfromdb == null)
            {
                return NotFound();
            }

            return View(readfromdb);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
            if (ModelState.IsValid)
            {

                _unitofwrok.CoverType.Update(obj);

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

            var readfromdb = _unitofwrok.CoverType.GetFirstOrDefault(x => x.ID == Id);
            if (readfromdb != null)
                _unitofwrok.CoverType.Remove(readfromdb);
            TempData["Sucess"] = "Data Deleted Sucessfully";
            _unitofwrok.save();


            return RedirectToAction("Search");
        }



    }
}
