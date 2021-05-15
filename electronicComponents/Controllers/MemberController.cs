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
    public class MemberController : Controller
    {
        public MemberService _memberService=new MemberService();
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
    }
}