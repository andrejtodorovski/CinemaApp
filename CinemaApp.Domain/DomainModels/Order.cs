using CinemaApp.Domain.Identity;
using System;

namespace CinemaApp.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string OwnerId { get; set; }
        public virtual CinemaAppUser Owner { get; set; }
        public Guid ShoppingCartId { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }
        public DateTime OrderPlaced { get; set; }
    }
}
