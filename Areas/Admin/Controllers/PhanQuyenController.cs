using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using DoAnWebbb.Models;

namespace DoAnWebbb.Areas.Admin.Controllers
{
    public class PhanQuyenController : Controller
    {
        // GET: Admin/PhanQuyen
        MyDataDataContext db = new MyDataDataContext();
        public ActionResult Index(int? page)
        {
            int pageNum = (page ?? 1);
            int pageSize = 5;
            return View(db.QUYENs.Where(model => model.MAQUYEN != 0).ToList().ToPagedList(pageNum, pageSize));
        }

        public ActionResult TaoQuyen()
        {
            return View();
        }
    }
}