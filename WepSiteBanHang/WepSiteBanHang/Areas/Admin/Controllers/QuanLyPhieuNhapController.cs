using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WepSiteBanHang.Models;
namespace WepSiteBanHang.Areas.Admin.Controllers
{
    public class QuanLyPhieuNhapController : Controller
    {
        // GET: Admin/QuanLyPhieuNhap
        DBBanHangEntities db = new DBBanHangEntities();
        [HttpGet]
        public ActionResult NhapHang()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Số lượng người truy cập từ application đã được tạo
            ViewBag.SoLuongNguoiOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();//Lấy số lượng người đang truy cập
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();//Thống kê tổng doanh thu
            ViewBag.TongDDH = ThongKeDonHang();//Thống kê dơn hàng
            ViewBag.TongThanhVien = ThongKeThanhVien();//Thống kê thành viên
            //
            ViewBag.MaNCC = db.NHACUNGCAPs;
            ViewBag.ListSanPham = db.SANPHAMs;
            return View();
        }

        [HttpPost]
        public ActionResult NhapHang(PHIEUNHAP model, IEnumerable<CHITIETPHIEUNHAP> lstModel)
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Số lượng người truy cập từ application đã được tạo
            ViewBag.SoLuongNguoiOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();//Lấy số lượng người đang truy cập
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();//Thống kê tổng doanh thu
            ViewBag.TongDDH = ThongKeDonHang();//Thống kê dơn hàng
            ViewBag.TongThanhVien = ThongKeThanhVien();//Thống kê thành viên

            ViewBag.MaNCC = db.NHACUNGCAPs;
            ViewBag.ListSanPham = db.SANPHAMs;
            //Sau khi các bạn đã kiểm tra tất cả dữ liệu đầu vào
            //Gán đã xóa: False
            model.DAXOA = 1;
            db.PHIEUNHAPs.Add(model);
            db.SaveChanges();
            //SaveChanges để lấy được mã phiếu nhập gán cho lstChiTietPhieuNhap
            SANPHAM sp;
            foreach (var item in lstModel)
            {
                //Cập nhật số lượng tồn
                sp = db.SANPHAMs.Single(n => n.MASP == item.MASP);
                sp.SOLUONGTON += item.SOLUONGNHAP;
                //Gán mã phiếu nhập cho tất cả chi tiết phiếu nhập
                item.MAPN = model.MAPN;
            }
            db.CHITIETPHIEUNHAPs.AddRange(lstModel);
            db.SaveChanges();
            return View();
        }
        [HttpGet]
        public ActionResult DSSHetHang()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Số lượng người truy cập từ application đã được tạo
            ViewBag.SoLuongNguoiOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();//Lấy số lượng người đang truy cập
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();//Thống kê tổng doanh thu
            ViewBag.TongDDH = ThongKeDonHang();//Thống kê dơn hàng
            ViewBag.TongThanhVien = ThongKeThanhVien();//Thống kê thành viên
            var lstSP = db.SANPHAMs.Where(n => n.SOLUONGTON <= 5);
            return View(lstSP);
        }
        [HttpGet]
        public ActionResult NhapHangDon(int? id)
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Số lượng người truy cập từ application đã được tạo
            ViewBag.SoLuongNguoiOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();//Lấy số lượng người đang truy cập
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();//Thống kê tổng doanh thu
            ViewBag.TongDDH = ThongKeDonHang();//Thống kê dơn hàng
            ViewBag.TongThanhVien = ThongKeThanhVien();//Thống kê thành viên

            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.OrderBy(n => n.TENNCC), "MaNCC", "TenNCC");
            //Tương tự như trang chỉnh sửa sản phẩm nhưng ta không cần phải show hết các thuộc tính 
            //Chỉ thuộc tính nào cần thiết mà thôi đó là số lượng tồn hình ảnh... thông tin hiển thị cần thiết
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MASP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);

        }
        [HttpPost]
        public ActionResult NhapHangDon(PHIEUNHAP model, CHITIETPHIEUNHAP ctpn)
        {

            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Số lượng người truy cập từ application đã được tạo
            ViewBag.SoLuongNguoiOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();//Lấy số lượng người đang truy cập
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();//Thống kê tổng doanh thu
            ViewBag.TongDDH = ThongKeDonHang();//Thống kê dơn hàng
            ViewBag.TongThanhVien = ThongKeThanhVien();//Thống kê thành viên

            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.OrderBy(n => n.TENNCC), "MaNCC", "TenNCC", model.MANCC);
            //Sau khi các bạn đã kiểm tra tất cả dữ liệu đầu vào
            //Gán đã xóa: False
            model.NGAYNHAP = DateTime.Now;
            model.DAXOA = 0;
            db.PHIEUNHAPs.Add(model);
            db.SaveChanges();
            //SaveChanges để lấy được mã phiếu nhập gán cho lstChiTietPhieuNhap
            ctpn.MAPN = model.MAPN;
            //Cập nhật tồn 
            SANPHAM sp = db.SANPHAMs.Single(n => n.MASP == ctpn.MASP);
            sp.SOLUONGTON += ctpn.SOLUONGNHAP;
            db.CHITIETPHIEUNHAPs.Add(ctpn);
            db.SaveChanges();
            return View(sp);

        }
        public decimal ThongKeTongDoanhThu()
        {
            //Thống kê theo tất cả doanh thu
            decimal TongDoanhThu = db.CHITIETDONDATHANGs.Sum(n => n.SOLUONG * n.DONGIA);
            return TongDoanhThu;
        }
        public double ThongKeDonHang()
        {
            //Dếm đơn đặt hàng
            double slDDH = db.DONDATHANGs.Count();
            return slDDH;
        }
        public double ThongKeThanhVien()
        {
            //Dếm đơn đặt hàng
            double slTV = db.THANHVIENs.Count();
            return slTV;
        }
        //Giải phóng biến cho vùng nhớ
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}