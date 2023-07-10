using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.DTO;
using CinemaApp.Repository.Interface;
using CinemaApp.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaApp.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITicketService _ticketService;


        public OrderService(IRepository<Order> orderRepository, IUserRepository userRepository, ITicketService ticketService)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _ticketService = ticketService;
        }
        public Order CreateNewOrder(Order o)
        {
            o.Id = Guid.NewGuid();
            return _orderRepository.Insert(o);

        }

        public Order DeleteOrder(Guid id)
        {
            return _orderRepository.Delete(_orderRepository.Get(id));
        }

        public List<OrderDTO> GetAllOrders()
        {
            List<Order> orders = this._orderRepository.GetAll().ToList();
            List<OrderDTO> result = new List<OrderDTO>();
            foreach(var o in orders) 
            {
                var tickets = _ticketService.GetAllTickets().Where(
                    z => z.ShoppingCartId == o.ShoppingCartId);
                var TotalPrice = 0;
                foreach(var t in tickets)
                {
                    TotalPrice += t.Quantity * t.Price;
                }
                var Owner = _userRepository.Get(o.OwnerId);
                result.Add(new OrderDTO
                {
                    Id = o.Id,
                    OwnerId = o.OwnerId,
                    OwnerName = Owner.FirstName + " " + Owner.LastName,
                    Tickets = tickets.ToList(),
                    TotalPrice = TotalPrice,
                    OrderPlaced = o.OrderPlaced
                });
            }
            return result;
        }
        public List<OrderDTO> GetAllOrdersByOwnerId(string userId)
        {
            List<Order> orders = this._orderRepository.GetAll().Where(
                z => z.OwnerId == userId).ToList();
            List<OrderDTO> result = new List<OrderDTO>();
            foreach (var o in orders)
            {
                var tickets = _ticketService.GetAllTickets().Where(
                    z => z.ShoppingCartId == o.ShoppingCartId);
                var TotalPrice = 0;
                foreach (var t in tickets)
                {
                    TotalPrice += t.Quantity * t.Price;
                }
                var Owner = _userRepository.Get(o.OwnerId);
                result.Add(new OrderDTO
                {
                    Id = o.Id,
                    OwnerId = o.OwnerId,
                    OwnerName = Owner.FirstName + " " + Owner.LastName,
                    Tickets = tickets.ToList(),
                    TotalPrice = TotalPrice,
                    OrderPlaced = o.OrderPlaced
                });
            }
            return result;
        }

        public OrderDTO GetDetailsForOrder(Guid? id)
        {
            var o = _orderRepository.Get(id);
            var tickets = _ticketService.GetAllTickets().Where(
                    z => z.ShoppingCartId == o.ShoppingCartId);
            var TotalPrice = 0;
            foreach (var t in tickets)
            {
                TotalPrice += t.Quantity * t.Price;
            }
            var Owner = _userRepository.Get(o.OwnerId);
            return new OrderDTO
            {
                Id = o.Id,
                OwnerId = o.OwnerId,
                OwnerName = Owner.FirstName + " " + Owner.LastName,
                Tickets = tickets.ToList(),
                TotalPrice = TotalPrice,
                OrderPlaced = o.OrderPlaced
            };
        }

        public Order GetOrder(Guid? id)
        {
            return _orderRepository.Get(id);
        }

        public Order UpdateExistingOrder(Order p)
        {
            return _orderRepository.Update(p);
        }
    }
}
