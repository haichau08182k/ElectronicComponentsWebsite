using electronicComponents.DAL;
using System;
using System.Collections.Generic;

namespace electronicComponents.Service
{
    public interface IOrderService
    {
        OrderShip AddOrder(OrderShip order);
        IEnumerable<OrderShip> GetOrderNotApproval();
        IEnumerable<OrderShip> GetOrderNotDelivery();
        IEnumerable<OrderShip> GetOrderDeliveredAndPaid();
        IEnumerable<OrderShip> ApprovedAndNotDelivery();
        IEnumerable<OrderShip> GetDelivered();
        OrderShip GetByID(int ID);
        IEnumerable<OrderShip> GetByCustomerID(int ID);
        OrderShip Approved(int ID);
        OrderShip Delivered(int ID);
        OrderShip Received(int ID);
        decimal GetTotalRevenue();
        int GetTotalOrder();
        void Update(OrderShip order);
        void UpdateTotal(int OrderID, decimal Total);
        IEnumerable<OrderShip> GetListOrderStatistic(DateTime from, DateTime to);
    }
}