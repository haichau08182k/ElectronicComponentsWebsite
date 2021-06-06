using electronicComponents.DAL;
using electronicComponents.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace electronicComponents.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _productService;
        //private IProducerService _producerService;
        //private ISupplierService _supplierService;
        //private IProductCategoryService _productCategoryService;
     
        //private IProductCategoryParentService _productCategoryParentService;
   
        private IMemberService _memberService;
        private IQAService _qaService;
        private IEmployeeService _emloyeeService;
        //private IProductViewedService _productViewedService;
        private IRatingService _ratingService;
        private IOrderDetailService _orderDetailService;

        public ProductController(IProductService productService, /*IProducerService producerService*/ IMemberService memberService, IQAService qAService, IRatingService ratingService, IOrderDetailService orderDetailService,IEmployeeService emloyeeService)
        {
            _productService = productService;
           // _producerService = producerService;
            //_supplierService = supplierService;
            //_productCategoryService = productCategoryService;
            //_ageService = ageService;
            //_productCategoryParentService = productCategoryParentService;
            //_genderService = genderService;
            _memberService = memberService;
            _qaService = qAService;
            _emloyeeService = emloyeeService;
            //_productViewedService = productViewedService;
            _ratingService = ratingService;
            _orderDetailService = orderDetailService;
        }
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details(int ID)
        {
            var product = _productService.GetProductID(ID);
            //var listProduct = _productService.GetProductListByCategory(product.CategoryID);
            //ViewBag.ListProduct = listProduct;
            IEnumerable<QA> listQA = _qaService.GetQAByProductID(ID).OrderByDescending(x => x.dateQuestion);
            ViewBag.CommentQA = listQA;
            IEnumerable<Member> listMember = _memberService.GetMemberList();
            ViewBag.MemberList = listMember;

            if (Session["Member"] != null)
            {
                Member member = Session["Member"] as Member;
                _productService.AddProductViewByMember(ID, member.id);
            }
            //Add view count
            if (Request.Cookies["ViewedPage"] != null)
            {
                if (Request.Cookies["ViewedPage"][string.Format("ID_{0}", ID)] == null)
                {
                    HttpCookie cookie = (HttpCookie)Request.Cookies["ViewedPage"];
                    cookie[string.Format("ID_{0}", ID)] = "1";
                    cookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(cookie);

                    _productService.AddViewCount(ID);
                }
            }
            else
            {
                HttpCookie cookie = new HttpCookie("ViewedPage");
                cookie[string.Format("ID_{0}", ID)] = "1";
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(cookie);

                _productService.AddViewCount(ID);
            }
            //Get rating
            ViewBag.Rating = _ratingService.GetRating(ID);
            //Get list rating
            ViewBag.ListRating = _ratingService.GetListRating(ID);
            return View(product);
        }
        [HttpGet]
        public ActionResult AddQuestion(int productID, int memberID, string Question)
        {
            QA qa = new QA();
            qa.memberID = memberID;
            qa.question = Question;
            qa.productID = productID;
            qa.dateQuestion = DateTime.Now;
            qa.dateAnswer = DateTime.Now;
            qa.employeeID = 1;
            _qaService.AddQA(qa);

            IEnumerable<QA> listQA = _qaService.GetQAByProductID(productID).OrderByDescending(x => x.dateQuestion);
            ViewBag.QAList = listQA;
            IEnumerable<Member> listMember = _memberService.GetMemberList();
            ViewBag.MemberList = listMember;
            return PartialView("_QAPartial");
        }
    }
}