using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Service.Interface
{
    public interface IOrderService
    {

        public OrderDTO GetDetailsForOrder(Guid? id);
        public List<OrderDTO> GetAllOrders();
        public Order CreateNewOrder(Order o);
        public Order UpdateExistingOrder(Order o);
        public Order DeleteOrder(Guid id);
        public Order GetOrder(Guid? id);
        public List<OrderDTO> GetAllOrdersByOwnerId(string userId);

    }
}
