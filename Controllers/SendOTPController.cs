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
        public ActionResult SenOTP()
        {
            int OTP = new Random().Next(100000, 999999);
            NGUOIDUNG kh = new NGUOIDUNG();
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("ngluan161121@gmail.com", "VNBookStore");
                    var receiverEmail = new MailAddress(kh.GMAIL, "Receiver");
                    var password = "usgjxlkacnydjaru";
                    var sub = "Mã OTP Xác Nhận";
                    var body = "Mã OTP Của Bạn là :" + OTP + "Vui Lòng Sử Dụng Mã Này Để Xác Thực";
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = sub,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    Session["OTPDK"] = OTP;
                }
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
            return RedirectToAction("CheckOTP","SendOTP");
        }
       /* public ActionResult SendOTP(FormCollection collection)
        {
            return View();
            int OTP = new Random().Next(100000, 999999);
            NGUOIDUNG kh = new NGUOIDUNG();
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("ngluan161121@gmail.com", "VNBookStore");
                    var receiverEmail = new MailAddress(kh.GMAIL, "Receiver");
                    var password = "usgjxlkacnydjaru";
                    var sub = "Mã OTP Xác Nhận";
                    var body = "Mã OTP Của Bạn là :" + OTP + "Vui Lòng Sử Dụng Mã Này Để Xác Thực";
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = sub,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    Session["OTPDK"] = OTP;
                }
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }

            
        }*/
        public ActionResult CheckOTP(FormCollection collection)
        {
            if (Session["OTPDK"] == collection["OTP"])
            {
                return RedirectToAction("XacNhanDonHang", "GioHang");
            }
            else
            {
                return RedirectToAction("XacNhanDonHangThatBai", "GioHang");
            }
        }
    }
}