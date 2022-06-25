using DoAnWebbb.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnWebbb.Areas.DVVC.Controllers
{
    public class HomeDVVCController : Controller
    {
        // GET: DVVC/Home
        MyDataDataContext db = new MyDataDataContext();
        public ActionResult IndexDVVC(int? page)
        {
            ViewBag.DonHang = db.HOADONs.Count(i => i.TONGTIEN > 0 && i.PHIEUMUA.TRANGTHAI >=3 );
            var all = from a in db.PHIEUMUAs
                      from b in db.HOADONs
                      where a.MAPHIEUMUA == b.MAPHIEUMUA
                      where b.TONGTIEN > 0
                      where a.TRANGTHAI >= 3
                      select a;
            int pageNum = (page ?? 1);
            int pageSize = 7;
            return View(all.OrderBy(n => n.NGAYDAT).ToPagedList(pageNum, pageSize));
        }
        public ActionResult DetailDVVC(int id, int? page)
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

            return View(donhang.OrderBy(n => n.SOLUONG).ToPagedList(pageNum, pageSize));

        }
        [HttpPost]
        public ActionResult DetailDVVC(int id, FormCollection collection)
        {
            var trangthai = db.PHIEUMUAs.Where(a => a.MAPHIEUMUA - 159 == id).SingleOrDefault();
            var tt = collection["TrangThaiDH"];
            if (tt != null)
            {
                trangthai.TRANGTHAI = Convert.ToInt32(tt);
                UpdateModel(trangthai);
                db.SubmitChanges();
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ViewBag.erorr = "Hãy chọn trạng thái";
                return View();
            }

        }
        public ActionResult LogOutDVVC()
        {
            Session["TaiKhoanAD"] = null;
            return RedirectToAction("LoginDVVC", "LoginDVVC");
        }
    }
}