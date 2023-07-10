using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Domain.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public List<TicketDTO> Tickets { get; set; }
        public int TotalPrice { get; set; }
        public DateTime OrderPlaced { get; set; }
    }
}
