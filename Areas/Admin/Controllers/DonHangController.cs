using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWebbb.Models;
using PagedList;
using PagedList.Mvc;

namespace DoAnWebbb.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        // GET: Admin/DonHang
        public ActionResult Index(int?page)
        {
            int pageNum = (page ?? 1);
            int pageSize = 10;
            var all = from a in db.PHIEUMUAs
                      from b in db.HOADONs
                      where a.MAPHIEUMUA == b.MAPHIEUMUA
                      where b.TONGTIEN > 0
                      select a ;
            return View(all.OrderBy(n => n.NGAYDAT).ToPagedList(pageNum, pageSize));
        }

        public ActionResult Detail(int id, int?page)
        {
            /*            SANPHAM sanpham = db.SANPHAMs.SingleOrDefault(n => n.MASANPHAM == id);
                        ViewBag.MaSanPham = sanpham.MASANPHAM;
                        if (sanpham == null)
                        {
                            Response.StatusCode = 404;
                            return null;
                        }
                        return View(sanpham);*/
            var donhang = db.CT_HOADONs.Where(m => m.MAHD == id).ToList();
            /*ViewBag.MaPhieuMua = donhang.MAHD;*/
            if (donhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            int pageNum = (page ?? 1);
            int pageSize = 10;

            return View(donhang.OrderBy(n=>n.SOLUONG).ToPagedList(pageNum, pageSize));

        }
        [HttpPost]
        public ActionResult Detail(int id, FormCollection collection)
        {
            var trangthai = db.PHIEUMUAs.Where(a => a.MAPHIEUMUA-159 == id).SingleOrDefault();
            var tt = collection["TrangThai"];
            if (tt != null)
            {
                trangthai.TRANGTHAI = Convert.ToInt32(tt);
                UpdateModel(trangthai);
                db.SubmitChanges();
                return RedirectToAction("Index","DonHang");

            }
            else
            {
                ViewBag.erorr = "Hãy chọn trạng thái";
                return View();
            }
        }
        public ActionResult Edit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
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
    }
}