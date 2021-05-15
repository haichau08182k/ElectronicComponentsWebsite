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
            this._unitOfWork.GetRepositoryInstance<Member>().Add(member);
            return member;
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
    }
}