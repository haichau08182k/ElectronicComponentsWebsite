using electronicComponents.DAL;
using electronicComponents.Repository;
using electronicComponents.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace electronicComponents.Controllers
{
    public class HomeController : Controller
    {
        private IMemberService _memberService;
        private IProductService _productService;
        private ICartService _cartService;
        private IOrderDetailService _orderDetailService;

        private IRatingService _ratingService;

        public HomeController(IMemberService memberService, IProductService productService, ICartService cartService, IOrderDetailService orderDetailService, IRatingService ratingService)
        {
            _memberService = memberService;
            _productService = productService;
            _cartService = cartService;
            _orderDetailService = orderDetailService;
            _ratingService = ratingService;
        }
        public ActionResult Index()
        {

            var listProductNew = _productService.GetListProductNew();
            ViewBag.listProductNew = listProductNew;
            

            return View();
        }
        [HttpPost]
        public ActionResult Login( Member member)
        {
            if (member == null)
            {
                return null;
            }
            else
            {
                Member memberCheck = _memberService.CheckLogin(member.userName, member.passwordd);
                if (memberCheck != null)
                {
                    Session["Member"] = memberCheck;
                    if (memberCheck.emailConfirmed == false)
                    {
                        return RedirectToAction("ConfirmEmail", "Member", new { ID = memberCheck.id });
                    }
                    else
                    {
                        if (_cartService.CheckCartMember(memberCheck.id))
                        {
                            List<Cart> carts = _cartService.GetCart(memberCheck.id);
                            Session["Cart"] = carts;
                            return RedirectToAction("Index");
                        }
                        if (Session["Cart"] != null)
                        {
                            List<Cart> listCart = Session["Cart"] as List<Cart>;
                            foreach (var item in listCart)
                            {
                                _cartService.AddCartIntoMember(item, memberCheck.id);
                            }
                        }
                    }
                }
                else
                {
                    ViewBag.Message = "Tên đăng nhập/Email hoặc mật khẩu không đúng.";
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult SignOut()
        {
            Session["Member"] = null;
            Session["Cart"] = null;
            return RedirectToAction("Index");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Member member)
        {
            if (ModelState.IsValid)
            {
                if (member == null)
                {
                    return null;
                }
                else
                {
                    Member member1 = _memberService.AddMember(member);
                    return RedirectToAction("ConfirmEmail", "Member", new { ID = member1.id });
                }
            }
            return null;
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult MenuPartial()
        {
            var listParent = _productService.GetProductCategoryParentList();
            ViewBag.listParent = listParent;
            var listCategories = _productService.GetProductCategoryList();
            ViewBag.listCategories = listCategories;
            return PartialView();
        }

        public ActionResult HeaderPartial()
        {
            return PartialView();
        }

       
        public ActionResult NewProductPartial()
        {
            var listProductNew = _productService.GetListProductNew();
            ViewBag.listProductNew = listProductNew;
            return PartialView();
        }
        public ActionResult FeaturedProductPartial()
        {
            var listProductSelling = _productService.GetListSellingProduct();
            ViewBag.listFeatured = listProductSelling;
            return PartialView();
        }
        [HttpPost]
        public ActionResult Rating(Rating rating, int OrderDetailID)
        {
            Member member = Session["Member"] as Member;
            rating.memberID = member.id;
            _ratingService.AddRating(rating);
            _orderDetailService.SetIsRating(OrderDetailID);
            string urlBase = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~");
            return Redirect(urlBase);
        }
    }
}