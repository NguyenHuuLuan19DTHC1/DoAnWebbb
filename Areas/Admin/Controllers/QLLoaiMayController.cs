using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWebbb.Models;

namespace DoAnWebbb.Areas.Admin.Controllers
{
    public class QLLoaiMayController : Controller
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
            var ten = collection["TenL"];
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

    }
}