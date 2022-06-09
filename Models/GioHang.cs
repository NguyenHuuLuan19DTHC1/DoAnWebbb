using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoAnWebbb.Models;


namespace DoAnWebbb.Models
{
    public class GioHang
    {
        MyDataDataContext data = new MyDataDataContext();
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string Hinh { get; set; }
        public double GiaSP { get; set; }
        public int SoLuong { get; set; }
        public int? SoLuongTon { get; set; }
        public double ThanhTien
        {
            get { return SoLuong * GiaSP; }
        }
        public GioHang(int id)
        {
            MaSP = id;
            SANPHAM sanpham = data.SANPHAMs.Single(n => n.MASANPHAM == MaSP);
            TenSP = sanpham.TENSANPHAM;
            Hinh = sanpham.HINH;
            GiaSP = double.Parse(sanpham.GIABAN.ToString());
            SoLuong = 1;
            SoLuongTon = sanpham.SOLUONGTON;
        }
    }
}