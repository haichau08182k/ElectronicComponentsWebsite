using electronicComponents.DAL;
using System.Collections.Generic;

namespace electronicComponents.Service
{
    public interface IMemberService
    {
        Member AddMember(Member member);
        Member CheckCapcha(int ID, string capcha);
        Member CheckLogin(string username, string password);
        Member GetByID(int ID);
        void UpdateCapcha(int ID, string capcha);
        void UpdateMember(Member member);
        void UpdateAmountPurchased(int ID, decimal AmountPurchased);
        IEnumerable<Member> GetMemberList();
        int GetTotalMember();
        void GiftForNewMember(int MemberID);
        IEnumerable<Member> GetMemberListForStatistic();
        void ResetPassword(int MemberID, string NewPassword);
    }
}