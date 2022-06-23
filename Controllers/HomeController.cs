
using DoAnWebbb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net.Mail;
using System.Net;
using System.Web.Helpers;
using System.Threading.Tasks;

namespace demoWeb.Controllers
{
    public class HomeController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();

        public ActionResult Index(int? page, string searchString, string searchKey)
        {

            ViewBag.Key = searchString;
            if (page == null) page = 1;
            var all_sanPham = (from tt in data.SANPHAMs select tt).OrderBy(m => m.NGAYCAPNHAT);
            int pageSize = 6;
            int pageNum = page ?? 1;
            return View(all_sanPham.ToPagedList(pageNum, pageSize));
        }
        public ActionResult TongQuan()
        {
            return View();
        }
        public ActionResult LienHe()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        //public class MailInfor{
        //    public string From { get; set; }
        //    public string To { get; set; }
        //    public string Subject { get; set; }
        //    public string Body { get; set; }
        //}
        public ActionResult Book()
        {
            return View();
        }


        public ActionResult Menu()
        {
            var all_sanPham = from tt in data.SANPHAMs select tt;
            return View(all_sanPham);

        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(FormCollection collection, NGUOIDUNG kh)
        {
            //int OTP = new Random().Next(100000, 999999);
            var check = data.NGUOIDUNGs.Where(m => m.GMAIL == kh.GMAIL).FirstOrDefault();
            var checksdt = data.NGUOIDUNGs.Where(m => m.SDT == kh.SDT).FirstOrDefault();
            if (check != null)
            {
                ViewData["CheckMail"] = "Tài Khoản Này Đã Được Sử Dụng";
                return this.Register();
            }
            if (checksdt !=null)
            {
                ViewBag.er = "Số điện thoại này đã được sử dụng!";
                return this.Register();
            }
            else
            {
                var TenKH = collection["HOVATEN"];
                var pass = collection["PASS"];
                var repass = collection["rePass"];
                var Gmail = collection["GMAIL"];
                if(pass != repass)
                {
                    ViewBag.er = "Xác nhận mật khẩu không chính xác!";
                    return this.Register();
                }
                var sdt = collection["SDT"];
                var GioiTinh = collection["GIOITINH"];
                var NamSinh = string.Format("{0:dd/MM/yyyy}", collection["NAMSINH"]);
                if (Convert.ToDateTime(NamSinh) > DateTime.Now)
                {
                    ViewBag.er = "Năm sinh không chính xác!";
                    return this.Register();
                }
                if (GioiTinh == null)
                {
                    ViewBag.er = "Giới tính không được để trống!";
                    return this.Register();
                }
                
                var DiaChi = collection["DIACHI"];
                //int quyen = 0;

                //Gán giá trị cho đổi tượng được tạo mới (kh)
                kh.HOVATEN = TenKH;
                kh.GMAIL = Gmail;
                kh.DIACHI = DiaChi;
                kh.SDT = sdt;
                kh.GIOITINH = int.Parse(GioiTinh);
                kh.NAMSINH = DateTime.Parse(NamSinh);
                kh.PASS = pass;
                kh.USERNAME = Gmail;
                kh.TRANGTHAI = 1;
                kh.NGAYDANGKY = DateTime.Now;
                kh.MAQUYEN = 2;
                kh.DIEMTD = 0;
                kh.UUDAI = 0;
                //kh.GIOITINH = int.Parse(OTP);
                data.NGUOIDUNGs.InsertOnSubmit(kh);
                data.SubmitChanges();

            }
            return RedirectToAction("Login", "Home");

        }

        public ActionResult LogOut()
        {
            Session["TaiKhoanKH"] = null;
            Session["GioHang"] = null;
            return RedirectToAction("Login", "Home");
        }
        //Xác nhận OTP
        public ActionResult XacNhanOTP()
        {
            return View();
        }


        [HttpPost]
        public JsonResult KiemTraOTP(string otp)
        {
            bool result = false;

            string sessionOTP = Session["OTPDK"].ToString();
            if (otp == sessionOTP)
            {
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["USERNAME"];
            var matkhau = collection["PASS"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Error1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Error2"] = "Phải nhập mật khẩu";
            }
            else
            {
                // Gán giá trị cho đối tượng được tạo mới(kh)
                NGUOIDUNG kh = data.NGUOIDUNGs.SingleOrDefault(n => n.USERNAME == tendn && n.PASS == matkhau);
                if (kh != null && kh.MAQUYEN == 2 && kh.TRANGTHAI!=0)
                {
                    Session["TaiKhoanKH"] = kh;
                    return RedirectToAction("Index", "Home");
                }

                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

        public ActionResult Details(int id)
        {
            var detail = data.SANPHAMs.Where(m => m.MASANPHAM == id).First();
            return View(detail);
            //var sp = from s in data.SANPHAMs where s.MASANPHAM == id select s;
            //return View(sp.Single());
        }
        [HttpGet]
        public ActionResult ResetPass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPass(FormCollection collection)
        {
            var tk = collection["Username"];
            var tknd = data.NGUOIDUNGs.SingleOrDefault(u => u.USERNAME.ToUpper() == tk.ToUpper());
            if (tknd != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("ngluan161121@gmail.com", "usgjxlkacnydjaru");
                        MailMessage mail = new MailMessage();
                        mail.From = new MailAddress("ngluan161121@gmail.com");
                        mail.To.Add(tknd.GMAIL);
                        mail.Subject = "Thay đổi mật khẩu";
                        mail.IsBodyHtml = true;
                        mail.Body = "Mật khẩu của bạn là " + tknd.PASS + " \nTrân trọng!";
                        mail.Priority = MailPriority.High;

                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("ngluan161121@gmail.com", "usgjxlkacnydjaru");
                        SmtpServer.EnableSsl = true;

                        SmtpServer.Send(mail);
                        return RedirectToAction("Index", "Home");
                    }
                }

                catch (Exception ex)
                {
                    return RedirectToAction("XacNhanDonHangThatBai", "GioHang");
                }
            }
            else
            {
                ViewBag.TB = "Hãy điền đúng email bạn đã đăng ký";
            }
            return View();
        }


    }
}