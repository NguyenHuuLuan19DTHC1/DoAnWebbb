﻿using DoAnWebbb.Controllers.MoMo;
using DoAnWebbb.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace DoAnWebbb.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        MyDataDataContext data = new MyDataDataContext();

        public List<GioHang> Laygiohang()
        {
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<GioHang>();
                Session["GioHang"] = lstGiohang;
            }
            return lstGiohang;
        }

        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<GioHang> lstGioHang = Laygiohang();
            GioHang sanpham = lstGioHang.Find(n => n.MaSP == id);
            if (sanpham == null)
            {
                sanpham = new GioHang(id);
                lstGioHang.Add(sanpham);
               /* return Redirect(strURL);*/
            }
            else
            {
                sanpham.SoLuong++;
                /*return Redirect(strURL);*/
            }
            return Content("");
        }

        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tsl = lstGioHang.Sum(n => n.SoLuong);
            }
            return tsl;
        }

        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Count;
            }
            return tsl;
        }

        private decimal TongTien()
        {
            decimal tt = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tt = (decimal)lstGiohang.Sum(n => n.ThanhTien);
            }
            return tt;
        }

        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            ViewBag.Employee = "";

            return View(lstGioHang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();

            return PartialView();
        }

        public ActionResult XoaGioHang(int id)
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.MaSP == id);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.MaSP == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhapGioHang(int id, FormCollection collection)
        {
            SANPHAM sp = data.SANPHAMs.First(n => n.MASANPHAM == id);
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.MaSP == id);
            if (sanpham != null)
            {  
                    sanpham.SoLuong = int.Parse(collection["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }



        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("GioHang");
        }


        [HttpGet]

        public ActionResult DatHang()
        {
            if (Session["TaiKhoanKH"] == null || Session["TaiKhoanKH"].ToString() == "")
            {
                return RedirectToAction("Login", "Home");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("DatHang", "GioHang");
            }
            List<GioHang> lstGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGioHang);
        }
        public ActionResult DatHang(FormCollection collection)
        {

            PHIEUMUA dh = new PHIEUMUA();
            NGUOIDUNG tk = (NGUOIDUNG)Session["TaiKhoanKH"];
            SANPHAM sp = new SANPHAM();
            List<GioHang> gh = Laygiohang();

            var ngaygiao = String.Format("{0:dd/MM/yyyy}", collection["NgayGiao"]);
            var diachi = collection["DIACHIGIAOHANG"];
            var ngaydat = DateTime.Now;
            var user = Session["TaiKhoanKH"] as NGUOIDUNG;
            foreach (var item in gh)
            {

                sp = data.SANPHAMs.Single(n => n.MASANPHAM == item.MaSP);
                CT_PHIEUMUA ctdh = new CT_PHIEUMUA();
                ctdh.MASANPHAM = sp.MASANPHAM;
                ctdh.SOLUONG = item.SoLuong;
                sp.SOLUONGTON = ctdh.SOLUONG;
                dh.DIACHIGIAOHANG = diachi;
                dh.NGAYDAT = ngaydat;
                dh.USERNAME = user.USERNAME;


                //data.SANPHAMs.InsertOnSubmit(sp);
                data.PHIEUMUAs.InsertOnSubmit(dh);
                data.CT_PHIEUMUAs.InsertOnSubmit(ctdh);
                try
                {
                    if (ModelState.IsValid)
                    {
                        /*var senderEmail = new MailAddress("ngluan161121@gmail.com", "VNBookStore");
                        var receiverEmail = new MailAddress(tk.GMAIL, "Receiver");
                        var password = "usgjxlkacnydjaru";
                        var sub = "Xác Nhận Mua Hàng Thành Công";
                        var body = "Đơn hàng " + ctdh.MAPHIEUMUA + " đang được giao đến bạn \nCảm ơn bạn";*/
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("ngluan161121@gmail.com", "usgjxlkacnydjaru");
                        /*try
                        {*/
                        MailMessage mail = new MailMessage();
                        mail.From = new MailAddress("ngluan161121@gmail.com");
                        mail.To.Add(tk.GMAIL);
                        mail.Subject = "VNBookStore - Xác nhận đơn hàng";
                        mail.IsBodyHtml = true;
                        mail.Body = "Đơn hàng " + ctdh.MAPHIEUMUA + " đang được giao đến bạn \nCảm ơn bạn";
                        mail.Priority = MailPriority.High;

                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("ngluan161121@gmail.com", "usgjxlkacnydjaru");
                        SmtpServer.EnableSsl = true;

                        SmtpServer.Send(mail);
                    }
                }

                /*}
                catch (Exception ex)
                {
                    return HttpNotFound();
                }*/

                /*var smtp = new SmtpClient
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
                    return RedirectToAction("XacNhanDonHang", "GioHang");
                }

            }
        }*/
                catch (Exception ex)
                {
                    return RedirectToAction("XacNhanDonHangThatBai", "GioHang");
                }

                }
                    data.SubmitChanges();
                    Session["GioHang"] = null;
                    return RedirectToAction("XacNhanDonHang", "GioHang");           
        }

        public ActionResult XacNhanDonHangThatBai()
        {
            return View();
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }

        public ActionResult ThanhToanMoMo(FormCollection collection)
        {
            PHIEUMUA dh = new PHIEUMUA();
            NGUOIDUNG tk = (NGUOIDUNG)Session["TaiKhoanKH"];
            SANPHAM sp = new SANPHAM();
            List<GioHang> gh = Laygiohang();
            var ngaygiao = String.Format("{0:dd/MM/yyyy}", collection["NgayGiao"]);
            var diachi = collection["DIACHIGIAOHANG"];
            var ngaydat = DateTime.Now;
            var user = Session["TaiKhoanKH"] as NGUOIDUNG;




            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOGZB820220616";
            string accessKey = "CrFXKje9gAyhrQ3u";
            string serectkey = "H8t28CW4NVT1YgKSuF2ocM91bZUMTOez";

            string orderInfo = "ĐH" + DateTime.Now.ToString("yyyyMMddHHss");
            string returnUrl = "https://localhost:5045/GioHang/returnUrl";
            string notifyurl = "https://4a0d-2402-800-63a7-f118-146a-68ef-831-f950.ap.ngrok.io/GioHang/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test
            string amount = TongTien().ToString();
            string orderid = DateTime.Now.Ticks.ToString();
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            string rawHash = "partnerCode=" +
            partnerCode + "&accessKey=" +
            accessKey + "&requestId=" +
            requestId + "&amount=" +
            amount + "&orderId=" +
            orderid + "&orderInfo=" +
            orderInfo + "&returnUrl=" +
            returnUrl + "&notifyUrl=" +
            notifyurl + "&extraData=" +
            extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }
            };

            foreach (var item in gh)
            {
                var max = data.PHIEUMUAs.Max(i => i.MAPHIEUMUA) + 1;
                sp = data.SANPHAMs.Single(n => n.MASANPHAM == item.MaSP);
                CT_PHIEUMUA ctdh = new CT_PHIEUMUA();
                ctdh.MASANPHAM = sp.MASANPHAM;
                ctdh.SOLUONG = item.SoLuong;
                sp.SOLUONGTON -= ctdh.SOLUONG;
                dh.DIACHIGIAOHANG = diachi;
                dh.NGAYDAT = ngaydat;
                dh.USERNAME = user.USERNAME;
                /*ctdh.MAPHIEUMUA = max;*/

                //data.SANPHAMs.InsertOnSubmit(sp);
                data.PHIEUMUAs.InsertOnSubmit(dh);
                data.CT_PHIEUMUAs.InsertOnSubmit(ctdh);
                try
                {
                    if (ModelState.IsValid)
                    {
                        var senderEmail = new MailAddress("ngluan161121@gmail.com", "VNBookStore");
                        var receiverEmail = new MailAddress(tk.GMAIL, "Receiver");
                        var password = "usgjxlkacnydjaru";
                        var sub = "VNBookStore - Xác Nhận Thanh Toán Thành Công";
                        var body = "Đơn hàng " + ctdh.MAPHIEUMUA + " Đã được thanh toán bằng momo và đang giao điến bạn. \nCảm ơn bạn đã mua sản phẩm";
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

                    }
                }
                catch (Exception ex)
                {
                    return HttpNotFound(ex.Message);
                }

                data.SubmitChanges();
                Session["GioHang"] = null;
            }
            data.SubmitChanges();


            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);

            return Redirect(jmessage.GetValue("payUrl").ToString());
        }


        public ActionResult returnUrl()
        {
            return View();
            /*CT_PHIEUMUA ctdh = new CT_PHIEUMUA();
            NGUOIDUNG kh = new NGUOIDUNG();
            try
            {
                var senderEmail = new MailAddress("ngluan161121@gmail.com", "VNBookStore");
                var receiverEmail = new MailAddress(kh.GMAIL, "Receiver");
                var password = "usgjxlkacnydjaru";
                var sub = "Xác Nhận Mua Hàng Thành Công";
                var body = "Đơn hàng "; //+ ctdh.MAPHIEUMUA + " đang được giao đến bạn \nCảm ơn bạn";
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


            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index", "Home");*/
        }




        [HttpPost]
        public void SavePayment()
        {
            //cập nhật dữ liệu vào db
        }


    }
}