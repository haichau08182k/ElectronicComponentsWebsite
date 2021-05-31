using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace electronicComponents.Service
{
    public class ImportCouponService : IImportCouponService
    {
        private readonly GenericUnitOfWork _unitOfWork;
        public ImportCouponService(GenericUnitOfWork repositoryContext)
        {
            this._unitOfWork = repositoryContext;
        }
        public ImportCoupon AddImportCoupon(ImportCoupon importCoupon)
        {
            _unitOfWork.GetRepositoryInstance<ImportCoupon>().Add(importCoupon);
            return importCoupon;
        }

        public void Delete(int ID)
        {
            ImportCoupon importCoupon = _unitOfWork.GetRepositoryInstance<ImportCoupon>().GetFirstorDefault(ID);
            importCoupon.isDelete = true;
            _unitOfWork.GetRepositoryInstance<ImportCoupon>().Update(importCoupon);
        }

        public ImportCoupon GetByID(int ID)
        {
            return _unitOfWork.GetRepositoryInstance<ImportCoupon>().GetFirstorDefault(ID);
        }

        public IEnumerable<ImportCoupon> GetImportCoupon()
        {
            return _unitOfWork.GetRepositoryInstance<ImportCoupon>().GetAllRecords();
        }

        public void Rehibilitate(int ID)
        {
            ImportCoupon importCoupon = _unitOfWork.GetRepositoryInstance<ImportCoupon>().GetFirstorDefault(ID);
            importCoupon.isDelete = false;
            _unitOfWork.GetRepositoryInstance<ImportCoupon>().Update(importCoupon);
        }
    }
}