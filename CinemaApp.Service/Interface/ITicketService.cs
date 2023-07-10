using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.DTO;
using CinemaApp.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Service.Interface
{
    public interface ITicketService
    {
        
        public void AddToShoppingCart(AddToShoppingCartDTO addToShoppingCartDTO, string userId);
        public TicketDTO GetDetailsForTicket(Guid? id);
        public Ticket GetTicket(Guid? id);
        public List<TicketDTO> GetAllTickets();
        public Ticket CreateNewTicket(Ticket t);
        public Ticket UpdateExistingTicket(Ticket t);
        public Ticket DeleteTicket(Guid id);
    }
}
