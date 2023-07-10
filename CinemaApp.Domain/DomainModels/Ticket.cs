using CinemaApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Domain.DomainModels
{
    public class Ticket : BaseEntity
    {
        public Guid ProjectionId { get; set; }
        public virtual Projection Projection { get; set; }
        
        public int Quantity { get; set; }

        public Ticket(Guid projectionId, int quantity, Guid shoppingCartId)
        {
            ProjectionId = projectionId;
            Quantity = quantity;
            ShoppingCartId = shoppingCartId;
        }
        public Ticket()
        {

        }
        public Guid ShoppingCartId { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}
