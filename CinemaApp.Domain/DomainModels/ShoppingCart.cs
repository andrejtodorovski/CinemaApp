using CinemaApp.Domain.Identity;
using System;
using System.Collections.Generic;

namespace CinemaApp.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string OwnerId { get; set; }
        public virtual CinemaAppUser Owner { get; set; }

        public Guid OrderId { get; set; }

        public virtual Order Order { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
