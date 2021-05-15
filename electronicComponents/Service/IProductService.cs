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
        IEnumerable<Product> GetListProductFeatured();
        IEnumerable<Product> GetListProductNew();
        List<SelectListItem> GetProducer();
        IEnumerable<ProductCategory> GetProductCategoryList();
        IEnumerable<ProductCategoryParent> GetProductCategoryParentList();
        IEnumerable<Product> GetProductInHome();
        List<SelectListItem> GetSupplier();
    }
}