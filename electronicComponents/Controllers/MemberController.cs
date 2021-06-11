﻿using electronicComponents.DAL;
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
    public class MemberController : Controller
    {

        private ICartService _cartService;
        private IProductService _productService;
        private IDiscountCodeDetailService _discountCodeDetailService;
        private IDiscountCodeService _discountCodeService;
        private ICustomerService _customerService;
        private IOrderService _orderService;
        private IOrderDetailService _orderDetailService;
        private IMemberService _memberService;
        private IRatingService _ratingService;


        public MemberController(IRatingService ratingService,ICartService cartService, IProductService productService, IDiscountCodeDetailService discountCodeDetailService, IDiscountCodeService discountCodeService, ICustomerService customerService, IOrderService orderService, IOrderDetailService orderDetailService, IMemberService memberService)
        {

            _cartService = cartService;
            _productService = productService;
            _discountCodeDetailService = discountCodeDetailService;
            _discountCodeService = discountCodeService;
            _customerService = customerService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _memberService = memberService;
            _ratingService = ratingService;
        }
        [HttpGet]
        public ActionResult ConfirmEmail(int ID)
        {
            if (_memberService.GetByID(ID).emailConfirmed.HasValue)
            {
                ViewBag.Message = "EmailConfirmed";
                return View();
            }
            string strString = "abcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            int randomCharIndex = 0;
            char randomChar;
            string captcha = "";
            for (int i = 0; i < 10; i++)
            {
                randomCharIndex = random.Next(0, strString.Length);
                randomChar = strString[randomCharIndex];
                captcha += Convert.ToString(randomChar);
            }
            string urlBase = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~");
            ViewBag.ID = ID;
            string email = _memberService.GetByID(ID).email;
            ViewBag.Email = "Mã xác minh đã được gửi đến: " + email;
            _memberService.UpdateCapcha(ID, captcha);
            SentMail("Mã xác minh tài khoản E-Commerce Website", email, "haichau0818@gmail.com", "id0ntkn0w", "<p>Mã xác minh của bạn: " + captcha);
            return View();
        }
        [HttpPost]
        public ActionResult ConfirmEmail(int ID, string capcha)
        {
            Member member = _memberService.CheckCapcha(ID, capcha);
            if (member != null)
            {
                ViewBag.Message = "emailConfirmed";
                return View();
            }
            ViewBag.Message = "Mã xác minh tài khoản không đúng";
            ViewBag.ID = ID;
            return View(new { ID = ID });
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

        public ActionResult CheckoutOrder(int ID)
        {
            string Name = _memberService.GetByID(ID).fullName;
            Customer customer = _customerService.GetAll().FirstOrDefault(x => x.fullName.Contains(Name));
            if (customer != null)
            {
                var orders = _orderService.GetByCustomerID(customer.id);
                Member member = Session["Member"] as Member;
                ViewBag.ProductRating = _ratingService.GetListAllRating().Where(x => x.memberID == member.id);
                return View(orders);
            }
            return View();
        }
        public ActionResult OrderDetail(int ID)
        {
            if (Session["Member"] == null)
            {
                return View();
            }
            if (ID == null)
            {
                return null;
            }
            OrderShip order = _orderService.GetByID(ID);
            IEnumerable<OrderDetail> orderDetails = _orderDetailService.GetByOrderID(ID);
            if (orderDetails == null)
            {
                return null;
            }
            ViewBag.OrderID = ID;
            if (order.isApproved.Value)
            {
                ViewBag.Approved = "Approved";
            }
            if (order.isDelivere.Value)
            {
                ViewBag.Delivere = "Delivere";
            }
            if (order.isReceived.Value)
            {
                ViewBag.Received = "Received";
            }
            ViewBag.Total = order.total;
            return View(orderDetails);
        }
        public JsonResult Cancel(int ID)
        {
            OrderShip order = _orderService.GetByID(ID);
            order.isCancel = true;
            _orderService.Update(order);
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Received(int OrderID)
        {
            _orderService.Received(OrderID);

            Member member = Session["Member"] as Member;
            //Update AmountPurchased for member
            if (member != null)
            {
                _memberService.UpdateAmountPurchased(member.id, _orderService.GetByID(OrderID).total.Value);
            }
            return RedirectToAction("OrderDetail", new { ID = OrderID });
        }
        public ActionResult GetDataProduct(int ID)
        {
            Product product = _productService.GetProductID(ID);
            return Json(new
            {
                ID = product.id,
                Name = product.name,
                Image = product.image1,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
      
    }
}