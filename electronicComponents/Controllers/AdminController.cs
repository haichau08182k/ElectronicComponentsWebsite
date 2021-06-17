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
using System.Net;
using PagedList;
using OfficeOpenXml;

namespace electronicComponents.Controllers
{

    public class AdminController : Controller
    {
        private IQAService _qAService;
        private IProductService _productService;
        private IMemberService _memberService;
        private IEmployeeService _employeeService;
        private IEmployeeTypeService _employeeTypeService;
        private IOrderService _orderService;
        private IDiscountCodeService _discountCodeService;
        private IImportCouponService _importCouponService;
        private IImportCouponDetailService _importCouponDetailService;
        private ISupplierService _supplierService;
        private IRoleService _roleService;
        private IDecentralizationService _decentralizationService;
        private IAccessTimesCountService _accessTimesCountService;




        public AdminController(IAccessTimesCountService accessTimesCountService, IDecentralizationService decentralizationService, IRoleService roleService, ISupplierService suplierService, IImportCouponDetailService importCouponDetailService, IImportCouponService importCouponService, IDiscountCodeService discountCodeService, IOrderService orderService, IQAService qAService, IProductService productService, IMemberService memberService, IEmployeeService employeeService, IEmployeeTypeService employeeTypeService)
        {
            _qAService = qAService;
            _productService = productService;
            _memberService = memberService;
            _employeeService = employeeService;
            _employeeTypeService = employeeTypeService;
            _orderService = orderService;
            _discountCodeService = discountCodeService;
            _importCouponService = importCouponService;
            _importCouponDetailService = importCouponDetailService;
            _supplierService = suplierService;
            _roleService = roleService;
            _decentralizationService = decentralizationService;
            _accessTimesCountService = accessTimesCountService;
        }

        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        // GET: Admin

