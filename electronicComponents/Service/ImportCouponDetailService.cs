using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace electronicComponents.Service
{
    public class ImportCouponDetailService : IImportCouponDetailService
    {
        private readonly GenericUnitOfWork _unitOfWork;
        public ImportCouponDetailService(GenericUnitOfWork repositoryContext)
        {
            this._unitOfWork = repositoryContext;
        }

        public ImportCouponDetail AddImportCouponDetail(ImportCouponDetail importCouponDetail)
        {
            _unitOfWork.GetRepositoryInstance<ImportCouponDetail>().Add(importCouponDetail);
            //Update total amount
            ImportCoupon importCoupon =_unitOfWork.GetRepositoryInstance<ImportCoupon>().GetFirstorDefault(importCouponDetail.importCouponID.Value);
            Supplier supplier = _unitOfWork.GetRepositoryInstance<Supplier>().GetFirstorDefault(importCoupon.supplierID.Value);
            supplier.totalAmount += importCouponDetail.price * importCouponDetail.quantity;
            _unitOfWork.GetRepositoryInstance<Supplier>().Update(supplier);
            return importCouponDetail;
        }

        public IEnumerable<ImportCouponDetail> GetByImportCouponID(int ID)
        {
            return _unitOfWork.GetRepositoryInstance<ImportCouponDetail>().GetAllRecords().Where(x => x.importCouponID == ID);
        }
    }
}