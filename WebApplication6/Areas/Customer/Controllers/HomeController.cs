using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BulkyBook.Models;
using Bulkybook.DataAccess.Repository.IRepository;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Net.WebSockets;

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


        
        public IActionResult Details(int productid)
        {
            ShoppingCart cartobj = new()
            {
                count = 1,
                ProductId= productid,
                product = _unitOfwork.Product.GetFirstOrDefault(u => u.Id == productid)
        };
                return View(cartobj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var Claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = Claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            var updateCart = _unitOfwork.ShopingCartRepository.GetFirstOrDefault(x => x.ProductId == shoppingCart.ProductId && x.ApplicationUserId == claim.Value);
            if (updateCart == null)
            {
                _unitOfwork.ShopingCartRepository.Add(shoppingCart);
                _unitOfwork.save();

            }
            else
            {
                _unitOfwork.ShopingCartRepository.incrementshoppingcart(updateCart, shoppingCart.count);

            }

            _unitOfwork.save();
            
            return RedirectToAction(nameof(Index));

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