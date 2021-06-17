using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace electronicComponents.Service
{
    public class ProductService : IProductService
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        public Product GetProductID(int ID)
        {
                return this._unitOfWork.GetRepositoryInstance<Product>().GetFirstorDefault(ID);
        }

        public List<SelectListItem> GetCategoryParent()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var cate = _unitOfWork.GetRepositoryInstance<ProductCategoryParent>().GetAllRecords();
            foreach (var item in cate)
            {
                list.Add(new SelectListItem { Value = item.id.ToString(), Text = item.name });

            }
            return list;

        }
        #region Checkname Producer
        public Producer GetByNameProducer(string Name)
        {
            Producer producer = _unitOfWork.GetRepositoryInstance<Producer>().GetAllRecords().FirstOrDefault(x => x.name == Name);
            return producer;
        }

        public bool CheckNameProducer(string Name)
        {
            var check = _unitOfWork.GetRepositoryInstance<Producer>().GetAllRecords(x => x.name == Name && x.isActive == true);
            if (check.Count() > 0)
            {
                return false;
            }
            return true;
        }
        #endregion
        public Product GetByName(string Name)
        {
            Product product = _unitOfWork.GetRepositoryInstance<Product>().GetAllRecords().FirstOrDefault(x => x.name == Name);
            return product;
        }
        public bool CheckName(string Name)
        {
            var check = _unitOfWork.GetRepositoryInstance<Product>().GetAllRecords(x => x.name == Name && x.isActive == true);
            if (check.Count() > 0)
            {
                return false;
            }
            return true;
        }
        public int GetTotalProduct()
        {
            return _unitOfWork.GetRepositoryInstance<Product>().GetAllRecords().Count();
        }
        public int GetTotalProductPurchased()
        {
            return (int)_unitOfWork.GetRepositoryInstance<Product>().GetAllRecords().Sum(x => x.purchaseCount.Value);
        }

        public ProductCategory GetProductCateID(int ID)
        {
            return this._unitOfWork.GetRepositoryInstance<ProductCategory>().GetFirstorDefault(ID);
        }
        public IEnumerable<Product> GetProductListByCategory(int ProductCategoryID)
        {
            IEnumerable<Product> listProduct = this._unitOfWork.GetRepositoryInstance<Product>().GetAllRecords(x => x.categoryID == ProductCategoryID && x.isActive == true);
            return listProduct;
        }

        public List<SelectListItem> GetCategory()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var cate = _unitOfWork.GetRepositoryInstance<ProductCategory>().GetAllRecords();
            foreach (var item in cate)
            {
                list.Add(new SelectListItem { Value = item.id.ToString(), Text = item.name });

            }
            return list;

        }


        public List<SelectListItem> GetSupplier()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var cate = _unitOfWork.GetRepositoryInstance<Supplier>().GetAllRecords();
            foreach (var item in cate)
            {
                list.Add(new SelectListItem { Value = item.id.ToString(), Text = item.name });

            }
            return list;

        }

        public List<SelectListItem> GetProducer()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var cate = _unitOfWork.GetRepositoryInstance<Producer>().GetAllRecords();
            foreach (var item in cate)
            {
                list.Add(new SelectListItem { Value = item.id.ToString(), Text = item.name });

            }
            return list;

        }

        public IEnumerable<Product> GetListFeaturedProduct()
        {
            return this._unitOfWork.GetRepositoryInstance<Product>().GetAllRecords().Where(x => x.purchaseCount.Value > 0).OrderBy(x => x.purchaseCount.Value);
        }

        public IEnumerable<Product> GetListSellingProduct()
        {
            return this._unitOfWork.GetRepositoryInstance<Product>().GetAllRecords().Where(x => x.discount.Value > 0).OrderBy(x => x.discount.Value);
        }
        public IEnumerable<Product> GetListProduct()
        {
            return this._unitOfWork.GetRepositoryInstance<Product>().GetListParameter(x => x.isActive == true);
        }

        public IEnumerable<Product> GetListProductNew()
        {

            IEnumerable<Product> listProduct = this._unitOfWork.GetRepositoryInstance<Product>().GetListParameter(x => x.isNew == true && x.isActive == true);
            return listProduct;
        }

       

        public IEnumerable<ProductCategoryParent> GetProductCategoryParentList()
        {
            IEnumerable<ProductCategoryParent> listProductCategoryParent = this._unitOfWork.GetRepositoryInstance<ProductCategoryParent>().GetAllRecords().ToList();
            return listProductCategoryParent;
        }

        public IEnumerable<ProductCategory> GetProductCategoryList()
        {
            IEnumerable<ProductCategory> listProductCategory = this._unitOfWork.GetRepositoryInstance<ProductCategory>().GetAllRecords().ToList();
            return listProductCategory;
        }



        //productviewed
        public void AddProductViewByMember(int productID, int memberID)
        {
            try
            {
                ProductViewed productVieweds = _unitOfWork.GetRepositoryInstance<ProductViewed>().GetAllRecords().Single(x => x.productID == productID && x.memberID == memberID);
                if (productVieweds != null)
                {
                    productVieweds.datee = DateTime.Now;
                    _unitOfWork.GetRepositoryInstance<ProductViewed>().Update(productVieweds);
                }
            }
            catch (Exception)
            {
                ProductViewed productViewed = new ProductViewed();
                productViewed.productID = productID;
                productViewed.memberID = memberID;
                productViewed.datee = DateTime.Now;
                _unitOfWork.GetRepositoryInstance<ProductViewed>().Add(productViewed);
            }
        }

        public void DeleteProductViewed(int memberID)
        {
            IEnumerable<ProductViewed> productViewed = _unitOfWork.GetRepositoryInstance<ProductViewed>().GetAllRecords(x => x.memberID == memberID);
            foreach (var item in productViewed)
            {
                _unitOfWork.GetRepositoryInstance<ProductViewed>().Remove(item);
            }
        }

        public void AddViewCount(int ID)
        {
            Product product = _unitOfWork.GetRepositoryInstance<Product>().GetFirstorDefault(ID);
            product.viewCount += 1;
            _unitOfWork.GetRepositoryInstance<Product>().Update(product);
        }


        public List<string> GetProductListName(string keyword)
        {
            IEnumerable<Product> listProductName = this._unitOfWork.GetRepositoryInstance<Product>().GetAllRecords(x => x.name.Contains(keyword) && x.isActive == true);
            List<string> names = new List<string>();
            foreach (var item in listProductName)
            {
                names.Add(item.name);
            }
            return names;
        }
        public IEnumerable<Product> GetProductList(string keyWord)
        {
            IEnumerable<Product> listProduct = this._unitOfWork.GetRepositoryInstance<Product>().GetAllRecords(x => x.name.Contains(keyWord) && x.isActive == true);
            return listProduct;
        }

        public void UpdateProduct(Product product)
        {
            product.lastUpdatedDate = DateTime.Now;
            product.promotionPrice = product.price.Value - (product.price.Value / 100 * product.discount.Value);
            this._unitOfWork.GetRepositoryInstance<Product>().Update(product);
        }

        public IEnumerable<Product> GetProductListStocking()
        {
            return _unitOfWork.GetRepositoryInstance<Product>().GetAllRecords(x => x.quantity > 0 && x.isActive == true);
        }

        public IEnumerable<Product> GetProductListSold(DateTime from, DateTime to)
        {
            IEnumerable<OrderDetail> orderDetails = _unitOfWork.GetRepositoryInstance<OrderDetail>().GetAllRecords(x => DbFunctions.TruncateTime(x.OrderShip.dateOrder) >= from.Date && DbFunctions.TruncateTime(x.OrderShip.dateOrder) <= to.Date);

            List<int> ProductIDs = new List<int>();
            foreach (var item in orderDetails)
            {
                ProductIDs.Add(item.productID.Value);
            }
            if (ProductIDs.Count() > 0)
            {
                return _unitOfWork.GetRepositoryInstance<Product>().GetAllRecords(x => x.purchaseCount > 0 && ProductIDs.Contains(x.id)).OrderByDescending(x => x.purchaseCount);
            }
            return null;
        }
        public IEnumerable<Product> GetProductListViewedByMemberID(int MemberID)
        {
            var productIDList = _unitOfWork.GetRepositoryInstance<ProductViewed>().GetAllRecords(x => x.memberID == MemberID).OrderByDescending(x => x.datee).Select(x => x.productID);
            List<Product> productsList = new List<Product>();
            foreach (var item in productIDList)
            {
                productsList.Add(_unitOfWork.GetRepositoryInstance<Product>().GetFirstorDefault(item.Value));
            }
            return productsList;
        }

        public void Delete(int memberID)
        {
            IEnumerable<ProductViewed> productViewed = _unitOfWork.GetRepositoryInstance<ProductViewed>().GetAllRecords(x => x.memberID == memberID);
            foreach (var item in productViewed)
            {
                _unitOfWork.GetRepositoryInstance<ProductViewed>().Remove(item);
            }
        }
    }

}