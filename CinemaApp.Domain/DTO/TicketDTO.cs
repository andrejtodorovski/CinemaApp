using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Domain.DTO
{
    public class TicketDTO
    {
        public Guid Id { get; set; }
        public string MovieName { get; set; }
        public string MovieGenre { get; set; }
        public Guid ShoppingCartId { get; set; }
        public Guid MovieId { get; set; }
        public DateTime TimeOfProjection { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string UserName { get; set; }
        public string userId { get; set; }

    }
}
