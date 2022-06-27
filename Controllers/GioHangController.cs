using DoAnWebbb.Controllers.MoMo;
using DoAnWebbb.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            }
            else
            {
                sanpham.SoLuong++;
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
            Session["Tongtien"] = tt;

            return tt;
        }

        public ActionResult GioHang()
        {
            NGUOIDUNG tk = (NGUOIDUNG)Session["TaiKhoanKH"];
            List<GioHang> lstGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TT = TongTien();
            if (Session["GioHang"] != null && Session["TaiKhoanKH"]!=null)
            {
                if (Convert.ToInt32(tk.DIEMTD) >= 0 && tk.DIEMTD < 500000)
                {
                    tk.UUDAI = 0;
                    ViewBag.Tongtien = Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI;
                }
                if (Convert.ToInt32(tk.DIEMTD) >= 500000 && tk.DIEMTD < 1000000)
                {
                    tk.UUDAI = 0.05;
                    ViewBag.Tongtien = Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI;
                }
                if (Convert.ToInt32(tk.DIEMTD) >= 1000000 && tk.DIEMTD < 2000000)
                {
                    tk.UUDAI = 0.06;
                    ViewBag.Tongtien = Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI;
                }
                if (Convert.ToInt32(tk.DIEMTD) >= 2000000 && tk.DIEMTD < 3000000)
                {
                    tk.UUDAI = 0.07;
                    ViewBag.Tongtien = Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI;
                }
                if (Convert.ToInt32(tk.DIEMTD) >= 3000000 && tk.DIEMTD < 4000000)
                {
                    tk.UUDAI = 0.08;
                    ViewBag.Tongtien = Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI;
                }
                if (Convert.ToInt32(tk.DIEMTD) >= 4000000 && tk.DIEMTD < 5000000)
                {
                    tk.UUDAI = 0.09;
                    ViewBag.Tongtien = Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI;
                }
                if (Convert.ToInt32(tk.DIEMTD) >= 5000000 && tk.DIEMTD < 6000000)
                {
                    tk.UUDAI = 0.1;
                    ViewBag.Tongtien = Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI;
                }
                if (Convert.ToInt32(tk.DIEMTD) >= 6000000)
                {
                    tk.UUDAI = 0.11;
                    ViewBag.Tongtien = Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI;
                }

            }
            else
            {
                ViewBag.Tongtien = TongTien();
            }
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
            NGUOIDUNG tk = (NGUOIDUNG)Session["TaiKhoanKH"];
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
            ViewBag.TT = TongTien();
            if (Session["GioHang"] != null && Session["TaiKhoanKH"] != null)
            {
                if (Convert.ToInt32(tk.DIEMTD) >= 0 && tk.DIEMTD < 500000)
                {
                    tk.UUDAI = 0;
                    ViewBag.Tongtien = Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI;
                }
                if (Convert.ToInt32(tk.DIEMTD) >= 500000 && tk.DIEMTD < 1000000)
                {
                    tk.UUDAI = 0.05;
                    ViewBag.Tongtien = Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI;
                }
                if (Convert.ToInt32(tk.DIEMTD) >= 1000000 && tk.DIEMTD < 2000000)
                {
                    tk.UUDAI = 0.06;
                    ViewBag.Tongtien = Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI;
                }
                if (Convert.ToInt32(tk.DIEMTD) >= 2000000 && tk.DIEMTD < 3000000)
                {
                    tk.UUDAI = 0.07;
                    ViewBag.Tongtien = Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI;
                }
                if (Convert.ToInt32(tk.DIEMTD) >= 3000000 && tk.DIEMTD < 4000000)
                {
                    tk.UUDAI = 0.08;
                    ViewBag.Tongtien = Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI;
                }
                if (Convert.ToInt32(tk.DIEMTD) >= 4000000 && tk.DIEMTD < 5000000)
                {
                    tk.UUDAI = 0.09;
                    ViewBag.Tongtien = Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI;
                }
                if (Convert.ToInt32(tk.DIEMTD) >= 5000000 && tk.DIEMTD < 6000000)
                {
                    tk.UUDAI = 0.1;
                    ViewBag.Tongtien = Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI;
                }
                if (Convert.ToInt32(tk.DIEMTD) >= 6000000)
                {
                    tk.UUDAI = 0.11;
                    ViewBag.Tongtien = Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI;
                }
                
            }
            else
            {
                ViewBag.Tongtien = TongTien();
            }
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGioHang);
        }
        public ActionResult XacNhanDon()
        {
            return View();
        }
        public ActionResult DatHang(FormCollection collection)
        {
            NGUOIDUNG tk = (NGUOIDUNG)Session["TaiKhoanKH"];
            List<GioHang> gh = Laygiohang();
            var ngaygiao = String.Format("{0:dd/MM/yyyy}", collection["NgayGiao"]);
            var diachi = tk.DIACHI;
            var ngaydat = DateTime.Now;


            PHIEUMUA pm = new PHIEUMUA();
            pm.DIACHIGIAOHANG = tk.DIACHI;
            pm.NGAYDAT = ngaydat;
            pm.USERNAME = tk.USERNAME;
            pm.TRANGTHAI = 1;
            data.PHIEUMUAs.InsertOnSubmit(pm);

            var maxdh = data.PHIEUMUAs.Max(i => i.MAPHIEUMUA) + 1;


            HOADON hd = new HOADON();
            hd.YEUCAU = "Không có";
            hd.TONGTIEN = Convert.ToInt32((Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI));
            hd.TRANGTHAI = 1;
            hd.MAPHIEUMUA = maxdh;
            data.HOADONs.InsertOnSubmit(hd);

            var maxhd = data.HOADONs.Max(i => i.MAHD) + 1;

            data.SubmitChanges();



            foreach (var item in gh)
            {
                CT_PHIEUMUA ctdh = new CT_PHIEUMUA();
                ctdh.MAPHIEUMUA = maxdh;
                ctdh.MASANPHAM = item.MaSP;
                ctdh.SOLUONG = item.SoLuong;
                data.CT_PHIEUMUAs.InsertOnSubmit(ctdh);

                var sp = data.SANPHAMs.Single(s => s.MASANPHAM == item.MaSP);
                sp.SOLUONGTON -= ctdh.SOLUONG;
                UpdateModel(sp);

                CT_HOADON cthd = new CT_HOADON();
                cthd.MAHD = maxhd;
                cthd.SOLUONG = item.SoLuong;
                cthd.MASANPHAM = item.MaSP;
                data.CT_HOADONs.InsertOnSubmit(cthd);
                data.SubmitChanges();

            }

            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("ngluan161121@gmail.com", "VNBookStore");
                    var receiverEmail = new MailAddress(tk.GMAIL, "Receiver");
                    var password = "usgjxlkacnydjaru";
                    var sub = "VNBookStore - Xác Nhận Đơn Hàng Thành Công";
                    var body = "Đơn hàng " + hd.MAHD + " Đã được xác nhận. \nCảm ơn bạn đã mua sản phẩm";
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

            ViewBag.message = "Thanh toán thành công";
            Session["GioHang"] = null;

            return RedirectToAction("XacNhanDon", "GioHang");
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
            Session["YeuCau"] = collection["YEUCAU"];
            NGUOIDUNG tk = (NGUOIDUNG)Session["TaiKhoanKH"];

            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOGZB820220616";
            string accessKey = "CrFXKje9gAyhrQ3u";
            string serectkey = "H8t28CW4NVT1YgKSuF2ocM91bZUMTOez";

            string orderInfo = "ĐH" + DateTime.Now.ToString("yyyyMMddHHss");
            string returnUrl = "https://localhost:44322/GioHang/returnUrl";
            string notifyurl = "https://259e-2402-800-63a7-f118-7151-5195-a6e9-8113.ap.ngrok.io/GioHang/Payment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test
            string amount = (Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI).ToString();
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
            string signature = crypto.signSHA256(rawHash, serectkey);

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
                       


            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);
            return Redirect(jmessage.GetValue("payUrl").ToString());

        }
        public ActionResult returnUrl()
        {
            string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
            param = Server.UrlDecode(param);
            MoMoSecurity crypto = new MoMoSecurity();
            string serectkey = "H8t28CW4NVT1YgKSuF2ocM91bZUMTOez";

            string signture = crypto.signSHA256(param, serectkey);
            if (signture != Request["signature"].ToString())
            {
                ViewBag.message = "Thông tin Request không hợp lệ";
            }
            if (!param.Contains("errorCode=0"))
            {
                ViewBag.message = "Thanh toán thất bại";
            }
            else
            {
                ViewBag.message = "Thanh toán thànhh công";

            }
            return View();
        }
            public ActionResult returnUrll( FormCollection collection)
        {
            NGUOIDUNG tk = (NGUOIDUNG)Session["TaiKhoanKH"];
            List<GioHang> gh = Laygiohang();
            var ngaygiao = String.Format("{0:dd/MM/yyyy}", collection["NgayGiao"]);
            var diachi = tk.DIACHI;
            var ngaydat = DateTime.Now;         

            
                PHIEUMUA pm = new PHIEUMUA();
                pm.DIACHIGIAOHANG = tk.DIACHI;
                pm.NGAYDAT = ngaydat;
                pm.USERNAME = tk.USERNAME;
                pm.TRANGTHAI = 2;
                data.PHIEUMUAs.InsertOnSubmit(pm);

                var maxdh = data.PHIEUMUAs.Max(i => i.MAPHIEUMUA) + 1;
                

                HOADON hd = new HOADON();
                hd.YEUCAU = "Không có";
                hd.TONGTIEN = Convert.ToInt32((Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI));
                hd.TRANGTHAI = 2;
                hd.MAPHIEUMUA = maxdh;
                data.HOADONs.InsertOnSubmit(hd);

                var maxhd = data.HOADONs.Max(i => i.MAHD) + 1;

                var nd = data.NGUOIDUNGs.First(u => u.USERNAME == tk.USERNAME);
                nd.UUDAI = tk.UUDAI;
                nd.DIEMTD += Convert.ToInt32((Convert.ToDouble(TongTien()) - Convert.ToDouble(TongTien()) * tk.UUDAI));
                UpdateModel(nd);
            data.SubmitChanges();



            foreach (var item in gh)
                {
                    CT_PHIEUMUA ctdh = new CT_PHIEUMUA();
                    ctdh.MAPHIEUMUA = maxdh;
                    ctdh.MASANPHAM = item.MaSP;
                    ctdh.SOLUONG = item.SoLuong;
                    data.CT_PHIEUMUAs.InsertOnSubmit(ctdh);

                    var sp = data.SANPHAMs.Single(s => s.MASANPHAM == item.MaSP);
                    sp.SOLUONGTON -= ctdh.SOLUONG;
                    UpdateModel(sp);

                    CT_HOADON cthd = new CT_HOADON();
                    cthd.MAHD = maxhd;
                    cthd.SOLUONG = item.SoLuong;
                    cthd.MASANPHAM = item.MaSP;
                    data.CT_HOADONs.InsertOnSubmit(cthd);
                    data.SubmitChanges();
                    
                }
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("ngluan161121@gmail.com", "VNBookStore");
                    var receiverEmail = new MailAddress(tk.GMAIL, "Receiver");
                    var password = "usgjxlkacnydjaru";
                    var sub = "VNBookStore - Xác Nhận Thanh Toán Thành Công";
                    var body = "Đơn hàng " + hd.MAHD + " Đã được thanh toán bằng momo và đang giao điến bạn. \nCảm ơn bạn đã mua sản phẩm";
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

            ViewBag.message = "Thanh toán thành công";
                Session["GioHang"] = null;

            return RedirectToAction("Index","Home");
        }




    }
}