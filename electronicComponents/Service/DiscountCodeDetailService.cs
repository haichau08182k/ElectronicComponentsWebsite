using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace electronicComponents.Service
{
    public class DiscountCodeDetailService : IDiscountCodeDetailService
    {
        private readonly GenericUnitOfWork _unitOfWork;
        public DiscountCodeDetailService(GenericUnitOfWork repositoryContext)
        {
            this._unitOfWork = repositoryContext;
        }

        public DiscountCodeDetail GetByID(int ID)
        {
            return _unitOfWork.GetRepositoryInstance<DiscountCodeDetail>().GetFirstorDefault(ID);
        }

        public int GetDiscountByCode(string Code)
        {
            return (int)_unitOfWork.GetRepositoryInstance<DiscountCodeDetail>().GetAllRecords().SingleOrDefault(x => x.code.Contains(Code)).DiscountCode.numberDiscount;
        }

        public IEnumerable<DiscountCodeDetail> GetDiscountCodeDetailList()
        {
            return _unitOfWork.GetRepositoryInstance<DiscountCodeDetail>().GetAllRecords(x => x.isUsed == false);
        }

        public IEnumerable<MemberDiscountCode> GetDiscountCodeDetailListByMember(int MemberID)
        {
            return _unitOfWork.GetRepositoryInstance<MemberDiscountCode>().GetAllRecords(x => x.memberID == MemberID && x.DiscountCodeDetail.isUsed == false);
        }

        public void Used(string code)
        {
            DiscountCodeDetail discountCodeDetail = _unitOfWork.GetRepositoryInstance<DiscountCodeDetail>().GetAllRecords().SingleOrDefault(x => x.code == code);
            discountCodeDetail.isUsed = true;
            _unitOfWork.GetRepositoryInstance<DiscountCodeDetail>().Update(discountCodeDetail);
        }
    }
}