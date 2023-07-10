using CinemaApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Domain.DTO
{
    public class AddToShoppingCartDTO
    {
        public Guid ProjectionId { get; set; }

        public int Quantity { get; set; }
    }
}
