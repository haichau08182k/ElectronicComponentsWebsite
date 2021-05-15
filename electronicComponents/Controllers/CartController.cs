using electronicComponents.DAL;
using electronicComponents.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace electronicComponents.Controllers
{
    public class CartController : Controller
    {
        private  ICartService _cartService;
        private IProductService _productService;
        public CartController(ICartService cartService, IProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CartPartial()
        {
            if (GetTotalQuanity() == 0)
            {
                ViewBag.TotalQuanity = 0;
                ViewBag.TotalPrice = 0;
                return PartialView();
            }
            ViewBag.TotalQuanity = GetTotalQuanity();
            ViewBag.TotalPrice = GetTotalPrice().ToString("#,##");
            return PartialView();
        }
        public List<Cart> GetCart()
        {
            Member member = Session["Member"] as Member;
            if (member != null)
            {
                if (_cartService.CheckCartMember(member.id))
                {
                    List<Cart> listCart = _cartService.GetCart(member.id);
                    Session["Cart"] = listCart;
                    return listCart;
                }
            }
            else
            {
                List<Cart> listCart = Session["Cart"] as List<Cart>;
                //Check null session Cart
                if (listCart == null)
                {
                    //Initialization listCart
                    listCart = new List<Cart>();
                    Session["Cart"] = listCart;
                    return listCart;
                }
                return listCart;
            }
            return null;
        }
        public ActionResult AddItemInCart(int ID)
        {
            //Check product already exists in DB
            Product product = _productService.GetProductID(ID);
            if (product == null)
            {
                //product does not exist
                Response.StatusCode = 404;
                return null;
            }
            //Get cart
            List<Cart> listCart = GetCart();

            //If member
            Member member = Session["Member"] as Member;
            if (member != null)
            {
                //Case 1: If product already exists in Member Cart
                if (_cartService.CheckProductInCart(ID, member.id))
                {
                    _cartService.AddQuantityProductCartMember(ID, member.id);
                }
                else
                {
                    //Case 2: If product does not exist in Member Cart
                    //Get product
                    Product productAdd = _productService.GetProductID(ID);
                    Cart itemCart = new Cart();
                    itemCart.id = productAdd.id;
                    itemCart.price = (decimal)productAdd.promotionPrice;
                    itemCart.name = productAdd.name;
                    itemCart.quantity = 1;
                    itemCart.total = itemCart.price * itemCart.quantity;
                    itemCart.imagee = productAdd.image1;
                    _cartService.AddCartIntoMember(itemCart, member.id);
                }
                List<Cart> carts = _cartService.GetCart(member.id);
                Session["Cart"] = carts;
                ViewBag.TotalQuanity = GetTotalQuanity();
                ViewBag.TotalPrice = GetTotalPrice().ToString("#,##");
                return PartialView("CartPartial");
            }
            else
            {
                if (listCart != null)
                {
                    //Case 1: If product already exists in session Cart
                    Cart itemCart = listCart.SingleOrDefault(n => n.productID == ID);
                    if (itemCart != null)
                    {
                        //Check inventory before letting customers make a purchase
                        if (product.quantity < itemCart.quantity)
                        {
                            return View("ThongBao");
                        }
                        itemCart.quantity++;
                        itemCart.total = itemCart.quantity * itemCart.price;
                        ViewBag.TotalQuanity = GetTotalQuanity();
                        ViewBag.TotalPrice = GetTotalPrice().ToString("#,##");
                        return PartialView("CartPartial");
                    }

                    //Case 2: If product does not exist in the Session Cart
                    Cart item = new Cart(ID);
                    listCart.Add(item);
                }
            }
            ViewBag.TotalQuanity = GetTotalQuanity();
            ViewBag.TotalPrice = GetTotalPrice().ToString("#,##");
            return PartialView("CartPartial");
            
        }
        public double GetTotalQuanity()
        {
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart == null)
            {
                return 0;
            }
            return listCart.Sum(n => n.quantity.Value);
        }
        public decimal GetTotalPrice()
        {//Lấy giỏ hàng
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart == null)
            {
                return 0;
            }
            return listCart.Sum(n => n.total.Value);
        }
        public ActionResult Checkout()
        {
            ViewBag.TotalQuantity = GetTotalQuanity();
            Member member = Session["Member"] as Member;
            //ViewBag.DiscountCodeDetailListByMemer = _discountCodeDetailService.GetDiscountCodeDetailListByMember(member.ID);
            return View();
        }
        [HttpGet]
        public ActionResult RemoveItemCart(int ID)
        {
            //Check null session Cart
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Check whether the product exists in the database or not?
            Product product = _productService.GetProductID(ID);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Get cart
            List<Cart> listCart = GetCart();
            //Check if the product exists in the shopping cart
            Cart item = listCart.SingleOrDefault(n => n.productID == ID);
            if (item == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Remove item cart
            listCart.Remove(item);
            Member member = Session["Member"] as Member;
            if (member != null)
            {
                _cartService.RemoveCart(ID, member.id);
                List<Cart> carts = _cartService.GetCart(member.id);
                Session["Cart"] = carts;
            }
            ViewBag.TotalQuantity = GetTotalQuanity();
            return PartialView("CheckoutPartial");
        }

    }
}