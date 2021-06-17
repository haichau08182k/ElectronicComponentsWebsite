using electronicComponents.DAL;
using electronicComponents.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace electronicComponents.Controllers
{
    public class CheckingController : Controller
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




        public CheckingController(IAccessTimesCountService accessTimesCountService, IDecentralizationService decentralizationService, IRoleService roleService, ISupplierService suplierService, IImportCouponDetailService importCouponDetailService, IImportCouponService importCouponService, IDiscountCodeService discountCodeService, IOrderService orderService, IQAService qAService, IProductService productService, IMemberService memberService, IEmployeeService employeeService, IEmployeeTypeService employeeTypeService)
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



        // GET: Checking
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CheckNameProduct(string name, int id = 0)
        {
            Product product = _productService.GetByName(name);
            if (product != null)
            {
                if (product.id == id)
                {
                    return Json(new
                    {
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (_productService.CheckName(name))
                    {
                        return Json(new
                        {
                            status = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            if (_productService.CheckName(name))
            {
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = false
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CheckNameProducer(string name, int id = 0)
        {
            Producer producer = _productService.GetByNameProducer(name);
            if (producer != null)
            {
                if (producer.id == id)
                {
                    return Json(new
                    {
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (_productService.CheckNameProducer(name))
                    {
                        return Json(new
                        {
                            status = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            if (_productService.CheckNameProducer(name))
            {
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = false
            }, JsonRequestBehavior.AllowGet);
        }

        #region Check Employee


        [HttpPost]
        public JsonResult CheckNameEmployee(string name, int id = 0)
        {
            Employee emloyee = _employeeService.GetByNameEmployee(name);
            if (emloyee != null)
            {
                if (emloyee.id == id)
                {
                    return Json(new
                    {
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (_employeeService.CheckNameEmployee(name))
                    {
                        return Json(new
                        {
                            status = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            if (_employeeService.CheckNameEmployee(name))
            {
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = false
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CheckUserNameEmployee(string name, int id = 0)
        {
            Employee emloyee = _employeeService.GetByUserNameEmployee(name);
            if (emloyee != null)
            {
                if (emloyee.id == id)
                {
                    return Json(new
                    {
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (_employeeService.CheckUserNameEmployee(name))
                    {
                        return Json(new
                        {
                            status = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            if (_employeeService.CheckUserNameEmployee(name))
            {
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = false
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CheckPhoneNumberEmployee(string phoneNumber, int id = 0)
        {
            Employee emloyee = _employeeService.GetByPhoneNumberEmployee(phoneNumber);
            if (emloyee != null)
            {
                if (emloyee.id == id)
                {
                    return Json(new
                    {
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (_employeeService.CheckPhoneNumberEmployee(phoneNumber))
                    {
                        return Json(new
                        {
                            status = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            if (_employeeService.CheckPhoneNumberEmployee(phoneNumber))
            {
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = false
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CheckEmailEmployee(string email, int id = 0)
        {
            Employee emloyee = _employeeService.GetByEmailEmployee(email);
            if (emloyee != null)
            {
                if (emloyee.id == id)
                {
                    return Json(new
                    {
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (_employeeService.CheckEmailEmployee(email))
                    {
                        return Json(new
                        {
                            status = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            if (_employeeService.CheckEmailEmployee(email))
            {
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = false
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}