using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.DTO;
using CinemaApp.Repository.Interface;
using CinemaApp.Service.Interface;
using System;
using System.Linq;

namespace CinemaApp.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly ITicketService _ticketService;
        private readonly IEmailService _emailService;

        public ShoppingCartService(IUserRepository userRepository, IRepository<ShoppingCart> shoppingCartRepository, IRepository<Order> orderRepository, ITicketService ticketService, IEmailService emailService)
        {
            _userRepository = userRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _orderRepository = orderRepository;
            _ticketService = ticketService;
            _emailService = emailService;
        }

        public Order CreateOrder(Guid Id)
        {

            var UserCart = _shoppingCartRepository.Get(Id);
            var loggedInUser = _userRepository.Get(UserCart.OwnerId);
            Order newOrder = new Order
            {
                Id = Guid.NewGuid(),
                Owner = loggedInUser,
                OwnerId = loggedInUser.Id,
                ShoppingCart = UserCart,
                ShoppingCartId = UserCart.Id,
                OrderPlaced = DateTime.Now
            };
            _orderRepository.Insert(newOrder);
            UserCart.Owner = null;
            UserCart.OwnerId = null;
            UserCart.OrderId = newOrder.Id;
            UserCart.Order = newOrder;
            _shoppingCartRepository.Update(UserCart);
            ShoppingCart newShoppingCart = new ShoppingCart
            {
                Id = Guid.NewGuid(),
                Owner = loggedInUser,
                OwnerId = loggedInUser.Id
            };
            _shoppingCartRepository.Insert(newShoppingCart);
            _emailService.SendEmailAsync(new EmailMessage
            {
                MailTo = loggedInUser.Email,
                Subject = "Order created",
                Content = "Your order has been created successfully!",
            });
            return newOrder;
        }

        public ShoppingCart GetShoppingCart(Guid Id)
        {
            return _shoppingCartRepository.Get(Id);
        }

        public ShoppingCartDTO GetShoppingCartInfo(string userId)
        {
            var loggedInUser = _userRepository.Get(userId);
            var userShoppingCart = _shoppingCartRepository.GetAll().Where(z => z.OwnerId == loggedInUser.Id).FirstOrDefault();
            var tickets = _ticketService.GetAllTickets().Where(z => z.ShoppingCartId == userShoppingCart.Id).ToList();
            var TotalPrice = _ticketService.GetAllTickets().Where(z => z.ShoppingCartId == userShoppingCart.Id).Sum(z => z.Price * z.Quantity);
            return new ShoppingCartDTO
            {
                Id = userShoppingCart.Id,
                OwnerId = userId,
                OwnerName = loggedInUser.FirstName + " " + loggedInUser.LastName,
                Tickets = tickets,
                TotalPrice = TotalPrice
            };
        }
    }
}
