using CinemaApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Domain.DTO
{
    public class ProjectionDTO
    {
        public Guid Id { get; set; }
        public string MovieName { get; set; }
        public Guid MovieId { get; set; }
        public DateTime TimeOfProjection { get; set; }
        public int Price { get; set; }
        public int TicketsAvailable { get; set; }
    }
}
