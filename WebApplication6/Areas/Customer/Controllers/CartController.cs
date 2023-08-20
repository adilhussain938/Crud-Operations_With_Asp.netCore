using Bulkybook.DataAccess.Repository;
using Bulkybook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Unicode;

namespace BulkyApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        public IUnitOfwork _unitOfWork;
        [BindProperty]
        public ShoppingCartVm ShoppingCartVm { get; set; }


        public CartController(IUnitOfwork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var Claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = Claimsidentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVm = new ShoppingCartVm();

            ShoppingCartVm.ListCart = _unitOfWork.ShopingCartRepository.GetAll(x => x.ApplicationUserId == claim.Value).ToList();
            ShoppingCartVm.ListCart?.ForEach(x => x.product = _unitOfWork.Product.GetFirstOrDefault(y => y.Id == x.ProductId));

            ShoppingCartVm.ListCart.ForEach(x =>

           ShoppingCartVm.Price += x.count * x.product.Price100);


            return View(ShoppingCartVm);

        }

        public IActionResult plus(int cartId)
        {
            var Claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = Claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            //Product.ApplicationUserId = claim.Value;

            var updateCart = _unitOfWork.ShopingCartRepository.GetFirstOrDefault(x => x.ProductId == cartId && x.ApplicationUserId == claim.Value);
            if (updateCart != null)
            {
                _unitOfWork.ShopingCartRepository.incrementshoppingcart(updateCart, 1);
                _unitOfWork.save();

            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult minus(int cartId)
        {
            var Claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = Claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            //Product.ApplicationUserId = claim.Value;

            var updateCart = _unitOfWork.ShopingCartRepository.GetFirstOrDefault(x => x.ProductId == cartId && x.ApplicationUserId == claim.Value);
            if (updateCart != null)
            {
                _unitOfWork.ShopingCartRepository.decrementshoppingcart(updateCart, 1);
                _unitOfWork.save();

            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult remove(int cartId)
        {
            var Claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = Claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            //Product.ApplicationUserId = claim.Value;

            var updateCart = _unitOfWork.ShopingCartRepository.GetFirstOrDefault(x => x.ProductId == cartId && x.ApplicationUserId == claim.Value);
            if (updateCart != null)
            {
                _unitOfWork.ShopingCartRepository.Remove(updateCart);
                _unitOfWork.save();

            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVm = new()
            {
                ListCart = (List<ShoppingCart>)_unitOfWork.ShopingCartRepository.GetAll(u => u.ApplicationUserId == userId),
                OrderHeader = new()
            };
            ShoppingCartVm.ListCart?.ForEach(x => x.product = _unitOfWork.Product.GetFirstOrDefault(y => y.Id == x.ProductId));


            ShoppingCartVm.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUserRepository.GetAll(u => u.Id == userId).FirstOrDefault();

            ShoppingCartVm.OrderHeader.Name = ShoppingCartVm.OrderHeader.ApplicationUser.Name;
            ShoppingCartVm.OrderHeader.PhoneNumber = ShoppingCartVm.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVm.OrderHeader.StreetAddress = ShoppingCartVm.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVm.OrderHeader.City = ShoppingCartVm.OrderHeader.ApplicationUser.City;
            ShoppingCartVm.OrderHeader.State = ShoppingCartVm.OrderHeader.ApplicationUser.State;
            ShoppingCartVm.OrderHeader.PostalCode = ShoppingCartVm.OrderHeader.ApplicationUser.PostalCode;



            foreach (var cart in ShoppingCartVm.ListCart)
            {
                //cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVm.OrderHeader.OrderTotal += (cart.product.ListPrice * cart.count);
            }
            return View(ShoppingCartVm);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVm.ListCart = _unitOfWork.ShopingCartRepository.GetAll(u => u.ApplicationUserId == userId).ToList();

			ShoppingCartVm.OrderHeader.OrderDate = System.DateTime.Now;
			ShoppingCartVm.OrderHeader.ApplicationUserId = userId;

            ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.GetAll(u => u.Id == userId).FirstOrDefault();


            foreach (var cart in ShoppingCartVm.ListCart)
            {
				//cart.Price = GetPriceBasedOnQuantity(cart);
				ShoppingCartVm.OrderHeader.OrderTotal += (cart.product.Price * cart.count);
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
				//it is a regular customer 
				ShoppingCartVm.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
				ShoppingCartVm.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
				//it is a company user
				ShoppingCartVm.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
				ShoppingCartVm.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            _unitOfWork.OrderHeader.Add(ShoppingCartVm.OrderHeader);
            _unitOfWork.save();
            foreach (var cart in ShoppingCartVm.ListCart)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderId = ShoppingCartVm.OrderHeader.Id,
                    Price = cart.product.Price,
                    Count = cart.count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.save();
            }
            return View();
        }
    }
}

