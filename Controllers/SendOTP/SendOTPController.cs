using DoAnWebbb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace DoAnWebbb.Controllers.SendOTP
{
    public class SendOTPController : Controller
    {
        MyDataDataContext context = new MyDataDataContext();
        // GET: SendOTP
        public ActionResult SendOTP()
        {
            return View();
            //int OTP = new Random().Next(100000, 999999);
            //NGUOIDUNG kh = new NGUOIDUNG();
            //try
            //{
            //    if (ModelState.IsValid)
            //    {
            //        var senderEmail = new MailAddress("haunguyenaaaa6@gmail.com", "Cửa Hàng Máy Ảnh Hutech");
            //        var receiverEmail = new MailAddress(kh.GMAIL, "Receiver");
            //        var password = "hau121pk";
            //        var sub = "Mã OTP Xác Nhận";
            //        var body = "Mã OTP Của Bạn là :"+ OTP +"Vui Lòng Sử Dụng Mã Này Để Xác Thực";
            //        var smtp = new SmtpClient
            //        {
            //            Host = "smtp.gmail.com",
            //            Port = 587,
            //            EnableSsl = true,
            //            DeliveryMethod = SmtpDeliveryMethod.Network,
            //            UseDefaultCredentials = false,
            //            Credentials = new NetworkCredential(senderEmail.Address, password)
            //        };
            //        using (var mess = new MailMessage(senderEmail, receiverEmail)
            //        {
            //            Subject = sub,
            //            Body = body
            //        })
            //        {
            //            smtp.Send(mess);
            //        }
            //        Session["OTPDK"] = OTP;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return HttpNotFound();
            //}
        }
    }
}