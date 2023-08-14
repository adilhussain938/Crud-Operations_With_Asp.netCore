using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BulkyBook.Models;
using Bulkybook.DataAccess.Repository.IRepository;
using BulkyBook.Models.ViewModels;

namespace BulkyApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfwork _unitOfwork;

        public HomeController(ILogger<HomeController> logger,IUnitOfwork unitOfWork)
        {
            _logger = logger;
            _unitOfwork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productlist = _unitOfwork.Product.GetAll();
            return View(productlist);
        }
        public IActionResult Details(int id)
        {
            ShoppingCart cartobj = new()
            {
                count = 1,
                product = _unitOfwork.Product.GetFirstOrDefault(u => u.Id == id)
        };
                return View(cartobj);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}