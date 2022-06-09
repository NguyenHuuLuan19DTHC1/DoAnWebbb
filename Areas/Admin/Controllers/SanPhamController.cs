using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWebbb.Models;
using PagedList;
using PagedList.Mvc;

namespace DoAnWebbb.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        public ActionResult Index(int? page)
        {
            int pageNum = (page ?? 1);
            int pageSize = 7;
            return View(db.SANPHAMs.ToList().OrderBy(n => n.NGAYCAPNHAT).ToPagedList(pageNum, pageSize));
        }
        public ActionResult ChiTietSanPham(int id)
        {
            //Lấy ra đối tượng sách theo mã
            SANPHAM sanpham = db.SANPHAMs.SingleOrDefault(n => n.MASANPHAM == id);
            ViewBag.MaSanPham = sanpham.MASANPHAM;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }
        public ActionResult ThemSanPham()
        {
            //đưa dữ liệu vào dropdownList
            //Lấy ds từ table chủ đề, sắp xếp tăng dần theo tên chỉ đề
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(n => n.TENNCCC), "MANCC", "TENNCCC");
            ViewBag.MaLoai = new SelectList(db.LOAIMAYs.ToList().OrderBy(n => n.TENLOAI), "MALOAI", "TENLOAI");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemSanPham(SANPHAM sanpham, FormCollection collection)
        {
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(n => n.TENNCCC), "MANCC", "TENNCCC");
            ViewBag.MaLoai = new SelectList(db.LOAIMAYs.ToList().OrderBy(n => n.TENLOAI), "MALOAI", "TENLOAI");
            var E_tensanpham = collection["tensanpham"];
            var E_thongsokythuat = collection["thongsokythuat"];
            var E_anhsanpham = collection["HINH"];
            var E_giaban = Convert.ToDecimal(collection["giaban"]);
            var E_giagiam = Convert.ToDouble(collection["giagiam"]);
            var E_soluongton = collection["soluongton"];
            var E_ngaycapnhat = DateTime.Now;
    
            //if (string.IsNullOrEmpty(E_tensanpham) || string.IsNullOrEmpty(E_thongsokythuat))
            //{
            //    ViewData["Error"] = "Don't empty!";
            //    return this.ThemSanPham();
            //}
            //else
            //{
                sanpham.TENSANPHAM = E_tensanpham.ToString();
                sanpham.THONGSOKYTHUAT = E_thongsokythuat;
                sanpham.HINH = E_anhsanpham;
                sanpham.GIABAN = E_giaban;
                sanpham.GIAGIAM = E_giagiam;
                sanpham.NGAYCAPNHAT = E_ngaycapnhat;

                sanpham.TRANGTHAI = 1;
                db.SANPHAMs.InsertOnSubmit(sanpham);
                db.SubmitChanges();
               
            //}
            return RedirectToAction("Index");
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images_product/" + file.FileName));
            return "Content/images_product/" + file.FileName;
        }
        public ActionResult SuaSanPham(int id)
        {
            SANPHAM sanpham = db.SANPHAMs.SingleOrDefault(n => n.MASANPHAM == id);
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(n => n.TENNCCC), "MANCC", "TENNCCC");
            ViewBag.MaLoai = new SelectList(db.LOAIMAYs.ToList().OrderBy(n => n.TENLOAI), "MALOAI", "TENLOAI");
            return View(sanpham);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSanPham(int id, FormCollection collection)
        {
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(n => n.TENNCCC), "MANCC", "TENNCCC");
            ViewBag.MaLoai = new SelectList(db.LOAIMAYs.ToList().OrderBy(n => n.TENLOAI), "MALOAI", "TENLOAI");
            var E_sanpham = db.SANPHAMs.First(m => m.MASANPHAM == id);
            var E_tensanpham = collection["tensanpham"];
            var E_thongsokythuat = collection["thongsokythuat"];
            var E_anhsanpham = collection["anhsanpham"];
            var E_giaban = Convert.ToDecimal(collection["giaban"]);
            var E_giagiam = Convert.ToDouble(collection["giagiam"]);
            var E_ngaycapnhat = DateTime.Now;


            E_sanpham.MASANPHAM = id;

                E_sanpham.TENSANPHAM = E_tensanpham;
                E_sanpham.THONGSOKYTHUAT = E_thongsokythuat;
                E_sanpham.HINH = E_anhsanpham;
                E_sanpham.GIABAN = E_giaban;
                E_sanpham.GIAGIAM = E_giagiam;
                E_sanpham.NGAYCAPNHAT = E_ngaycapnhat;

                UpdateModel(E_sanpham);
                db.SubmitChanges();
                return RedirectToAction("Index");             
        }
        public ActionResult Delete()
        {
            return View("Index");
        }
        [HttpPost]
        public ActionResult Delete (int id)
        {
            var E_sanpham = db.SANPHAMs.First(m => m.MASANPHAM == id);
            if (E_sanpham.MASANPHAM == id)
            {
                E_sanpham.TRANGTHAI = 0;
                db.SANPHAMs.DeleteOnSubmit(E_sanpham);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            } 
            return RedirectToAction("Index");
        }
    }
}