using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace electronicComponents.Service
{
    public class OrderDetailService : IOrderDetailService
    {

        private readonly GenericUnitOfWork _unitOfWork;
        public OrderDetailService(GenericUnitOfWork repositoryContext)
        {
            this._unitOfWork = repositoryContext;
        }
        public OrderDetail AddOrderDetail(OrderDetail orderDetail)
        {
            orderDetail.isRating = false;
            this._unitOfWork.GetRepositoryInstance<OrderDetail>().Add(orderDetail);
            return orderDetail;
        }

        public IEnumerable<OrderDetail> GetByOrderID(int ID)
        {
            return this._unitOfWork.GetRepositoryInstance<OrderDetail>().GetAllRecords().Where(x => x.orderID == ID);
        }
        public void SetIsRating(int ID)
        {
            OrderDetail orderDetail = _unitOfWork.GetRepositoryInstance<OrderDetail>().GetFirstorDefault(ID);
            orderDetail.isRating = true;
            _unitOfWork.GetRepositoryInstance<OrderDetail>().Update(orderDetail);
        }

    }
}