using electronicComponents.DAL;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace electronicComponents.Service
{
    public interface IProductService
    {
        void UpdateProduct(Product product);
        IEnumerable<Product> GetProductListSold(DateTime from, DateTime to);
        Product GetProductID(int ID);
        ProductCategory GetProductCateID(int ID);
        List<SelectListItem> GetCategory();
        List<SelectListItem> GetCategoryParent();
        IEnumerable<Product> GetProductListByCategory(int ProductCategoryID);
        IEnumerable<Product> GetListProduct();
        List<string> GetProductListName(string keyword);
        IEnumerable<Product> GetProductList(string keyWord);
        IEnumerable<Product> GetListFeaturedProduct();
        IEnumerable<Product> GetListSellingProduct();
        IEnumerable<Product> GetListProductNew();
        List<SelectListItem> GetProducer();
        IEnumerable<ProductCategory> GetProductCategoryList();
        IEnumerable<ProductCategoryParent> GetProductCategoryParentList();
        int GetTotalProduct();
        int GetTotalProductPurchased();
        List<SelectListItem> GetSupplier();
        void AddProductViewByMember(int productID, int memberID);
        void DeleteProductViewed(int memberID);
        void AddViewCount(int ID);
        IEnumerable<Product> GetProductListStocking();
        IEnumerable<Product> GetProductListViewedByMemberID(int MemberID);
        void Delete(int memberID);
        Product GetByName(string Name);
        bool CheckName(string Name);
        Producer GetByNameProducer(string Name);
        bool CheckNameProducer(string Name);
    }
}