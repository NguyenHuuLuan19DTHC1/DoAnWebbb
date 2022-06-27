using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DoAnWebbb.Models;

namespace DoAnWebbb.Areas.Admin.Controllers
{
    public class QLHangController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        public ActionResult Index( string searchString )
        {

            ViewBag.Key=searchString;
            
            if (!string.IsNullOrEmpty(searchString))
            {
                return View(db.NHACUNGCAPs.OrderBy(n => n.MANCC).Where(s => s.TENNCCC.Contains(searchString)));
            }
            return View(db.NHACUNGCAPs.ToList());
        }
        public ActionResult Edit(int id)
        {
            NHACUNGCAP ncc = db.NHACUNGCAPs.SingleOrDefault(n => n.MANCC == id);
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ncc);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_loai = db.NHACUNGCAPs.First(m => m.MANCC == id);
            var E_tenloai = collection["TenLoai"];
            E_loai.MANCC = id;
            UpdateModel(E_loai);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]

        [ValidateInput(false)]
        public ActionResult Create(NHACUNGCAP nhacc, FormCollection collection)
        {

            var a = from n in db.NHACUNGCAPs select n.TENNCCC;
            var ten = collection["TenNCC"];
            foreach (var n in a)
            {
                if (string.Compare(n, ten, true) == 0)
                {
                    return Create();
                }
            }
            if (string.IsNullOrEmpty(ten))
            {
                ViewData["Error"] = "Don't empty!";
                return Create();
            }
            else
            {
                nhacc.TENNCCC = ten;
                db.NHACUNGCAPs.InsertOnSubmit(nhacc);
                db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var D_NCC = db.NHACUNGCAPs.FirstOrDefault(m => m.MANCC == id);
            return View(D_NCC);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_NCC = db.NHACUNGCAPs.FirstOrDefault(m => m.MANCC == id);
            var I_SP = from pk in db.SANPHAMs where pk.MANCC == id select pk.MANCC;
            if (D_NCC.MANCC == id)
            {

                foreach (var pk in I_SP)
                {
                    if (D_NCC.MANCC == pk)
                    {   
                        ViewBag.ErorrD = "Không xóa được do còn sản phẩm thuộc nhà cung cấp này!";

                        return View(D_NCC);
                    }
                }               
                    db.NHACUNGCAPs.DeleteOnSubmit(D_NCC);
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