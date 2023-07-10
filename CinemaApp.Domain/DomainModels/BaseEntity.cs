using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Domain.DomainModels
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
