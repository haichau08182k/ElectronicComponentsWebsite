using electronicComponents.DAL;
using System.Collections.Generic;

namespace electronicComponents.Service
{
    public interface IOrderDetailService
    {
        OrderDetail AddOrderDetail(OrderDetail order);
        IEnumerable<OrderDetail> GetByOrderID(int ID);
        void SetIsRating(int ID);
    }
}