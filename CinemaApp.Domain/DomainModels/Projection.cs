using CinemaApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Domain.DomainModels
{
    public class Projection : BaseEntity
    {
        public Guid MovieId { get; set; }
        public virtual Movie Movie { get; set; }
        public DateTime TimeOfProjection { get; set; }
        public int Price { get; set; }
        public int TicketsAvailable { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
