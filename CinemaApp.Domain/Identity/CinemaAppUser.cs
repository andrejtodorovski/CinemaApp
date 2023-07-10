using CinemaApp.Domain.DomainModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CinemaApp.Domain.Identity
{
    public class CinemaAppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }

        public virtual ShoppingCart UserShoppingCart { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
