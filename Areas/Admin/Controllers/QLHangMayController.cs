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
        public ActionResult Index()
        {
            return View(db.NHACUNGCAPs.ToList());
        }
    }
}