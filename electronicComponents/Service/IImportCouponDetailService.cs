using electronicComponents.DAL;
using System.Collections.Generic;

namespace electronicComponents.Service
{
    public interface IImportCouponDetailService
    {
        ImportCouponDetail AddImportCouponDetail(ImportCouponDetail import);
        IEnumerable<ImportCouponDetail> GetByImportCouponID(int ID);
    }
}