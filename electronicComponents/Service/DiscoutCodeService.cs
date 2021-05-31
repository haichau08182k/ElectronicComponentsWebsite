﻿using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace electronicComponents.Service
{
    public class DiscountCodeService : IDiscountCodeService
    {
        private readonly GenericUnitOfWork _unitOfWork;
        public DiscountCodeService(GenericUnitOfWork repositoryContext)
        {
            this._unitOfWork = repositoryContext;
        }

        public void Active(int ID)
        {
            DiscountCode discount = _unitOfWork.GetRepositoryInstance<DiscountCode>().GetFirstorDefault(ID);
            discount.isActive = true;
            _unitOfWork.GetRepositoryInstance<DiscountCode>().Update(discount);
        }

        public void AddDiscountCode(DiscountCode discountCode, int quantity)
        {
            discountCode.isActive = true;
            _unitOfWork.GetRepositoryInstance<DiscountCode>().Add(discountCode);
            DiscountCodeDetail discountCodeDetail = new DiscountCodeDetail();
            discountCodeDetail.discountCodeID = discountCode.id;
            discountCodeDetail.isUsed = false;
            Random random = new Random();
            for (int i = 0; i < quantity; i++)
            {
                lock (discountCodeDetail)
                { // synchronize

                    int code = random.Next(100000, 999999);
                    discountCodeDetail.code = code.ToString();
                    _unitOfWork.GetRepositoryInstance<DiscountCodeDetail>().Add(discountCodeDetail);
                }
            }
        }
        public void Block(int ID)
        {
            DiscountCode discount = _unitOfWork.GetRepositoryInstance<DiscountCode>().GetFirstorDefault(ID);
            discount.isActive = false;
            _unitOfWork.GetRepositoryInstance<DiscountCode>().Update(discount);
        }

        public bool CheckCode(string Code)
        {
            DiscountCodeDetail discountCodeDetail = _unitOfWork.GetRepositoryInstance<DiscountCodeDetail>().GetAllRecords().FirstOrDefault(x => x.code == Code);
            if (discountCodeDetail != null)
            {
                return true;
            }
            return false;
        }

        public DiscountCode GetByID(int ID)
        {
            return _unitOfWork.GetRepositoryInstance<DiscountCode>().GetFirstorDefault(ID);
        }

        public IEnumerable<DiscountCode> GetDiscountCodeList()
        {
            return _unitOfWork.GetRepositoryInstance<DiscountCode>().GetAllRecords();
        }
    }
}