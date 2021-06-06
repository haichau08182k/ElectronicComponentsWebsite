using electronicComponents.DAL;
using electronicComponents.Service;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace electronicComponents.Controllers
{
    public class ManageOrderController : Controller
    {
        private IOrderService _orderService;
        private ICustomerService _customerService;
        private IOrderDetailService _orderDetailService;
        private IProductService _productService;
        public ManageOrderController(IOrderService orderService, ICustomerService customerService, IOrderDetailService orderDetailService, IProductService productService)
        {
            _orderService = orderService;
            _customerService = customerService;
            _orderDetailService = orderDetailService;
            _productService = productService;
        }
        // GET: OrderManage
        public ActionResult NotApproval()
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            IEnumerable<OrderShip> orderList = _orderService.GetOrderNotApproval();
            List<OrderShip> orderListPaging = new List<OrderShip>(orderList);

            IEnumerable<Customer> customerList = _customerService.GetAll();
            ViewBag.CustomerList = customerList;
            return View(orderList);
        }
        public ActionResult NotDelivery(int page = 1)
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            IEnumerable<OrderShip> orderList = _orderService.GetOrderNotDelivery();
            PagedList<OrderShip> orderListPaging = new PagedList<OrderShip>(orderList, page, 10);

            IEnumerable<Customer> customerList = _customerService.GetAll();
            ViewBag.CustomerList = customerList;
            return View(orderListPaging);
        }

        public ActionResult DeliveredAndPaid(int page = 1)
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            IEnumerable<OrderShip> orderList = _orderService.GetOrderDeliveredAndPaid();
            PagedList<OrderShip> orderListPaging = new PagedList<OrderShip>(orderList, page, 10);

            IEnumerable<Customer> customerList = _customerService.GetAll();
            ViewBag.CustomerList = customerList;
            return View(orderListPaging);
        }
        public ActionResult ApprovedAndNotDelivery(int page = 1)
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            IEnumerable<OrderShip> orderList = _orderService.ApprovedAndNotDelivery();
            PagedList<OrderShip> orderListPaging = new PagedList<OrderShip>(orderList, page, 10);

            IEnumerable<Customer> customerList = _customerService.GetAll();
            ViewBag.CustomerList = customerList;
            return View(orderListPaging);
        }
        public ActionResult DeliveredList(int page = 1)
        {
            if (Session["Employee"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            IEnumerable<OrderShip> orderList = _orderService.GetDelivered();
            PagedList<OrderShip> orderListPaging = new PagedList<OrderShip>(orderList, page, 10);

            IEnumerable<Customer> customerList = _customerService.GetAll();
            ViewBag.CustomerList = customerList;
            return View(orderList);
        }
        [HttpGet]
        public ActionResult OrderApproval(int ID)
        {
            OrderShip order = _orderService.Approved(ID);
            //Get email customer
            string Email = _customerService.GetEmailByID(order.customerID.Value);
            SentMail("Đơn hàng của bạn đã được duyệt", Email, "haichau0818@gmail.com", "id0ntkn0w", "Vào đơn hàng của bạn để xem thông tin chi tiết");
            return RedirectToAction("ApprovedAndNotDelivery");
        }
        [HttpGet]
        public ActionResult Delivered(int ID)
        {
            OrderShip order = _orderService.Delivered(ID);
            //Get email customer
            string Email = _customerService.GetEmailByID(order.customerID.Value);
            string urlBase = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~");
            SentMail("Đơn hàng của bạn đã được giao cho đối tác vận chuyển", Email, "haichau0818@gmail.com", "id0ntkn0w", "Vào đơn hàng của bạn để xem thông tin chi tiết. Sau khi nhận được đơn hàng, bạn vui lòng click vào link sau để xác nhận đã nhận được đơn hàng từ đơn vị vận chuyển: " + urlBase + "/OrderManage/Received/" + ID + "");
            return RedirectToAction("DeliveredList");
        }
        [HttpGet]
        public ActionResult Detail(int ID)
        {
            if (ID == null)
            {
                return null;
            }
            //Get product catetgory
            IEnumerable<OrderDetail> orderDetails = _orderDetailService.GetByOrderID(ID);
            if (orderDetails == null)
            {
                return null;
            }
            ViewBag.OrderID = ID;
            return View(orderDetails);
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
        //[HttpGet]
        //public ActionResult Received(int ID)
        //{
        //    Order order = _orderService.GetByID(ID);
        //    order.IsReceived = true;
        //    order.IsPaid = true;
        //    _orderService.Update(order);
        //    IEnumerable<OrderDetail> orderDetail = _orderDetailService.GetByOrderID(order.ID);
        //    foreach (var item in orderDetail)
        //    {
        //        _productService.UpdateQuantity(item.ProductID, item.Quantity);
        //        _productService.UpdatePurchasedCount(item.ProductID, item.Quantity);
        //    }
        //    return RedirectToAction("Index", "Home");
        //}
    }
}