using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWebbb.Models;

namespace DoAnWebbb.Areas.Admin.Controllers
{
    public class QLLoaiSachController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        // GET: Admin/QLLoaiMay
        public ActionResult Index(string searchString)
        {
            ViewBag.Key = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                return View(db.LOAIMAYs.OrderBy(n => n.MALOAI).Where(s => s.TENLOAI.Contains(searchString)));
            }
            return View(db.LOAIMAYs.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]

        [ValidateInput(false)]
        public ActionResult Create(LOAIMAY loai, FormCollection collection)
        {
            var ten = collection["TenLSP"];
            var all = from a in db.LOAIMAYs select a.TENLOAI;
            foreach (var item in all)
            {
                if (string.Compare(ten, item, true) == 0)
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
                loai.TENLOAI = ten;
                db.LOAIMAYs.InsertOnSubmit(loai);
                db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            LOAIMAY lm = db.LOAIMAYs.SingleOrDefault(n => n.MALOAI == id);
            if (lm == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lm);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_loai = db.LOAIMAYs.First(m => m.MALOAI == id);
            var E_tenloai = collection["TenLoai"];
            E_loai.MALOAI = id;
            UpdateModel(E_loai);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var D_LM = db.LOAIMAYs.FirstOrDefault(m => m.MALOAI == id);
            return View(D_LM);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_LM = db.LOAIMAYs.FirstOrDefault(m => m.MALOAI == id);
            var I_SP = from pk in db.SANPHAMs where pk.MALOAI == id select pk.MALOAI;
            if (D_LM.MALOAI == id)
            {

                foreach (var pk in I_SP)
                {
                    if (D_LM.MALOAI == pk)
                    {
                        return View(D_LM);
                    }
                }
                db.LOAIMAYs.DeleteOnSubmit(D_LM);
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