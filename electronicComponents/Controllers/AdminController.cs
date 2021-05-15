using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using electronicComponents.DAL;
using electronicComponents.Models;
using Newtonsoft.Json;
using electronicComponents.Service;
using System.IO;
using System.Web.Security;

namespace electronicComponents.Controllers
{
    public class AdminController : Controller
    {

        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        // GET: Admin

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Employee employee)
        {
            //Check login
            LoginService loginService = new LoginService();
            Employee employeeCheck = loginService.CheckLogin(employee.userName, employee.passwordd);
            if (employeeCheck != null)
            {

                IEnumerable<Decentralization> decentralizations = _unitOfWork.GetRepositoryInstance<Decentralization>().GetAllRecords();
                List<Decentralization> dec = new List<Decentralization>();
                foreach (var item in decentralizations)
                {
                    if (item.employeeTypeID == employeeCheck.employeeTypeID)
                    {
                        dec.Add(item);
                    }
                }
                string role = "";
                if (dec.Count > 0)
                {
                    foreach (var item in dec)
                    {
                        role += item.Rolee.name + ",";
                    }
                    role = role.Substring(0, role.Length - 1);
                    Decentralization(employeeCheck.userName, role);
                }


                Session["Employee"] = employeeCheck;
                return RedirectToAction("Index");
            }
            return View("index");
        }
        private void Decentralization(string Username, string Role)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                Username,
                DateTime.Now,
                DateTime.Now.AddHours(3),
                false,
                Role,
                FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            Response.Cookies.Add(cookie);
        }







        //Danh mục sản phẩm
        public ActionResult CategoryParent()
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            List<ProductCategoryParent> allCategoryParent = _unitOfWork.GetRepositoryInstance<ProductCategoryParent>().GetAllRecords().ToList();
            return View(allCategoryParent);
        }

        public ActionResult AddCategoryParent()
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddCategoryParent(ProductCategoryParent prodPr)
        {
            prodPr.isDelete = false;
            _unitOfWork.GetRepositoryInstance<ProductCategoryParent>().Add(prodPr);
            return RedirectToAction("CategoryParent");

        }


        public ActionResult EditCategoryParent(int catePr)
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            return View(_unitOfWork.GetRepositoryInstance<ProductCategoryParent>().GetFirstorDefault(catePr));
        }
        [HttpPost]
        public ActionResult EditCategoryParent(ProductCategoryParent prodPr)
        {
            _unitOfWork.GetRepositoryInstance<ProductCategoryParent>().Update(prodPr);
            return RedirectToAction("CategoryParent");

        }






        //Loại sản phẩm
        public ActionResult Categories()
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            ProductService model = new ProductService();
            ViewBag.CategoryParentList = model.GetCategoryParent();
            List<ProductCategory> allCategories = _unitOfWork.GetRepositoryInstance<ProductCategory>().GetAllRecords().ToList();
            return View(allCategories);
        }
        //chỉnh sửa

        public ActionResult EditCategories(int cateID)
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            ProductService model = new ProductService();
            ViewBag.CategoryParentList = model.GetCategoryParent();
            return View(_unitOfWork.GetRepositoryInstance<ProductCategory>().GetFirstorDefault(cateID));
        }

        [HttpPost]
        public ActionResult EditCategories(ProductCategory tbl, HttpPostedFileBase file)
        {

            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Content/images"), pic);
                // file is uploaded
                file.SaveAs(path);
            }
            tbl.imagee = file != null ? pic : tbl.imagee;
            tbl.lastUpdatedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<ProductCategory>().Update(tbl);
            return RedirectToAction("Categories");
        }
        //Thêm

        public ActionResult AddCategories()
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            ProductService model = new ProductService();
            ViewBag.CategoryParentList = model.GetCategoryParent();
            return View();
        }
        [HttpPost]
        public ActionResult AddCategories(ProductCategory tbl, HttpPostedFileBase file)
        {
            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Content/images"), pic);
                // file is uploaded
                file.SaveAs(path);
            }
            tbl.imagee = pic;
            tbl.lastUpdatedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<ProductCategory>().Add(tbl);
            return RedirectToAction("Categories");
        }








        //Sản phẩm

        ////[Authorize(Roles = "ProductManage")]
        public ActionResult Product()
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            ProductService model = new ProductService();
            ViewBag.CategoryParentList = model.GetCategory();
            List<Product> allProduct = _unitOfWork.GetRepositoryInstance<Product>().GetAllRecords().ToList();
            return View(allProduct);
        }



        public ActionResult EditProduct(int prodID)
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            ProductService model = new ProductService();
            ViewBag.CategoryList = model.GetCategory();
            ViewBag.SupplierList = model.GetSupplier();
            ViewBag.ProducerList = model.GetProducer();

            return View(_unitOfWork.GetRepositoryInstance<Product>().GetFirstorDefault(prodID));
        }

        [HttpPost]
        public ActionResult EditProduct(Product tbl, HttpPostedFileBase[] file)
        {

            int errorCount = 0;
            for (int i = 0; i < file.Length; i++)
            {
                //Check content image
                if (file[i] != null && file[i].ContentLength > 0)
                {
                    //Check format iamge
                    if (file[i].ContentType != "image/jpeg" && file[i].ContentType != "image/png" && file[i].ContentType != "image/gif")
                    {
                        //Set viewbag
                        ViewBag.upload += "Hình ảnh không hợp lệ<br/>";
                        //increase by 1 unit errorCount
                        errorCount++;
                    }
                    else
                    {
                        //Get file name
                        var fileName = Path.GetFileName(file[i].FileName);
                        //Get path
                        var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                        //Check exitst
                        if (!System.IO.File.Exists(path))
                        {
                            //Add image into folder
                            file[i].SaveAs(path);
                        }
                    }
                }
            }
            //Set new value image for product
            for (int i = 0; i < file.Length; i++)
            {
                if (file[i] != null)
                {
                    if (i == 0)
                        tbl.image1 = file[0].FileName;
                    else if (i == 1)
                        tbl.image2 = file[1].FileName;
                    else if (i == 2)
                        tbl.image3 = file[2].FileName;
                }
            }
            tbl.lastUpdatedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<Product>().Update(tbl);
            return RedirectToAction("Product");
        }



        public ActionResult AddProduct()
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            ProductService model = new ProductService();
            ViewBag.CategoryList = model.GetCategory();
            ViewBag.SupplierList = model.GetSupplier();
            ViewBag.ProducerList = model.GetProducer();

            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Product tbl, HttpPostedFileBase[] file)
        {

            int errorCount = 0;
            for (int i = 0; i < file.Length; i++)
            {
                //Check content image
                if (file[i] != null && file[i].ContentLength > 0)
                {
                    //Check format iamge
                    if (file[i].ContentType != "image/jpeg" && file[i].ContentType != "image/png" && file[i].ContentType != "image/gif")
                    {
                        //Set viewbag
                        ViewBag.upload += "Hình ảnh không hợp lệ<br/>";
                        //increase by 1 unit errorCount
                        errorCount++;
                    }
                    else
                    {
                        //Get file name
                        var fileName = Path.GetFileName(file[i].FileName);
                        //Get path
                        var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                        //Check exitst
                        if (!System.IO.File.Exists(path))
                        {
                            //Add image into folder
                            file[i].SaveAs(path);
                        }
                    }
                }
            }
            //Set new value image for product
            for (int i = 0; i < file.Length; i++)
            {
                if (file[i] != null)
                {
                    if (i == 0)
                        tbl.image1 = file[0].FileName;
                    else if (i == 1)
                        tbl.image2 = file[1].FileName;
                    else if (i == 2)
                        tbl.image3 = file[2].FileName;
                }
            }

            tbl.lastUpdatedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<Product>().Add(tbl);
            return RedirectToAction("Product");
        }






        //Nhà cung cấp

        public ActionResult Supplier()
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            List<Supplier> allSupplier = _unitOfWork.GetRepositoryInstance<Supplier>().GetAllRecords().ToList();
            return View(allSupplier);

        }

        public ActionResult EditSupplier(int supID)
        {
            return View(_unitOfWork.GetRepositoryInstance<Supplier>().GetFirstorDefault(supID));
        }
        [HttpPost]
        public ActionResult EditSupplier(Supplier sup)
        {
            sup.lastUpdatedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<Supplier>().Update(sup);
            return RedirectToAction("Supplier");
        }

        public ActionResult AddSupplier()
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddSupplier(Supplier supplier)
        {
            supplier.lastUpdatedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<Supplier>().Add(supplier);
            return RedirectToAction("Supplier");
        }



        //Nhà sẩn xuất

        public ActionResult Producer()
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            List<Producer> allProducer = _unitOfWork.GetRepositoryInstance<Producer>().GetAllRecords().ToList();
            return View(allProducer);
        }

        public ActionResult AddProducer()
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddProducer(Producer producer)
        {
            producer.lastUpdateDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<Producer>().Add(producer);
            return RedirectToAction("Producer");

        }

        public ActionResult EditProducer(int prodID)
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            return View(_unitOfWork.GetRepositoryInstance<Producer>().GetFirstorDefault(prodID));
        }
        [HttpPost]
        public ActionResult EditProducer(Producer producer)
        {
            producer.lastUpdateDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<Producer>().Update(producer);
            return RedirectToAction("Producer");

        }
    }
}