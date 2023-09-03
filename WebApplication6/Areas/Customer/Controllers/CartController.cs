using Bulkybook.DataAccess.Repository;
using Bulkybook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
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

			ShoppingCartVm.ListCart?.ForEach(x => x.product = _unitOfWork.Product.GetFirstOrDefault(y => y.Id == x.ProductId));

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

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                //stripe settings 
                var domain = "https://localhost:7199/";
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVm.OrderHeader.Id}",
                    CancelUrl = domain + $"customer/cart/index",
                };

                foreach (var item in ShoppingCartVm.ListCart)
                {

                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.product.Price * 100),//20.00 -> 2000
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.product.Title
                            },

                        },
                        Quantity = item.count,
                    };
                    options.LineItems.Add(sessionLineItem);

                }

                var service = new SessionService();
                Session session = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVm.OrderHeader.Id, session.Id, session.PaymentIntentId);
                _unitOfWork.save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }

            else
            {
                return RedirectToAction("OrderConfirmation", "Cart", new { id = ShoppingCartVm.OrderHeader.Id });
            }

            //return View();
        }



        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id/*, includeProperties: "ApplicationUser"*/);
            if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                //check the stripe status
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentID(id, orderHeader.SessionId, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.save();
                }
            }
            //_emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email, "New Order - Bulky Book", "<p>New Order Created</p>");
            List<ShoppingCart> shoppingCarts = _unitOfWork.ShopingCartRepository.GetAll(u => u.ApplicationUserId ==
            orderHeader.ApplicationUserId).ToList();
            //HttpContext.Session.Clear();
            _unitOfWork.ShopingCartRepository.RemoveRange(shoppingCarts);
            _unitOfWork.save();
            return View(id);
        }
    }
}

