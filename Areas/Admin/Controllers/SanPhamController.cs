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
        public ActionResult Index(int? page,String searchString)
        {
            ViewBag.Key = searchString;
            int pageNum = (page ?? 1);
            int pageSize = 15;
            
            if (!string.IsNullOrEmpty(searchString))
            {

                return View(db.SANPHAMs.OrderBy(n => n.NGAYCAPNHAT).Where(s=>s.TENSANPHAM.Contains(searchString)).ToList().ToPagedList(pageNum, pageSize));
            }
            
            return View(db.SANPHAMs.OrderBy(n => n.NGAYCAPNHAT).ToList().ToPagedList(pageNum, pageSize));
        }
        public ActionResult ChiTietSanPham(int id)
        {
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
            var E_thongsokythuat = collection["Thongso"];
            var E_anhsanpham = collection["HINH"];
            if (string.IsNullOrEmpty(collection["giagiam"]))
            {
                var E_giagiam = 0;
                sanpham.GIAGIAM = E_giagiam;
                var E_giaban = Convert.ToInt32(collection["giaban"]);
                sanpham.GIABAN = E_giaban;

            }
            else
            {
                var E_giagiam = Convert.ToInt32(collection["giagiam"]);
                sanpham.GIAGIAM = E_giagiam;
                sanpham.GIABAN = E_giagiam;

            }
            var E_soluongton = collection["soluongton"];
            var E_ngaycapnhat = DateTime.Now;


            if (string.IsNullOrEmpty(E_soluongton) || string.IsNullOrEmpty(E_anhsanpham) || string.IsNullOrEmpty(E_tensanpham) || string.IsNullOrEmpty(E_thongsokythuat))
            {
                ViewData["Error"] = "Don't empty!";
                return this.ThemSanPham();
            }
            else
            {
                E_thongsokythuat = E_thongsokythuat.Remove(E_thongsokythuat.Length - 4);
                E_thongsokythuat = E_thongsokythuat.Remove(0, 3);
                sanpham.TENSANPHAM = E_tensanpham.ToString();
                sanpham.HINH = E_anhsanpham;
                sanpham.NGAYCAPNHAT = E_ngaycapnhat;
                sanpham.TRANGTHAI = 1;
                db.SANPHAMs.InsertOnSubmit(sanpham);
                db.SubmitChanges();

            }
            return RedirectToAction("Index");
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
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
        public ActionResult Delete(int id)
        {
            var D_SP = db.SANPHAMs.FirstOrDefault(m => m.MASANPHAM == id);
            return View(D_SP);
        }
        [HttpPost]
        public ActionResult Delete (int id, FormCollection collection)
        {
            var D_sanpham = db.SANPHAMs.First(m => m.MASANPHAM == id);
            var fk= from i in db.CT_HOADONs where(i.MASANPHAM==id) select i.MASANPHAM;
            if (D_sanpham.MASANPHAM == id)
            {


                    foreach (var pk in fk)
                    {
                        if (D_sanpham.MASANPHAM == pk)
                        {
                            ViewBag.ErorrD = "Không xóa được do sản phẩm đã có trong đơn hàng!";

                            return View(D_sanpham);
                        }
                    }

                D_sanpham.TRANGTHAI = 0;
                db.SANPHAMs.DeleteOnSubmit(D_sanpham);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            } 
        }
    }
}