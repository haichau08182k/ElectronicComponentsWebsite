using electronicComponents.DAL;
using System.Collections.Generic;

namespace electronicComponents.Service
{
    public interface IImportCouponService
    {
        ImportCoupon GetByID(int ID);
        ImportCoupon AddImportCoupon(ImportCoupon importCoupon);
        IEnumerable<ImportCoupon> GetImportCoupon();
        void Delete(int ID);
        void Rehibilitate(int ID);
    }
}