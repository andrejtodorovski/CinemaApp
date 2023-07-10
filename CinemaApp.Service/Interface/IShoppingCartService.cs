using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Service.Interface
{
    public interface IShoppingCartService
    {
        public Order CreateOrder(Guid Id);

        public ShoppingCartDTO GetShoppingCartInfo(string userId);
        public ShoppingCart GetShoppingCart(Guid Id);
    }
}
