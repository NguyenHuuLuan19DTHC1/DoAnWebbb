﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWebbb.Models;
using PagedList;
using PagedList.Mvc;

namespace DoAnWebbb.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        // GET: Admin/HomeAdmin
        public ActionResult Index(int? page)
        {
            // Lấy ra tổng số các đơn đặt hàng đang có trong database - tổng số đơn hàng 
            ViewBag.DonHang = db.PHIEUMUAs.Count();
            // Lấy ra tổng số tiền các đơn hàng - lợi nhuận
            ViewBag.LoiNhuan = db.HOADONs.Sum(model => model.TONGTIEN);
            // Lấy ra tổng số lượng các user khách hàng đang có trong database - tổng số tài khoản hiện tại
            ViewBag.NguoiDung = db.NGUOIDUNGs.Where(model => model.MAQUYEN == 0).Count();
            //// Lấy số lượng người truy cập từ Application đã tạo
            //ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();
            // Lấy ra tổng số sản phẩm đang có trong database
            ViewBag.SanPham = db.SANPHAMs.Count();

            int pageNum = (page ?? 1);
            int pageSize = 7;
            return View(db.PHIEUMUAs.ToList().OrderBy(n => n.NGAYDAT).ToPagedList(pageNum, pageSize));
        }
        public ActionResult Info()
        {
            
            NGUOIDUNG tk = (NGUOIDUNG)Session["TaiKhoanAD"];
            
            return View(tk);
        }
        public ActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePass(FormCollection collection)
        {
            NGUOIDUNG tk = (NGUOIDUNG)Session["TaiKhoanAD"];
            var nd = from n in db.NGUOIDUNGs where (n.USERNAME == tk.USERNAME) select n;
            tk.PASS = collection["PassAD"];
            foreach (var a in nd)
            {
                a.PASS = tk.PASS;
            }
            UpdateModel(nd);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult LogOut()
        {
            Session["TaiKhoanAD"] = null;
            return RedirectToAction("Login", "Home");
        }
    }
}