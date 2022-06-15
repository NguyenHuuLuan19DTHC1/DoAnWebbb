using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWebbb.Models;

namespace DoAnWebbb.Areas.Admin.Controllers
{
    public class QLHangMayController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        // GET: Admin/QLHangMay
        public ActionResult Index( string searchString )
        {

            ViewBag.Key=searchString;
            
            if (!string.IsNullOrEmpty(searchString))
            {
                return View(db.NHACUNGCAPs.OrderBy(n => n.MANCC).Where(s => s.TENNCCC.Contains(searchString)));
            }
            return View(db.NHACUNGCAPs.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]

        [ValidateInput(false)]
        public ActionResult Create(NHACUNGCAP nhacc, FormCollection collection)
        {
            var ten = collection["TenNCC"];
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
    }
}