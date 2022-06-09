using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWebbb.Models;

namespace DoAnWebbb.Areas.Admin.Controllers
{
    public class LoginAdminController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            //gán các giá trị người dùng nhập liệu cho các biến
            var tendn = collection["username"];
            var matkhau = collection["password"];
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
                // Gán giá trị cho đối tượng được tạo mới(admin)
                NGUOIDUNG admin = db.NGUOIDUNGs.SingleOrDefault(n => n.USERNAME == tendn && n.PASS == matkhau);
                if (admin != null && admin.QUYEN.MAQUYEN == 1)
                {
                    Session["TaiKhoanAdmin"] = admin;
                    return RedirectToAction("Index", "HomeAdmin");
                }
                else
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return this.Login();
        }
        public ActionResult Logout()
        {
            Session["TaiKhoanAdmin"] = "";
            return RedirectToAction("Login", "HomeAdmin");
        }
    }
}