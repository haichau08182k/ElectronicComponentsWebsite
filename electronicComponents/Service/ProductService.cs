﻿using electronicComponents.DAL;
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

        public IEnumerable<Product> GetListSellingProduct()
        {
            return this._unitOfWork.GetRepositoryInstance<Product>().GetAllRecords().Where(x => x.purchaseCount.Value > 0).OrderBy(x => x.purchaseCount.Value);
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
    }

}