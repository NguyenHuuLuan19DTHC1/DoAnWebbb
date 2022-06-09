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
            return View(db.PHIEUMUAs.ToList().OrderBy(n => n.NGAYDAT).ToPagedList(pageNum, pageSize));
        }
        public ActionResult ChiTietDonHang(int id)
        {
            //Lấy ra đối tượng sách theo mã
            PHIEUMUA donhang = db.PHIEUMUAs.SingleOrDefault(n => n.MAPHIEUMUA == id);
            ViewBag.MaPhieuMua = donhang.MAPHIEUMUA;
            if (donhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(donhang);
        }
    }
}