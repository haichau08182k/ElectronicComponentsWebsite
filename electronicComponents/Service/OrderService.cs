using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace electronicComponents.Service
{
    public class OrderService : IOrderService
    {
        private readonly GenericUnitOfWork _unitOfWork;
        public OrderService(GenericUnitOfWork repositoryContext)
        {
            this._unitOfWork = repositoryContext;
        }
        public OrderShip AddOrder(OrderShip order)
        {
            this._unitOfWork.GetRepositoryInstance<OrderShip>().Add(order);
            return order;
        }

        public OrderShip Approved(int ID)
        {
            OrderShip order = _unitOfWork.GetRepositoryInstance<OrderShip>().GetFirstorDefault(ID);
            order.isApproved = true;
            _unitOfWork.GetRepositoryInstance<OrderShip>().Update(order);
            return order;
        }

        public IEnumerable<OrderShip> ApprovedAndNotDelivery()
        {
            return _unitOfWork.GetRepositoryInstance<OrderShip>().GetAllRecords(x => x.isApproved == true && x.isDelivere == false);
        }

        public OrderShip Delivered(int ID)
        {
            OrderShip order = _unitOfWork.GetRepositoryInstance<OrderShip>().GetFirstorDefault(ID);
            order.isDelivere = true;
            _unitOfWork.GetRepositoryInstance<OrderShip>().Update(order);
            return order;
        }

        public OrderShip GetByID(int ID)
        {
            return _unitOfWork.GetRepositoryInstance<OrderShip>().GetFirstorDefault(ID);
        }

        public IEnumerable<OrderShip> GetDelivered()
        {
            return _unitOfWork.GetRepositoryInstance<OrderShip>().GetAllRecords(x => x.isDelivere == true);
        }

        public IEnumerable<OrderShip> GetOrderDeliveredAndPaid()
        {
            return _unitOfWork.GetRepositoryInstance<OrderShip>().GetAllRecords(x => x.isPaid == true && x.isDelivere == true);
        }

        public IEnumerable<OrderShip> GetOrderNotDelivery()
        {
            return _unitOfWork.GetRepositoryInstance<OrderShip>().GetAllRecords(x => x.isDelivere == false);
        }

        public IEnumerable<OrderShip> GetOrderNotApproval()
        {
            return _unitOfWork.GetRepositoryInstance<OrderShip>().GetAllRecords(x => x.isApproved == false);
        }

        public void Update(OrderShip order)
        {
            _unitOfWork.GetRepositoryInstance<OrderShip>().Update(order);
        }

        public IEnumerable<OrderShip> GetByCustomerID(int ID)
        {
            return _unitOfWork.GetRepositoryInstance<OrderShip>().GetAllRecords(x => x.customerID == ID);
        }

        public OrderShip Received(int ID)
        {
            OrderShip order = _unitOfWork.GetRepositoryInstance<OrderShip>().GetFirstorDefault(ID);
            order.isReceived = true;
            order.isPaid = true;
            _unitOfWork.GetRepositoryInstance<OrderShip>().Update(order);
            //Update PurchasedCount
            IEnumerable<OrderDetail> orderDetails = _unitOfWork.GetRepositoryInstance<OrderDetail>().GetAllRecords(x => x.orderID == ID);
            foreach (var item in orderDetails)
            {
                Product product = _unitOfWork.GetRepositoryInstance<Product>().GetFirstorDefault(item.productID.Value);
                product.purchaseCount += item.quantity;
                product.quantity -= item.quantity;
                _unitOfWork.GetRepositoryInstance<Product>().Update(product);
            }
            return order;
        }
        public decimal GetTotalRevenue()
        {
            return _unitOfWork.GetRepositoryInstance<OrderDetail>().GetAllRecords(x => x.OrderShip.isPaid == true).Sum(x => x.price.Value);
        }

        public int GetTotalOrder()
        {
            return _unitOfWork.GetRepositoryInstance<OrderShip>().GetAllRecords().Count();
        }

        public void UpdateTotal(int OrderID, decimal Total)
        {
            OrderShip order = _unitOfWork.GetRepositoryInstance<OrderShip>().GetFirstorDefault(OrderID);
            order.total = Total;
            _unitOfWork.GetRepositoryInstance<OrderShip>().Update(order);
        }

        public IEnumerable<OrderShip> GetListOrderStatistic(DateTime from, DateTime to)
        {
            IEnumerable<OrderDetail> orderDetails = _unitOfWork.GetRepositoryInstance<OrderDetail>().GetAllRecords(x => DbFunctions.TruncateTime(x.OrderShip.dateOrder) >= from.Date && DbFunctions.TruncateTime(x.OrderShip.dateOrder) <= to.Date);

            List<int> OrderIDs = new List<int>();
            foreach (var item in orderDetails)
            {
                OrderIDs.Add(item.orderID.Value);
            }
            if (OrderIDs.Count() > 0)
            {
                return _unitOfWork.GetRepositoryInstance<OrderShip>().GetAllRecords(x => x.isReceived == true && OrderIDs.Contains(x.id)).OrderByDescending(x => x.total);
            }
            return null;
        }
    }

}