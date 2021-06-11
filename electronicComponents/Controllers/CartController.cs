using electronicComponents.DAL;
using electronicComponents.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace electronicComponents.Controllers
{
    public class CartController : Controller
    {
        private ICartService _cartService;
        private IProductService _productService;
        private IDiscountCodeDetailService _discountCodeDetailService;
        private IDiscountCodeService _discountCodeService;
        private ICustomerService _customerService;
        private IOrderService _orderService;
        private IOrderDetailService _orderDetailService;
        public CartController(ICartService cartService, IProductService productService, IDiscountCodeDetailService discountCodeDetailService, IDiscountCodeService discountCodeService,ICustomerService customerService, IOrderService orderService, IOrderDetailService orderDetailService)
        {

            _cartService = cartService;
            _productService = productService;
            _discountCodeDetailService=discountCodeDetailService;
            _discountCodeService =discountCodeService;
            _customerService = customerService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;

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
                    itemCart.productID = productAdd.id;
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
                        return RedirectToAction("CartPartial");
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
            try
            {
                ViewBag.DiscountCodeDetailListByMemer = _discountCodeDetailService.GetDiscountCodeDetailListByMember(member.id);
                return View();
            }
            catch (Exception)
            {
                return View();
            }
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
            return View("Checkout");
        }

        [HttpPost]
        public ActionResult CheckQuantityAdd(int ID)
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
            //Check quantity
            if (listCart != null)
            {
                int sum = 0;
                foreach (Cart item in listCart.Where(x => x.productID == ID))
                {
                    sum += item.quantity.Value;
                }
                if (product.quantity > sum)
                {
                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    });
                }
            }
            else
            {
                if (product.quantity > 0)
                {

                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    });
                }
            }
        }
        [HttpPost]
        public ActionResult CheckQuantityUpdate(int ID, int Quantity)
        {
            //Check product already exists in DB
            Product product = _productService.GetProductID(ID);
            if (product.quantity >= Quantity)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }
        [HttpPost]
        public ActionResult AddOrder(Customer customer, int NumberDiscountPass = 0, string CodePass = "")
        {
            //Check null session cart
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Customer customercheck = new Customer();
            bool status = false;
            //Is Customer
            Customer customerNew = new Customer();
            if (Session["Member"] == null)
            {
                //Insert customer into DB
                customerNew = customer;
                customerNew.isMember = false;
                _customerService.AddCustomer(customerNew);
            }
            else
            {
                //Is member
                Member member = Session["Member"] as Member;
                customercheck = _customerService.GetAll().FirstOrDefault(x => x.fullName.Contains(member.fullName));
                if (customercheck != null)
                {
                    status = true;
                }
                else
                {
                    customerNew.fullName = member.fullName;
                    customerNew.addresss = member.addresss;
                    customerNew.email = member.email;
                    customerNew.phoneNumber = member.phoneNumber;
                    customerNew.isMember = true;
                    _customerService.AddCustomer(customerNew);
                }
            }
            //Add order
            OrderShip order = new OrderShip();
            if (status)
            {
                order.customerID = customercheck.id;
            }
            else
            {
                order.customerID = customerNew.id;
            }
            order.dateOrder = DateTime.Now;
            order.dateShip = DateTime.Now.AddDays(3);
            order.isPaid = false;
            order.isDelete = false;
            order.isDelivere = false;
            order.isApproved = false;
            order.isReceived = false;
            order.isCancel = false;
            order.offer = NumberDiscountPass;
            _orderService.AddOrder(order);
            //Add order detail
            List<Cart> listCart = GetCart();
            decimal sumtotal = 0;
            foreach (Cart item in listCart)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.orderID = order.id;
                orderDetail.productID = item.productID;
                orderDetail.quantity = item.quantity;
                orderDetail.price = item.price;
                _orderDetailService.AddOrderDetail(orderDetail);
                sumtotal += orderDetail.quantity.Value * orderDetail.price.Value;
                if (Session["Member"] != null)
                {
                    //Remove Cart
                    _cartService.RemoveCart(item.productID.Value, item.memberID.Value);
                }
            }
            if (NumberDiscountPass != 0)
            {
                _orderService.UpdateTotal(order.id, sumtotal - (sumtotal / 100 * NumberDiscountPass));
            }
            else
            {
                _orderService.UpdateTotal(order.id, sumtotal);
            }
            if (CodePass != "")
            {
                //Set discountcode used
                _discountCodeDetailService.Used(CodePass);
            }
            Session["Code"] = null;
            Session["num"] = null;
            Session["Cart"] = null;
            if (status)
            {
                SentMail("Đặt hàng thành công", customercheck.email, "haichau0818@gmail.com", "id0ntkn0w", "<p style=\"font-size:20px\">Cảm ơn bạn đã đặt hàng<br/>Mã đơn hàng của bạn là: " + order.id + "</p>");
            }
            else
            {
                SentMail("Đặt hàng thành công", customerNew.email, "haichau0818@gmail.com", "id0ntkn0w", "<p style=\"font-size:20px\">Cảm ơn bạn đã đặt hàng<br/>Mã đơn hàng của bạn là: " + order.id + "</p>");
            }
            return RedirectToAction("Message");
        }
        [HttpPost]
        public ActionResult EditCart(int ID, int Quantity)
        {
            //Check stock quantity
            Product product = _productService.GetProductID(ID);
            //Updated quantity in cart Session
            List<Cart> listCart = GetCart();
            //Get products from within listCart to update
            Cart itemCartUpdate = listCart.Find(n => n.productID == ID);
            itemCartUpdate.quantity = Quantity;
            itemCartUpdate.total = itemCartUpdate.quantity * itemCartUpdate.price;

            Member member = Session["Member"] as Member;
            if (member != null)
            {
                //Update Cart Quantity Member
                _cartService.UpdateQuantityCartMember(Quantity, ID, member.id);
                Session["Cart"] = listCart;
            }

            return RedirectToAction("Checkout");
        }
        public ActionResult Message()
        {
            return View();
        }

        public void SentMail(string Title, string ToEmail, string FromEmail, string Password, string Content)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail);
            mail.From = new MailAddress(ToEmail);
            mail.Subject = Title;
            mail.Body = Content;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(FromEmail, Password);
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        [HttpPost]
        public ActionResult Choose(string Code, string CodeInput)
        {
            if (CodeInput != "")
            {
                int numcheck = _discountCodeDetailService.GetDiscountByCode(CodeInput);
                if (numcheck != null)
                {
                    Session["Code"] = CodeInput;
                    Session["num"] = numcheck;
                }
                else
                {
                    int num = _discountCodeDetailService.GetDiscountByCode(Code);
                    Session["Code"] = Code;
                    Session["num"] = num;
                }
            }
            else
            {
                int num = _discountCodeDetailService.GetDiscountByCode(Code);
                Session["Code"] = Code;
                Session["num"] = num;
            }
            return RedirectToAction("Checkout");
        }
        public ActionResult CancelDiscount()
        {
            Session["Code"] = null;
            Session["num"] = null;
            return RedirectToAction("Checkout");
        }
    }
}