using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.DTO;
using CinemaApp.Domain.Identity;
using CinemaApp.Repository.Interface;
using CinemaApp.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaApp.Service.Implementation
{
    public class TicketService : ITicketService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<Projection> _projectionRepository;
        private readonly IRepository<Movie> _movieRepository;
        private readonly IRepository<Order> _orderRepository;


        public TicketService(IRepository<ShoppingCart> shoppingCartRepository, IUserRepository userRepository, IRepository<Ticket> ticketRepository, IRepository<Projection> projectionRepository, IRepository<Movie> movieRepository, IRepository<Order> orderRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _projectionRepository = projectionRepository;
            _movieRepository = movieRepository;
            _orderRepository = orderRepository;
        }

        public void AddToShoppingCart(AddToShoppingCartDTO addToShoppingCartDTO, string userId)
        {
            var user = this._userRepository.Get(userId);
            var shoppingCart = _shoppingCartRepository.Get(user.UserShoppingCart.Id);
            Ticket ticket = _ticketRepository.Insert(
                new Ticket(addToShoppingCartDTO.ProjectionId, addToShoppingCartDTO.Quantity, shoppingCart.Id));
            shoppingCart.Tickets.Add(ticket);
            _shoppingCartRepository.Update(shoppingCart);
        }

        public Ticket CreateNewTicket(Ticket t)
        {
            t.Id = Guid.NewGuid();
            return _ticketRepository.Insert(t);
        }

        public Ticket DeleteTicket(Guid id)
        {
            return _ticketRepository.Delete(_ticketRepository.Get(id));

        }

        public List<TicketDTO> GetAllTickets()
        {
            List<Ticket> tickets = _ticketRepository.GetAll().ToList();
            List<TicketDTO> ticketsDto = new List<TicketDTO>();
            foreach (var item in tickets)
            {
                var projection = _projectionRepository.Get(item.ProjectionId);
                var movie = _movieRepository.Get(projection.MovieId);
                var shoppingCart = _shoppingCartRepository.Get(item.ShoppingCartId);
                CinemaAppUser user;
                if (shoppingCart.OrderId != Guid.Empty)
                {
                    var order = _orderRepository.Get(shoppingCart.OrderId);
                    user = _userRepository.Get(order.OwnerId);
                }
                else 
                { 
                    user = _userRepository.Get(shoppingCart.OwnerId);
                }
                
                var ticketModel = new TicketDTO
                {
                    Id = item.Id,
                    MovieName = movie.MovieTitle,
                    MovieGenre = movie.MovieGenre,
                    ShoppingCartId = shoppingCart.Id,
                    MovieId = movie.Id,
                    TimeOfProjection = projection.TimeOfProjection,
                    Price = projection.Price,
                    Quantity = item.Quantity,
                    UserName = user.FirstName + " " + user.LastName,
                    userId = user.Id
                };
                ticketsDto.Add(ticketModel);
            }
            return ticketsDto;
        }

        public TicketDTO GetDetailsForTicket(Guid? id)
        {
            var item = _ticketRepository.Get(id);
            var projection = _projectionRepository.Get(item.ProjectionId);
            var movie = _movieRepository.Get(projection.MovieId);
            var shoppingCart = _shoppingCartRepository.Get(item.ShoppingCartId);
            var user = _userRepository.Get(shoppingCart.OwnerId);
            var ticketModel = new TicketDTO
            {
                Id = item.Id,
                MovieName = movie.MovieTitle,
                MovieGenre = movie.MovieGenre,
                ShoppingCartId = shoppingCart.Id,
                MovieId = movie.Id,
                TimeOfProjection = projection.TimeOfProjection,
                Price = projection.Price,
                Quantity = item.Quantity,
                UserName = user.FirstName + " " + user.LastName,
                userId = user.Id
            };
            return ticketModel;
        }

        public Ticket GetTicket(Guid? id)
        {
            return _ticketRepository.Get(id);
        }

        public Ticket UpdateExistingTicket(Ticket t)
        {
            return _ticketRepository.Update(t);
        }
    }
}
