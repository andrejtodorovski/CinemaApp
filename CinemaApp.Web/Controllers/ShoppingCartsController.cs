using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Domain.DomainModels;
using CinemaApp.Repository;
using CinemaApp.Service.Interface;
using System.Security.Claims;
using CinemaApp.Service.Implementation;
using Stripe;
using CinemaApp.Repository.Implementation;
using CinemaApp.Repository.Interface;

namespace CinemaApp.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ITicketService _ticketService;
        private readonly IUserRepository _userRepository;
        private readonly IOrderService _orderService;

        public ShoppingCartsController(ApplicationDbContext context, IShoppingCartService shoppingCartService, ITicketService ticketService, IUserRepository userRepository, IOrderService orderService)
        {
            _context = context;
            _shoppingCartService = shoppingCartService;
            _ticketService = ticketService;
            _userRepository = userRepository;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(_shoppingCartService.GetShoppingCartInfo(userId));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromCart(Guid Id)
        {
            _ticketService.DeleteTicket(Id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Order")]
        [ValidateAntiForgeryToken]
        public IActionResult OrderNow(Guid Id, string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);            
            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });
            var TotalPrice = _shoppingCartService.GetShoppingCartInfo(userId).TotalPrice;
            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(TotalPrice)*100),
                Description = "Application Payment",
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                _shoppingCartService.CreateOrder(Id);
                return RedirectToAction("MyOrders", "Orders");

            }

            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}
