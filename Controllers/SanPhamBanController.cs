﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWebbb.Models;
using PagedList;
using PagedList.Mvc;

namespace DoAnWebbb.Controllers
{
    public class SanPhamBanController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        public ActionResult Index(int? page, string searchString)
        {
            ViewBag.MacDinh = db.SANPHAMs.ToList();
            ViewBag.Moi = db.SANPHAMs.ToList().OrderBy(n => n.NGAYCAPNHAT);
            ViewBag.GiaCao = db.SANPHAMs.ToList().OrderByDescending(n => n.GIAGIAM);
            ViewBag.GiaThap = db.SANPHAMs.ToList().OrderBy(n => n.GIAGIAM);
            int pageSize = 15;
            int pageNum = (page ?? 1);
            ViewBag.Key = searchString;
            var SearchName = from s in db.SANPHAMs select s;


            if (!String.IsNullOrEmpty(searchString))
            {
                SearchName = SearchName.Where(s => s.TENSANPHAM.Contains(searchString));               
            }

            return View(SearchName.ToPagedList(pageNum, pageSize));
        }
        public ActionResult HangMay()
        {
            var hangmay = from hang in db.NHACUNGCAPs select hang;
            return PartialView(hangmay);
        }
        
        public ActionResult SearchName(string searchString)
        {
            ViewBag.Key=searchString;
            var SearchName = from s in db.SANPHAMs select s;

            
            if (!String.IsNullOrEmpty(searchString))
            {
                SearchName = SearchName.Where(s => s.TENSANPHAM.Contains(searchString));
            }
            else
            {
                SearchName = SearchName.Where(s => s.TENSANPHAM.Contains("co"));
            }
            return View(SearchName);
        }
        public ActionResult LoaiMay()
        {
            var loaimay = from loai in db.LOAIMAYs select loai;
            return PartialView(loaimay);
        }
        public ActionResult SPTheoHang(int id, string searchString, int? page)
        {
            ViewBag.Key = searchString;
            var SearchName = from sp in db.SANPHAMs where sp.MANCC == id select sp;
            int pageSize = 12;
            int pageNum = (page ?? 1);


            if (!String.IsNullOrEmpty(searchString))
            {
                SearchName = SearchName.Where(s => s.TENSANPHAM.Contains(searchString));
            }

            return View(SearchName.ToPagedList(pageNum,pageSize));
        }
        public ActionResult SPTheoLoai(int id, string searchString, int? page)
        {
            int pageSize = 12;
            int pageNum = (page ?? 1);
            var SearchName = from sp in db.SANPHAMs where sp.MALOAI == id select sp; ;
            ViewBag.Key = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                SearchName = SearchName.Where(s => s.TENSANPHAM.Contains(searchString));
            }

            return View(SearchName.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Details(int id)
        {
            var detail = db.SANPHAMs.Where(m => m.MASANPHAM == id).First();
            return View(detail);

        }
    }
}