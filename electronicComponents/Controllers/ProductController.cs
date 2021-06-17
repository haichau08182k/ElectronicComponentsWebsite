using electronicComponents.DAL;
using electronicComponents.Service;
using PagedList;
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


        private IMemberService _memberService;
        private IQAService _qaService;
        private IEmployeeService _emloyeeService;
        private IRatingService _ratingService;
        private IOrderDetailService _orderDetailService;

        public ProductController(IProductService productService, /*IProducerService producerService*/ IMemberService memberService, IQAService qAService, IRatingService ratingService, IOrderDetailService orderDetailService, IEmployeeService emloyeeService)
        {
            _productService = productService;
            // _producerService = producerService;
            //_supplierService = supplierService;
            _memberService = memberService;
            _qaService = qAService;
            _emloyeeService = emloyeeService;
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

            var ProSelling = _productService.GetListFeaturedProduct();
            ViewBag.ListProSelling = ProSelling;
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
        [HttpPost]
        public ActionResult AddQuestion(int productID, int memberID, string Question)
        {
            QA qa = new QA();
            qa.memberID = memberID;
            qa.question = Question;
            qa.productID = productID;
            qa.dateQuestion = DateTime.Now;
            qa.dateAnswer = DateTime.Now;
            qa.employeeID = 1;
            qa.statuss = false;
            _qaService.AddQA(qa);

            IEnumerable<QA> listQA = _qaService.GetQAByProductID(productID).OrderByDescending(x => x.dateQuestion);
            ViewBag.QAList = listQA;
            IEnumerable<Member> listMember = _memberService.GetMemberList();
            ViewBag.MemberList = listMember;
            return PartialView("_QAPartial");

        }

        public ActionResult ProductPartial(Product product)
        {

            ViewBag.Rating = _ratingService.GetRating(product.id);
            return PartialView(product);
        }
        public ActionResult ProductFeaturePartial(Product product)
        {
            ViewBag.Rating = _ratingService.GetRating(product.id);
            var products = _productService.GetListFeaturedProduct();
            ViewBag.ProSelling = products;
            return PartialView(product);
        }

        public ActionResult ProductNewPartial(Product product)
        {

            ViewBag.Rating = _ratingService.GetRating(product.id);
            return PartialView(product);
        }

        public ActionResult NewProduct(int page = 1 )
        {
            var listProduct = _productService.GetListProduct().OrderByDescending(x => x.viewCount).Take(5);
            ViewBag.ListProduct = listProduct;
            var ProSelling = _productService.GetListFeaturedProduct();
            ViewBag.ListProSelling = ProSelling;

            PagedList<Product> listProductPaging;
            IEnumerable<Product> products = _productService.GetListProductNew();
            listProductPaging = new PagedList<Product>(products, page, 12);

            

            return View(listProductPaging);
        }

        public ActionResult ProFeatured(int page = 1)
        {

            var ProNew = _productService.GetListProductNew();
            ViewBag.ListNew = ProNew;

            PagedList<Product> listProductPaging;
            IEnumerable<Product> products = _productService.GetListFeaturedProduct();
            listProductPaging = new PagedList<Product>(products, page, 12);

            return View(listProductPaging);
        }
        public ActionResult ProSelling(int page = 1)
        {
           
            var ProFeatured = _productService.GetListFeaturedProduct();
            ViewBag.ProFeatured = ProFeatured;

            PagedList<Product> listProductPaging;
            IEnumerable<Product> products = _productService.GetListSellingProduct();
            listProductPaging = new PagedList<Product>(products, page, 12);

            return View(listProductPaging);
        }

        public ActionResult ProductCategory(int ID, int page =1)
        {
            var listProduct = _productService.GetListProduct().OrderByDescending(x => x.viewCount).Take(5);
            ViewBag.ListProduct = listProduct;

            var ProSelling = _productService.GetListFeaturedProduct();
            ViewBag.ListProSelling = ProSelling;

            ViewBag.productCategoryID = ID;
            ProductCategory productCategory = _productService.GetProductCateID(ID);
            ViewBag.Name = "Danh mục " + productCategory.name;

            PagedList<Product> listProductPaging;
            IEnumerable<Product> products = _productService.GetProductListByCategory(ID);
            listProductPaging = new PagedList<Product>(products, page, 9);
            return View(listProductPaging);
        }



        public ActionResult Search(string keyword, int page = 1)
        {
            var listProduct = _productService.GetProductList(keyword);
            ViewBag.ListProduct = listProduct;
            if (keyword == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ProSelling = _productService.GetListFeaturedProduct();
            ViewBag.ListProSelling = ProSelling;

            ViewBag.Keyword = keyword;
            var products = _productService.GetProductList(keyword);
            PagedList<Product> listProductSearch = new PagedList<Product>(products, page, 12);
            return View(listProductSearch);
        }

        public PartialViewResult FilterProductList(string type, int ID, int min = 0, int max = 0, int discount = 0, int page = 1)
        {
            PagedList<Product> listProductPaging = null;
            if (type == "Ages")
            {
                //    //ViewBag.Name = "Độ tuổi " + _ageService.GetAgeByID(ID).Name;
                //    IEnumerable<Product> products = _productService.GetProductFilterByAges(ID, min, max, discount);
                //    listProductPaging = new PagedList<Product>(products, page, 2);
            }

        ViewBag.Min = min;
            ViewBag.Max = max;
            ViewBag.Type = type;
            ViewBag.ID = ID;
            //Check null
            if (listProductPaging != null)
            {
                //Return view
                return PartialView("ProductContainerPartial", listProductPaging);
            }
            else
            {
                //return 404
                return null;
            }
        }


        public ActionResult ProductViewed(int page = 1)
        {
            var listProduct = _productService.GetListProduct().OrderByDescending(x => x.viewCount).Take(5);
            ViewBag.ListProduct = listProduct;

            Member member = Session["Member"] as Member;
            PagedList<Product> listProductPaging;
            IEnumerable<Product> products = _productService.GetProductListViewedByMemberID(member.id);
            listProductPaging = new PagedList<Product>(products, page, 10);
            return View(listProductPaging);
        }
        public ActionResult DeleteHistoryView()
        {
            Member member = Session["Member"] as Member;
            _productService.Delete(member.id);
            TempData["DeleteHistoryView"] = "Sucess";
            return RedirectToAction("ProductViewed");
        }
    }
}