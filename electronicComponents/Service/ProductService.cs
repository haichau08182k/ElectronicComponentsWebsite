using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
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

        public IEnumerable<Product> GetListProduct()
        {
            return this._unitOfWork.GetRepositoryInstance<Product>().GetListParameter(x => x.isActive == true);
        }

        public IEnumerable<Product> GetListProductNew()
        {

            IEnumerable<Product> listProduct = this._unitOfWork.GetRepositoryInstance<Product>().GetListParameter(x => x.isNew == true && x.isActive == true);
            return listProduct;
        }
        public IEnumerable<Product> GetListProductFeatured()
        {
            IEnumerable<Product> listProduct = this._unitOfWork.GetRepositoryInstance<Product>().GetListParameter(x => x.isActive == true && x.hotFlag == true);
            return listProduct;

        }
        public IEnumerable<Product> GetProductInHome()
        {
            IEnumerable<Product> products = this._unitOfWork.GetRepositoryInstance<Product>().GetAllRecords().Where(x => x.homeFlag == true).OrderByDescending(x => x.lastUpdatedDate).Take(10);
            return products;
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
    }

}