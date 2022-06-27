using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using DoAnWebbb.Models;

namespace DoAnWebbb.Areas.Admin.Controllers
{
    public class TaiKhoanController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        public ActionResult Index(int? page)
        {
            int pageNum = (page ?? 1);
            int pageSize = 10;
            return View(db.NGUOIDUNGs.Where(m=>m.MAQUYEN!=1).ToList().ToPagedList(pageNum, pageSize));
        }
        public ActionResult Edit(string id)
        {
            var a= db.NGUOIDUNGs.SingleOrDefault(m => m.SDT.ToString() == id.ToString() );
            return View(a);
        }
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            var a = db.NGUOIDUNGs.SingleOrDefault(m => m.SDT.ToString() == id.ToString());
            var tt = collection["TrangThai"];
            var q = collection["PhanQuyen"];
            if (tt != null && q!=null)
            {
                a.TRANGTHAI =Convert.ToInt32(tt);
                a.MAQUYEN = Convert.ToInt32(q);
                UpdateModel(a);
                db.SubmitChanges();
                return RedirectToAction("Index", "TaiKhoan");
            }
            else
            {
                ViewBag.error = "Lỗi !";
                return View(a);
            }
        }
        public ActionResult Delete(string id)
        {
            var a = db.NGUOIDUNGs.SingleOrDefault(m => m.SDT.ToString() == id.ToString());
            return View(a);
        }
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            var a = db.NGUOIDUNGs.SingleOrDefault(m => m.SDT.ToString() == id.ToString());
            try
            {
                db.NGUOIDUNGs.DeleteOnSubmit(a);
                db.SubmitChanges();
                return RedirectToAction("Index", "TaiKhoan");
            }
            catch (Exception)
            {
                ViewBag.err ="Không được xóa do còn đơn hàng của tài khoản này!";
                return View(a);
            }

        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, NGUOIDUNG nd)
        {
            var check = db.NGUOIDUNGs.Where(m => m.GMAIL == nd.GMAIL).FirstOrDefault();
            var checksdt = db.NGUOIDUNGs.Where(m => m.SDT == nd.SDT).FirstOrDefault();
            if (check != null)
            {
                ViewData["CheckMail"] = "Tài Khoản Này Đã Được Sử Dụng";
                return this.Create();
            }
            if (checksdt != null)
            {
                ViewBag.er = "Số điện thoại này đã được sử dụng!";
                return this.Create();
            }
            else
            {
                var TenKH = collection["HOVATEN"];
                var pass = collection["PASS"];
                var repass = collection["rePass"];
                var Gmail = collection["GMAIL"];
                if (pass != repass)
                {
                    ViewBag.er = "Xác nhận mật khẩu không chính xác!";
                    return this.Create();
                }
                var sdt = collection["SDT"];
                var GioiTinh = collection["GIOITINH"];
                var NamSinh = string.Format("{0:dd/MM/yyyy}", collection["NAMSINH"]);
                if (Convert.ToDateTime(NamSinh) > DateTime.Now)
                {
                    ViewBag.er = "Năm sinh không chính xác!";
                    return this.Create();
                }
                if (GioiTinh == null)
                {
                    ViewBag.er = "Giới tính không được để trống!";
                    return this.Create();
                }

                var DiaChi = collection["DIACHI"];
                try
                {

                
                nd.HOVATEN = TenKH;
                nd.GMAIL = Gmail;
                nd.DIACHI = DiaChi;
                nd.SDT = sdt;
                nd.GIOITINH = int.Parse(GioiTinh);
                nd.NAMSINH = DateTime.Parse(NamSinh);
                nd.PASS = pass;
                nd.USERNAME = Gmail;
                nd.TRANGTHAI = 1;
                nd.NGAYDANGKY = DateTime.Now;
                nd.MAQUYEN = 2;
                nd.DIEMTD = 0;
                nd.UUDAI = 0;
                db.NGUOIDUNGs.InsertOnSubmit(nd);
                db.SubmitChanges();
                }
                catch (Exception)
                {
                    ViewBag.er = "Lỗi khi tạo tài khoản!";
                    return this.Create();

                }

            }
            return RedirectToAction("Index", "TaiKhoan");
        }
    }
}