using electronicComponents.DAL;
using System.Collections.Generic;

namespace electronicComponents.Service
{
    public interface IDiscountCodeDetailService
    {
        int GetDiscountByCode(string Code);
        DiscountCodeDetail GetByID(int ID);
        IEnumerable<DiscountCodeDetail> GetDiscountCodeDetailList();
        IEnumerable<MemberDiscountCode> GetDiscountCodeDetailListByMember(int MemberID);
        void Used(string code);
    }
}