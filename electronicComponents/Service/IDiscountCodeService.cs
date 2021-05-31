using electronicComponents.DAL;
using System.Collections.Generic;

namespace electronicComponents.Service
{
    public interface IDiscountCodeService
    {
        IEnumerable<DiscountCode> GetDiscountCodeList();
        DiscountCode GetByID(int ID);
        void Block(int ID);
        void Active(int ID);
        void AddDiscountCode(DiscountCode discountCode, int quantity);
        bool CheckCode(string Code);
    }
}