using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWebbb.Models;
using OfficeOpenXml;
using PagedList;
using PagedList.Mvc;

namespace DoAnWebbb.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        public ActionResult Index(int? page)
        {
            ViewBag.DonHang = db.HOADONs.Count(i=>i.TONGTIEN>0);
            ViewBag.LoiNhuan = db.HOADONs.Where(m=>m.PHIEUMUA.TRANGTHAI!=1).Sum(model => model.TONGTIEN);
            ViewBag.NguoiDung = db.NGUOIDUNGs.Where(model => model.MAQUYEN ==2).Count();
            ViewBag.SanPham = db.SANPHAMs.Count();
            var all = from a in db.PHIEUMUAs
                      from b in db.HOADONs
                      where a.MAPHIEUMUA == b.MAPHIEUMUA
                      where b.TONGTIEN > 0
                      select a;
            int pageNum = (page ?? 1);
            int pageSize = 7;
            return View(all.OrderBy(n => n.NGAYDAT).ToPagedList(pageNum, pageSize));
        }
        public ActionResult Info()
        {
            
            NGUOIDUNG tk = (NGUOIDUNG)Session["TaiKhoanAD"];
            
            return View(tk);
        }
        public ActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePass(FormCollection collection)
        {
            NGUOIDUNG tk = (NGUOIDUNG)Session["TaiKhoanAD"];
            var nd = from n in db.NGUOIDUNGs where (n.USERNAME == tk.USERNAME) select n;
            tk.PASS = collection["PassAD"];
            foreach (var a in nd)
            {
                a.PASS = tk.PASS;
            }
            UpdateModel(nd);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult LogOut()
        {
            Session["TaiKhoanAD"] = null;
            return RedirectToAction("Login", "Home");
        }
        public void ExportExcel_EPPLUS()
        {


            var hoadon = from a in db.PHIEUMUAs
                         from b in db.HOADONs
                         where a.MAPHIEUMUA == b.MAPHIEUMUA
                         where b.TONGTIEN > 0
                         select a;
            /*            for (int i = 0; i < 10; i++)
                        {
                            list.Add(new Models.HoaDon
                            {

                                USERNAME = "user" + (i + 1),
                                HOVATEN = "Nguyễn Hữu Luân" + (i + 1),
                                GMAIL = "Ngluan161121@gmail.com" + (i + 1),
                                SDT = "0377153336",
                                DIACHIGIAOHANG = "TEST",
                                NGAYDAT = DateTime.Now.AddYears(20),
                            });
                        }*/
            ExcelPackage ep = new ExcelPackage();
            ExcelWorksheet Sheet = ep.Workbook.Worksheets.Add("BaoCao");
            Sheet.Cells["A1"].Value = "Tài Khoản";
            Sheet.Cells["B1"].Value = "Họ Và Tên";
            Sheet.Cells["C1"].Value = "Gmail";
            Sheet.Cells["D1"].Value = "Số Điện Thoại";
            Sheet.Cells["E1"].Value = "Địa Chỉ Giao Hàng";
            Sheet.Cells["F1"].Value = "Mã phiếu mua";
            Sheet.Cells["G1"].Value = "Ngày Đặt";
            int row = 2; // dòng bắt đầu ghi dữ liệu
            foreach (var item in hoadon)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = item.USERNAME;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.NGUOIDUNG.HOVATEN;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.NGUOIDUNG.GMAIL;
                Sheet.Cells[string.Format("D{0}", row)].Value = item.NGUOIDUNG.SDT;
                Sheet.Cells[string.Format("E{0}", row)].Value = item.NGUOIDUNG.DIACHI;
                Sheet.Cells[string.Format("F{0}", row)].Value = item.MAPHIEUMUA;
                Sheet.Cells[string.Format("G{0}", row)].Value = item.NGAYDAT.ToString();
                row++;
            }
            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + "BaoCao.xlsx");
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();
        }
    }
}