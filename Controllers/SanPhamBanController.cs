using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWebbb.Models;
using PagedList;
using PagedList.Mvc;

namespace DoAnWebbb.Controllers
{
    public class SanPhamBanController : Controller
    {
        // GET: SanPhamBan
        MyDataDataContext db = new MyDataDataContext();
        public ActionResult Index(int? page)
        {
            ViewBag.MacDinh = db.SANPHAMs.ToList();
            ViewBag.Moi = db.SANPHAMs.ToList().OrderBy(n => n.NGAYCAPNHAT);
            ViewBag.GiaCao = db.SANPHAMs.ToList().OrderByDescending(n => n.GIAGIAM);
            ViewBag.GiaThap = db.SANPHAMs.ToList().OrderBy(n => n.GIAGIAM);
            int pageSize = 9;
            int pageNum = (page ?? 1);
            return View(db.SANPHAMs.ToPagedList(pageNum, pageSize));
        }
        public ActionResult HangMay()
        {
            var hangmay = from hang in db.NHACUNGCAPs select hang;
            return PartialView(hangmay);
        }
        public ActionResult LoaiMay()
        {
            var loaimay = from loai in db.LOAIMAYs select loai;
            return PartialView(loaimay);
        }
        public ActionResult SPTheoHang(int id)
        {
            var sanpham = from sp in db.SANPHAMs where sp.MASANPHAM == id select sp;
            return View(sanpham);
        }
        public ActionResult SPTheoLoai(int id)
        {
            var sanpham = from sp in db.SANPHAMs where sp.MASANPHAM == id select sp; ;
            return View(sanpham);
        }
        public ActionResult Details(int id)
        {
            var detail = db.SANPHAMs.Where(m => m.MASANPHAM == id).First();
            return View(detail);
            //var sp = from s in data.SANPHAMs where s.MASANPHAM == id select s;
            //return View(sp.Single());
        }
    }
}