using electronicComponents.DAL;
using System.Collections.Generic;

namespace electronicComponents.Service
{
    public interface ICartService
    {
        List<Cart> GetCart(int memberID);
        bool CheckCartMember(int memberID);
        void AddCartIntoMember(Cart cart, int MemberID);
        bool CheckProductInCart(int ProductID, int MemberID);
        void AddQuantityProductCartMember(int ProductID, int MemberID);
        void RemoveCart(int ProductID, int MemberID);
        void UpdateQuantityCartMember(int Quantity, int ProductID, int MemberID);
    }
}