        public ActionResult Index()
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.SumAccessTimes = HttpContext.Application["SumAccessTimes"].ToString();
                ViewBag.RealAccessTimes = HttpContext.Application["RealAccessTimes"].ToString();
                Employee employee = Session["Employee"] as Employee;
                ViewBag.EmloyeeTypeName = (_employeeTypeService.GetEmployeeTypeByID(employee.employeeTypeID.Value)).name;
                ViewBag.TotalMember = _memberService.GetTotalMember();
                ViewBag.TotalEmployee = _employeeService.GetTotalEmployee();
                ViewBag.TotalProduct = _productService.GetTotalProduct();
                ViewBag.TotalProductPurchased = _productService.GetTotalProductPurchased();
                decimal TotalRevenue = _orderService.GetTotalRevenue();
                if (TotalRevenue < 1000000)
                {
                    TotalRevenue = TotalRevenue / 1000;
                    ViewBag.TotalRevenue = TotalRevenue.ToString("0.##") + "K";
                }
                else
                {
                    TotalRevenue = TotalRevenue / 1000000;
                    ViewBag.TotalRevenue = TotalRevenue.ToString("0.##") + "M";
                }
                ViewBag.TotalOrder = _orderService.GetTotalOrder();
                return View();
            }

        }

        

        #region Login Logout Manage

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session["Employee"] = null;
            return RedirectToAction("Login");
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
            else
            {
                
                    ViewBag.Message = "Success";
                
            }
            return View("Page404");
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


        #endregion

        #region InforEmployee

        [HttpGet]
        public ActionResult InfoEmloyee()
        {
            Employee emloyee = Session["Employee"] as Employee;
            return View(emloyee);
        }
        [HttpGet]
        public ActionResult Edit()
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get emloyee
            Employee emloyee = Session["Employee"] as Employee;
            //Check null
            if (emloyee != null)
            {
                //Return view
                return Json(new
                {
                    ID = emloyee.id,
                    FullName = emloyee.fullName,
                    Address = emloyee.addresss,
                    Email = emloyee.email,
                    PhoneNumber = emloyee.phoneNumber,
                    Image = emloyee.imagee,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Return 404
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        public ActionResult Edit(Employee emloyee, HttpPostedFileBase ImageUpload)
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get data for DropdownList
            ViewBag.EmloyeeTypeIDEdit = new SelectList(_employeeTypeService.GetListEmployeeType().OrderBy(x => x.name), "id", "name");

            if (ImageUpload != null)
            {
                int errorCount = 0;
                //Check content image
                if (ImageUpload != null && ImageUpload.ContentLength > 0)
                {
                    //Check format iamge
                    if (ImageUpload.ContentType != "image/jpeg" && ImageUpload.ContentType != "image/png" && ImageUpload.ContentType != "image/gif")
                    {
                        //Set viewbag
                        ViewBag.upload += "Hình ảnh không hợp lệ<br/>";
                        //increase by 1 unit errorCount
                        errorCount++;
                    }
                    else
                    {
                        //Get file name
                        var fileName = Path.GetFileName(ImageUpload.FileName);
                        //Get path
                        var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                        //Check exitst
                        if (!System.IO.File.Exists(path))
                        {
                            //Add image into folder
                            ImageUpload.SaveAs(path);
                        }
                    }
                }
                //Set new value image for emloyee
                emloyee.imagee = ImageUpload.FileName;
            }
            //Set TempData for checking in view to show swal
            TempData["edit"] = "Success";
            //Update emloyeetype
            Employee e = _employeeService.GetByID(emloyee.id);
            e.fullName = emloyee.fullName;
            e.phoneNumber = emloyee.phoneNumber;
            e.addresss = emloyee.addresss;
            e.email = emloyee.email;
            e.imagee = emloyee.imagee;
            _employeeService.Update(e);
            Session["Employee"] = e;
            string Url = Request.Url.ToString();
            return RedirectToAction("InfoEmloyee");
        }
        [HttpGet]
        public ActionResult ResetPassword()
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(string CurrentPassword, string NewPassword)
        {
            Employee emloyee = Session["Employee"] as Employee;
            Employee emloyeeCheck = _employeeService.CheckLogin1(emloyee.id, CurrentPassword);
            if (emloyeeCheck != null)
            {
                _employeeService.ResetPassword(emloyeeCheck.id, NewPassword);
                TempData["ResetPassword"] = "Success";
                return RedirectToAction("InfoEmloyee");
            }
            else
            {
                ViewBag.Message = "Mật khẩu hiện tại không đúng!";
            }
            return View();
        }
        #endregion

        #region CategoryParent Manage
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

        #endregion

        #region Categories Manage

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

        #endregion

        #region Product Manage
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
            tbl.purchaseCount = 0;
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
        #endregion

        #region DiscountCode Manage
        public ActionResult DiscountCode()
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            IEnumerable<DiscountCode> discountCodes = _discountCodeService.GetDiscountCodeList();
            return View(discountCodes);
        }
        [HttpGet]
        public ActionResult AddDiscountCode()
        {


            return View();
        }
        [HttpPost]
        public ActionResult AddDiscountCode(DiscountCode discountCode, int Quantity)
        {
            Employee emloyee = Session["Employee"] as Employee;
            discountCode.employeeID = emloyee.id;
            _discountCodeService.AddDiscountCode(discountCode, Quantity);
            TempData["create"] = "success";
            return RedirectToAction("DiscountCode");
        }

        #endregion

        #region Supplier Manage
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

        #endregion

        #region Producer Manage
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
        #endregion

        #region QA Manage
        public ActionResult QA()
        {


            //if (Session["Emloyee"] == null)
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            //int pageSize = 5;
            //Get qAs list
            IEnumerable<QA> qAs = _qAService.GetQAList().OrderBy(x => x.dateQuestion);
            //Check null
            if (qAs != null)
            {
                //Return view
                return View(qAs);
            }
            else
            {
                //return 404
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }

        [HttpGet]
        public ActionResult EditQA(int id)
        {
            if (id == null)
            {
                return null;
            }
            //Get product catetgory
            var qAs = _qAService.GetQAByID(id);
            if (qAs == null)
            {
                return null;
            }
            return Json(new
            {
                ID = qAs.id,
                MemberID = qAs.memberID,
                ProductID = qAs.productID,
                Question = qAs.question,
                Answer = qAs.answer,
                status = true,
                DateQuestion = qAs.dateQuestion
            }, JsonRequestBehavior.AllowGet); ;
        }
        [HttpPost]
        public ActionResult EditQA(QA qA, string DateQuestion)
        {
            qA.dateQuestion = DateTime.Now;
            Employee emloyee = Session["Employee"] as Employee;
            qA.employeeID = emloyee.id;
            _qAService.UpdateQA(qA);
            return RedirectToAction("QA");
        }
        [HttpPost]
        public ActionResult Answer(QA qA, string DateQuestion)
        {
            qA.dateQuestion = DateTime.Now;
            Employee emloyee = Session["Employee"] as Employee;
            qA.employeeID = emloyee.id;
            _qAService.UpdateQA(qA);
            return RedirectToAction("QA");
        }
        #endregion

        #region ImportManage
        //Import 

        [HttpGet]
        public ActionResult ImportProduct()
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.listSupplier = _supplierService.GetSupplierList();
            return View();
        }
        [HttpGet]
        public ActionResult ImportProductBySupplierID(int ID)
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.listSupplier = _supplierService.GetSupplierList();
            ViewBag.SupplierID = ID;
            ViewBag.SupplierName = _supplierService.GetByID(ID).name;
            ViewBag.ListProduct = _productService.GetListProduct().OrderBy(x => x.quantity.Value);
            return View();
        }
        [HttpPost]
        public ActionResult ImportProduct(DAL.ImportCoupon Model, IEnumerable<DAL.ImportCouponDetail> ListModel)
        {
            ViewBag.listSupplier = _supplierService.GetSupplierList();
            ViewBag.ListProduct = _productService.GetListProduct();
            Employee emloyee = Session["Employee"] as Employee;
            Model.employeeID = emloyee.id;
            Model.datee = DateTime.Now;
            Model.isDelete = false;
            //Add import coupon
            _importCouponService.AddImportCoupon(Model);
            //Update quantity product
            Product product;
            foreach (var item in ListModel)
            {
                //Set ImportCouponID for all ImportCouponDetail
                item.importCouponID = Model.id;
                //Update quanitty number
                product = _productService.GetProductID(item.productID.Value);
                product.quantity += item.quantity;
                _productService.UpdateProduct(product);

                _importCouponDetailService.AddImportCouponDetail(item);
            }
            //Set TempData for checking in view to show swal
            TempData["ImportProduct"] = "Success";
            return View();
        }

        [HttpGet]
        public ActionResult ImportCoupon(int page = 1)
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var LstImportCoupon = _importCouponService.GetImportCoupon();
            PagedList<DAL.ImportCoupon> listImportCoupon = new PagedList<DAL.ImportCoupon>(LstImportCoupon, page, 10);
            ViewBag.Page = page;
            return View(listImportCoupon);
        }
        [HttpGet]
        public ActionResult ImportProductDetail(int ID)
        {
            IEnumerable<DAL.ImportCouponDetail> importCouponDetails = _importCouponDetailService.GetByImportCouponID(ID);
            ViewBag.ID = ID;
            return View(importCouponDetails);
        }

        [HttpGet]
        public ActionResult Delete(int ID, int page)
        {
            _importCouponService.Delete(ID);
            return RedirectToAction("ImportCoupon", new { page = page });
        }
        [HttpGet]
        public ActionResult Rehibilitate(int ID, int page)
        {
            _importCouponService.Rehibilitate(ID);
            return RedirectToAction("ImportCoupon", new { page = page });
        }
        public ActionResult Incompetent()
        {
            return View();
        }
        #endregion

        #region UserManage
        public ActionResult EmployeeManage()
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.EmployeeTypeID = new SelectList(_employeeTypeService.GetListEmployeeType().OrderBy(x => x.name), "id", "name");

            ViewBag.EmployeeTypeIDEdit = ViewBag.EmployeeTypeID;

            ViewBag.EmployeeTypeIDDetail = ViewBag.EmployeeTypeID;
            IEnumerable<Employee> employees = _employeeService.GetList();
            return View(employees);
        }

        public ActionResult AddEmployee()
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            List<SelectListItem> listEmployeeType = _employeeService.GetEmployeeType();
            ViewBag.ListTypeEmployee = listEmployeeType;
            return View();
        }
        [HttpPost]
        public ActionResult AddEmployee(Employee employee, HttpPostedFileBase ImageUpload)
        {
            
            int errorCount = 0;
            //Check content image
            if (ImageUpload != null && ImageUpload.ContentLength > 0)
            {
                //Check format iamge
                if (ImageUpload.ContentType != "image/jpeg" && ImageUpload.ContentType != "image/png" && ImageUpload.ContentType != "image/gif")
                {
                    //Set viewbag
                    ViewBag.upload += "Hình ảnh không hợp lệ<br/>";
                    //increase by 1 unit errorCount
                    errorCount++;
                }
                else
                {
                    //Get file name
                    var fileName = Path.GetFileName(ImageUpload.FileName);
                    //Get path
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    //Check exitst
                    if (!System.IO.File.Exists(path))
                    {
                        //Add image into folder
                        ImageUpload.SaveAs(path);
                    }
                }
            }
            //Set new value image for emloyee
            employee.imagee = ImageUpload.FileName;
            TempData["create"] = "Success";

            _employeeService.AddEmployee(employee);
            return RedirectToAction("EmployeeManage");
        }

        [HttpGet]
        public ActionResult EditEmployee(int id)
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get emloyee
            var emloyee = _employeeService.GetByID(id);

            //Get data for DropdownList
            ViewBag.EmployeeTypeIDEdit = new SelectList(_employeeTypeService.GetListEmployeeType().OrderBy(x => x.name), "id", "name", emloyee.employeeTypeID);

            //Check null
            if (emloyee != null)
            {
                //Return view
                return Json(new
                {
                    ID = emloyee.id,
                    FullName = emloyee.fullName,
                    UserName= emloyee.userName,
                    Address = emloyee.addresss,
                    Email = emloyee.email,
                    PhoneNumber = emloyee.phoneNumber,
                    Image = emloyee.imagee,
                    EmloyeeTypeID = emloyee.employeeTypeID,
                    IsActive = emloyee.isActive,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Return 404
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        public ActionResult EditEmployee(Employee emloyee, HttpPostedFileBase ImageUpload, int EmployeeTypeIDEdit)
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get data for DropdownList
            ViewBag.EmployeeTypeIDEdit = new SelectList(_employeeTypeService.GetListEmployeeType().OrderBy(x => x.name), "id", "name");

            if (ImageUpload != null)
            {
                int errorCount = 0;
                //Check content image
                if (ImageUpload != null && ImageUpload.ContentLength > 0)
                {
                    //Check format iamge
                    if (ImageUpload.ContentType != "image/jpeg" && ImageUpload.ContentType != "image/png" && ImageUpload.ContentType != "image/gif")
                    {
                        //Set viewbag
                        ViewBag.upload += "Hình ảnh không hợp lệ<br/>";
                        //increase by 1 unit errorCount
                        errorCount++;
                    }
                    else
                    {
                        //Get file name
                        var fileName = Path.GetFileName(ImageUpload.FileName);
                        //Get path
                        var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                        //Check exitst
                        if (!System.IO.File.Exists(path))
                        {
                            //Add image into folder
                            ImageUpload.SaveAs(path);
                        }
                    }
                }
                //Set new value image for emloyee
                emloyee.imagee = ImageUpload.FileName;
            }
            //Set TempData for checking in view to show swal
            TempData["edit"] = "Success";
            //Update emloyeetype
            Employee e = _employeeService.GetByID(emloyee.id);
            e.fullName = emloyee.fullName;
            e.addresss = emloyee.addresss;
            e.email = emloyee.email;
            e.imagee = emloyee.imagee;
            e.employeeTypeID = EmployeeTypeIDEdit;
            _employeeService.Update(e);
          
            return RedirectToAction("EmployeeManage");
        }
        public void Block(int id)
        {
            //Get emloyee by ID
            var emloyee = _employeeService.GetByID(id);
            _employeeService.Block(emloyee);
        }
        public void Active(int id)
        {
            //Get emloyee by ID
            var emloyee = _employeeService.GetByID(id);
            _employeeService.Active(emloyee);
        }
        #endregion

        #region Phân quyền
        public ActionResult DecentralizationEmployeeType(int page = 1)
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            IEnumerable<EmployeeType> emloyeeTypes = _employeeTypeService.GetListEmployeeType();
            PagedList<EmployeeType> emloyeeTypesList = new PagedList<EmployeeType>(emloyeeTypes, page, 10);
            return View(emloyeeTypesList);
        }
        [HttpGet]
        public ActionResult Decentralization(int id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            EmployeeType emloyeeType = _employeeTypeService.GetEmployeeTypeByID(id);
            if (emloyeeType == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleList = _roleService.GetRoleList();
            ViewBag.ListDecentralization = _decentralizationService.GetDecentralizationByEmloyeeTypeID(id);
            return View(emloyeeType);
        }
        [HttpPost]
        public ActionResult Decentralization(int EmloyeeTypeID, IEnumerable<Decentralization> decentralizations)
        {
            //Trường hợp: Nếu đã tiến hành phân quyền rồi nhưng muốn phân quyền lại
            //Bước 1: Xóa những quyền cũ thuộc loại tv đó
            var ListDecentralization = _decentralizationService.GetDecentralizationByEmloyeeTypeID(EmloyeeTypeID);
            if (ListDecentralization.Count() != 0)
            {
                _decentralizationService.RemoveRange(ListDecentralization);
            }
            if (decentralizations != null)
            {
                //Kiểm tra danh sách quyền được check
                foreach (var item in decentralizations)
                {
                    item.employeeTypeID = EmloyeeTypeID;
                    //Nếu được check thì insert dữ liệu vào bảng phân quyền
                    _decentralizationService.Add(item);
                }
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Statistic

        [Authorize(Roles = "StatisticManage")]
        [HttpGet]
        public ActionResult StatisticManage()
        {
            return View();
        }
        [Authorize(Roles = "StatisticStocking")]
        [HttpGet]
        public ActionResult StatisticStocking()
        {
            IEnumerable<Product> products = _productService.GetProductListStocking();
            return View(products);
        }
        [HttpGet]
        public void DownloadExcelStatisticStocking()
        {
            Employee emloyee = Session["Employee"] as Employee;

            IEnumerable<Product> products = _productService.GetProductListStocking();
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Người lập";
            ws.Cells["B2"].Value = emloyee.fullName;

            ws.Cells["A3"].Value = "Ngày lập";
            ws.Cells["B3"].Value = DateTime.Now.ToShortDateString();

            ws.Cells["A6"].Value = "Mã SP";
            ws.Cells["B6"].Value = "Tên SP";
            ws.Cells["C6"].Value = "Số lượng tồn";

            int rowStart = 7;
            foreach (var item in products)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.id;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.name;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.quantity;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Danh sách tồn kho.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
        [Authorize(Roles = "StatisticMember")]
        [HttpGet]
        public ActionResult StatisticMember()
        {
            IEnumerable<Member> members = _memberService.GetMemberListForStatistic();
            return View(members);
        }
        [HttpGet]
        public void DownloadExcelStatisticMember()
        {
            Employee emloyee = Session["Employee"] as Employee;

            IEnumerable<Member> members = _memberService.GetMemberListForStatistic();
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Người lập";
            ws.Cells["B2"].Value = emloyee.fullName;

            ws.Cells["A3"].Value = "Ngày lập";
            ws.Cells["B3"].Value = DateTime.Now.ToShortDateString();

            ws.Cells["A6"].Value = "Mã Thành Viên";
            ws.Cells["B6"].Value = "Tên Thành Viên";
            ws.Cells["C6"].Value = "Địa Chỉ";
            ws.Cells["D6"].Value = "Email";
            ws.Cells["E6"].Value = "Số Điện Thoại";
            ws.Cells["F6"].Value = "Loại Thành Viên";
            ws.Cells["G6"].Value = "Doanh Số";

            int rowStart = 7;
            foreach (var item in members)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.id;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.fullName;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.addresss;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.email;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.phoneNumber;
                if (item.memberCategoryID == 1)
                    ws.Cells[string.Format("F{0}", rowStart)].Value = "Thường";
                else
                {
                    ws.Cells[string.Format("F{0}", rowStart)].Value = "VIP";
                }
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.amountPurchased;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Danh sách thành viên.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
        [Authorize(Roles = "StatisticSupplier")]
        [HttpGet]
        public ActionResult StatisticSupplier()
        {
            IEnumerable<Supplier> suppliers = _supplierService.GetSupplierList();
            return View(suppliers);
        }
        [HttpGet]
        public void DownloadExcelStatisticSupplier()
        {
            Employee emloyee = Session["Employee"] as Employee;

            IEnumerable<Supplier> suppliers = _supplierService.GetSupplierList();
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Người lập";
            ws.Cells["B2"].Value = emloyee.fullName;

            ws.Cells["A3"].Value = "Ngày lập";
            ws.Cells["B3"].Value = DateTime.Now.ToShortDateString();

            ws.Cells["A6"].Value = "Mã Nhà Cung Cấp";
            ws.Cells["B6"].Value = "Tên Nhà Cung Cấp";
            ws.Cells["C6"].Value = "Địa Chỉ";
            ws.Cells["D6"].Value = "Email";
            ws.Cells["E6"].Value = "Số Điện Thoại";
            ws.Cells["F6"].Value = "Tình Trạng";
            ws.Cells["G6"].Value = "Doanh Số";

            int rowStart = 7;
            foreach (var item in suppliers)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.id;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.name;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.addresss;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.email;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.phoneNumber;
                if (item.isActive.Value)
                    ws.Cells[string.Format("F{0}", rowStart)].Value = "Đang hợp tác";
                else
                {
                    ws.Cells[string.Format("F{0}", rowStart)].Value = "Đã ngừng hợp tác";
                }
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.totalAmount;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Nhà cung cấp tốt nhất.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
        [Authorize(Roles = "StatisticProductSold")]
        [HttpGet]
        public ActionResult StatisticProductSold(DateTime from, DateTime to)
        {
            IEnumerable<Product> products = _productService.GetProductListSold(from, to);
            ViewBag.from = from;
            ViewBag.to = to;
            return View(products);
        }
        [HttpGet]
        public void DownloadExcelStatisticProductSold(DateTime from, DateTime to)
        {
            Employee emloyee = Session["Employee"] as Employee;

            IEnumerable<Product> products = _productService.GetProductListSold(from, to);
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Người lập";
            ws.Cells["B2"].Value = emloyee.fullName;

            ws.Cells["A3"].Value = "Ngày lập";
            ws.Cells["B3"].Value = DateTime.Now.ToShortDateString();

            ws.Cells["A6"].Value = "Mã SP";
            ws.Cells["B6"].Value = "Tên SP";
            ws.Cells["C6"].Value = "Só Lượng Đã Bán";

            int rowStart = 7;
            foreach (var item in products)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.id;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.name;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.purchaseCount;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Sản phẩm đã bán.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
        [Authorize(Roles = "StatisticOrder")]
        [HttpGet]
        public ActionResult StatisticOrder(DateTime from, DateTime to)
        {
            IEnumerable<OrderShip> orders = _orderService.GetListOrderStatistic(from, to);
            ViewBag.from = from;
            ViewBag.to = to;
            return View(orders);
        }
        [HttpGet]
        public void DownloadExcelStatisticOrder(DateTime from, DateTime to)
        {
            Employee emloyee = Session["Employee"] as Employee;

            IEnumerable<OrderShip> orders = _orderService.GetListOrderStatistic(from, to);
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Người lập";
            ws.Cells["B2"].Value = emloyee.fullName;

            ws.Cells["A3"].Value = "Ngày lập";
            ws.Cells["B3"].Value = DateTime.Now.ToShortDateString();

            ws.Cells["A6"].Value = "Mã Hóa Đơn";
            ws.Cells["B6"].Value = "Tên Khách Hàng Thành Viên";
            ws.Cells["C6"].Value = "Ngày Đặt";
            ws.Cells["D6"].Value = "Ngày Giao";
            ws.Cells["E6"].Value = "Ưu Đãi";
            ws.Cells["F6"].Value = "Tình Trạng";
            ws.Cells["G6"].Value = "Thành Tiền";

            int rowStart = 7;
            foreach (var item in orders)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.id;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.Customer.fullName;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.dateOrder;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.dateShip;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.offer + "%";
                if (item.isReceived.Value)
                    ws.Cells[string.Format("F{0}", rowStart)].Value = "Hoàn thành";
                else
                {
                    ws.Cells[string.Format("F{0}", rowStart)].Value = "Chưa hoàn thành";
                }
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.total;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Đơn đặt hàng.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
        [Authorize(Roles = "StatisticAccessTime")]
        [HttpGet]
        public ActionResult StatisticAccessTime(DateTime from, DateTime to)
        {
            IEnumerable<AccessTimeCount> accessTimesCounts = _accessTimesCountService.GetListAccessTimeCountStatistic(from, to);
            ViewBag.from = from;
            ViewBag.to = to;
            return View(accessTimesCounts);
        }
        [HttpGet]
        public void DownloadExcelStatisticStatisticAccessTime(DateTime from, DateTime to)
        {
            Employee emloyee = Session["Employee"] as Employee;

            IEnumerable<AccessTimeCount> accessTimesCounts = _accessTimesCountService.GetListAccessTimeCountStatistic(from, to);
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Người lập";
            ws.Cells["B2"].Value = emloyee.fullName;

            ws.Cells["A3"].Value = "Ngày lập";
            ws.Cells["B3"].Value = DateTime.Now.ToShortDateString();

            ws.Cells["A6"].Value = "Ngày";
            ws.Cells["B6"].Value = "Số Lượng Truy Cập";

            int rowStart = 7;
            foreach (var item in accessTimesCounts)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.datee.Value.ToShortDateString();
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.acessTime;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Số lượng truy cập.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
        #endregion

        #region Developing
        public ActionResult MailBox()
        {
            return View();
        }
        public ActionResult Charts()
        {
            return View();
        }
        public ActionResult MemberManage()
        {
            return View();
        }
        public ActionResult Customer()
        {
            return View();
        }
        public ActionResult Page404()
        {
            return View();
        }
        #endregion
    }
}