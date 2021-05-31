using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace electronicComponents.Service
{

    public class CartService : ICartService
    {
        private readonly GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        public List<Cart> GetCart(int memberID)
        {
            return _unitOfWork.GetRepositoryInstance<Cart>().GetAllRecords().Where(x => x.memberID == memberID).ToList();
        }
        public bool CheckCartMember(int memberID)
        {
            if (_unitOfWork.GetRepositoryInstance<Cart>().GetAllRecords().Where(x => x.memberID == memberID).Count() > 0)
                return true;
            return false;
        }
        public bool CheckProductInCart(int ProductID, int MemberID)
        {
            if (_unitOfWork.GetRepositoryInstance<Cart>().GetAllRecords().Where(x => x.memberID == MemberID && x.productID == ProductID).Count() > 0)
                return true;
            return false;
        }
        public void AddCartIntoMember(Cart cart, int MemberID)
        {
            cart.memberID = MemberID;
            _unitOfWork.GetRepositoryInstance<Cart>().Add(cart);
        }
        public void AddQuantityProductCartMember(int ProductID, int MemberID)
        {
            Cart cartUpdate = _unitOfWork.GetRepositoryInstance<Cart>().GetAllRecords().Single(x => x.productID == ProductID && x.memberID == MemberID);
            cartUpdate.quantity += 1;
            _unitOfWork.GetRepositoryInstance<Cart>().Update(cartUpdate);
        }
        public void RemoveCart(int ProductID, int MemberID)
        {
            Cart cart = _unitOfWork.GetRepositoryInstance<Cart>().GetAllRecords().Single(x => x.productID == ProductID && x.memberID == MemberID);
            _unitOfWork.GetRepositoryInstance<Cart>().Remove(cart);
        }

        public void UpdateQuantityCartMember(int Quantity, int ProductID, int MemberID)
        {
            Cart cartUpdate = _unitOfWork.GetRepositoryInstance<Cart>().GetAllRecords().Single(x => x.productID == ProductID && x.memberID == MemberID);
            cartUpdate.quantity = Quantity;
            cartUpdate.total = cartUpdate.quantity * cartUpdate.Product.promotionPrice;
            _unitOfWork.GetRepositoryInstance<Cart>().Update(cartUpdate);
        }
    }
}