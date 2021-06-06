using electronicComponents.DAL;
using System.Collections.Generic;
using System.Web.Mvc;

namespace electronicComponents.Service
{
    public interface IProductService
    {
        Product GetProductID(int ID);
        List<SelectListItem> GetCategory();
        List<SelectListItem> GetCategoryParent();
        IEnumerable<Product> GetListProduct();

        IEnumerable<Product> GetListSellingProduct();
        IEnumerable<Product> GetListProductNew();
        List<SelectListItem> GetProducer();
        IEnumerable<ProductCategory> GetProductCategoryList();
        IEnumerable<ProductCategoryParent> GetProductCategoryParentList();
       
        List<SelectListItem> GetSupplier();
        void AddProductViewByMember(int productID, int memberID);
        void DeleteProductViewed(int memberID);
        void AddViewCount(int ID);
    }
}