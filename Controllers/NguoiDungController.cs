using DoAnWebbb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnWebbb.Controllers
{
    
    public class NguoiDungController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        // GET: NguoiDung
        public ActionResult Index()
        {
            var nd = from n in data.NGUOIDUNGs select n;
            return View(nd);
        }
        public ActionResult Edit(string id)
        {
            var E_nd = data.NGUOIDUNGs.SingleOrDefault(m => m.USERNAME == id);
            return View(E_nd);
        }
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            var sv = data.NGUOIDUNGs.SingleOrDefault(m => m.USERNAME == id);
            /*id = collection[]*/
            var hoten = collection["HoTen"];
            var matkhau = collection["Pass"];
            var gioitinh = collection["GioiTinh"];
            var namsinh = collection["NamSinh"];
            var diachi = collection["DiaChi"];
            sv.USERNAME = id;
            if (string.IsNullOrEmpty(hoten) || string.IsNullOrEmpty(matkhau) || string.IsNullOrEmpty(diachi) || string.IsNullOrEmpty(gioitinh))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                sv.HOVATEN = hoten;
                sv.GIOITINH = Convert.ToInt32(gioitinh);
                sv.NAMSINH = Convert.ToDateTime(namsinh);
                sv.PASS = matkhau;
                sv.DIACHI = diachi;
                UpdateModel(sv);
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }

    }

}