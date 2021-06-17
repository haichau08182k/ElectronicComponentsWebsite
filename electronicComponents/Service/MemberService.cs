using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace electronicComponents.Service
{

    public class MemberService : IMemberService
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        public Member AddMember(Member member)
        {
            member.memberCategoryID = 1;
            member.amountPurchased = 0;
            member.isDeleted = false;
            this._unitOfWork.GetRepositoryInstance<Member>().Add(member);
            return member;
        }
        public int GetTotalMember()
        {
            return _unitOfWork.GetRepositoryInstance<Member>().GetAllRecords().Count();
        }
        public Member CheckCapcha(int ID, string capcha)
        {
            Member member = GetByID(ID);
            if (member.capcha == capcha)
            {
                member.emailConfirmed = true;
                UpdateMember(member);
                return member;
            }
            return null;
        }
        public Member CheckLogin(string username, string password)
        {
            Member member = this._unitOfWork.GetRepositoryInstance<Member>().GetAllRecords().SingleOrDefault(x => x.userName == username && x.passwordd == password && x.emailConfirmed == true);
            if (member == null)
            {
                member = this._unitOfWork.GetRepositoryInstance<Member>().GetAllRecords().SingleOrDefault(x => x.email == username && x.passwordd == password && x.emailConfirmed == true);
            }
            return member;
        }

        public Member GetByID(int ID)
        {
            return this._unitOfWork.GetRepositoryInstance<Member>().GetFirstorDefault(ID);
        }


        public void UpdateMember(Member member)
        {
            this._unitOfWork.GetRepositoryInstance<Member>().Update(member);
        }

        public void UpdateCapcha(int ID, string capcha)
        {
            Member member = GetByID(ID);
            member.capcha = capcha;
            UpdateMember(member);
        }
        public void UpdateAmountPurchased(int ID, decimal AmountPurchased)
        {
            Member member = _unitOfWork.GetRepositoryInstance<Member>().GetFirstorDefault(ID);
            member.amountPurchased += AmountPurchased;
            _unitOfWork.GetRepositoryInstance<Member>().Update(member);
        }
        public IEnumerable<Member> GetMemberList()
        {
            IEnumerable<Member> listMember = this._unitOfWork.GetRepositoryInstance<Member>().GetAllRecords();
            return listMember;
        }
        public void GiftForNewMember(int MemberID)
        {
            MemberDiscountCode member = new MemberDiscountCode();
            string code = _unitOfWork.GetRepositoryInstance<DiscountCode>().GetAllRecords(x => x.numberDiscount == 10).First().DiscountCodeDetails.First(x => x.isUsed == false).code;
            member.discountCodeDetailID = _unitOfWork.GetRepositoryInstance<DiscountCodeDetail>().GetAllRecords(x => x.code == code).First().id;
            member.memberID = MemberID;
            _unitOfWork.GetRepositoryInstance<MemberDiscountCode>().Add(member);
        }
        public IEnumerable<Member> GetMemberListForStatistic()
        {
            IEnumerable<Member> listMember = this._unitOfWork.GetRepositoryInstance<Member>().GetAllRecords(x => x.amountPurchased > 0 && x.isDeleted == false).OrderByDescending(x => x.amountPurchased);
            return listMember;
        }
        public void ResetPassword(int MemberID, string NewPassword)
        {
            Member member = GetByID(MemberID);
            member.passwordd = NewPassword;
            _unitOfWork.GetRepositoryInstance<Member>().Update(member);
        }

    }
}