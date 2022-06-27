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

        public ActionResult Info()
        {
            NGUOIDUNG tk = (NGUOIDUNG)Session["TaiKhoanKH"];

            return View(tk);
        }
        public ActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePass(FormCollection collection)
        {
            NGUOIDUNG tk = (NGUOIDUNG)Session["TaiKhoanKH"];
            var nd = from n in data.NGUOIDUNGs where (n.USERNAME == tk.USERNAME) select n;
            tk.PASS = collection["Pass"];
            foreach(var a in nd)
            {
                a.PASS = tk.PASS;
            }
            UpdateModel(nd);
            data.SubmitChanges();
            return RedirectToAction("Info");
        }
    }

}