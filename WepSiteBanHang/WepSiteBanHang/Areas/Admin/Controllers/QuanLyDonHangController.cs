using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WepSiteBanHang.Models;
namespace WepSiteBanHang.Areas.Admin.Controllers
{
    public class QuanLyDonHangController : Controller
    {
        // GET: Admin/QuanLyDonHang
        DBBanHangEntities db = new DBBanHangEntities();
        public ActionResult ChuaThanhToan()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Số lượng người truy cập từ application đã được tạo
            ViewBag.SoLuongNguoiOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();//Lấy số lượng người đang truy cập
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();//Thống kê tổng doanh thu
            ViewBag.TongDDH = ThongKeDonHang();//Thống kê dơn hàng
            ViewBag.TongThanhVien = ThongKeThanhVien();//Thống kê thành viên
            //Lấy danh sách các đơn hàng Chưa duyệt
            var lst = db.DONDATHANGs.Where(n => n.DATHANHTOAN == 0).OrderBy(n => n.NGAYDAT);
            return View(lst);
        }
        public ActionResult ChuaGiao()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Số lượng người truy cập từ application đã được tạo
            ViewBag.SoLuongNguoiOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();//Lấy số lượng người đang truy cập
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();//Thống kê tổng doanh thu
            ViewBag.TongDDH = ThongKeDonHang();//Thống kê dơn hàng
            ViewBag.TongThanhVien = ThongKeThanhVien();//Thống kê thành viên
            //Lấy danh sách đơn hàng chưa giao 
            var lstDSDHCG = db.DONDATHANGs.Where(n => n.TINHTRANGGIAO == 0 && n.DATHANHTOAN == 0).OrderBy(n => n.NGAYGIAO);
            return View(lstDSDHCG);
        }
        public ActionResult DaGiaoDaThanhToan()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Số lượng người truy cập từ application đã được tạo
            ViewBag.SoLuongNguoiOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();//Lấy số lượng người đang truy cập
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();//Thống kê tổng doanh thu
            ViewBag.TongDDH = ThongKeDonHang();//Thống kê dơn hàng
            ViewBag.TongThanhVien = ThongKeThanhVien();//Thống kê thành viên
            //Lấy danh sách đơn hàng chưa giao 
            var lstDSDHCG = db.DONDATHANGs.Where(n => n.TINHTRANGGIAO == 1 && n.DATHANHTOAN == 1);
            return View(lstDSDHCG);
        }

        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Số lượng người truy cập từ application đã được tạo
            ViewBag.SoLuongNguoiOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();//Lấy số lượng người đang truy cập
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();//Thống kê tổng doanh thu
            ViewBag.TongDDH = ThongKeDonHang();//Thống kê dơn hàng
            ViewBag.TongThanhVien = ThongKeThanhVien();//Thống kê thành viên
            //Kiểm tra xem id hợp lệ không
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONDATHANG model = db.DONDATHANGs.SingleOrDefault(n => n.MADDH == id);
           
            //Kiểm tra đơn hàng có tồn tại không
            if (model == null)
            {
                return HttpNotFound();
            }
            //Lấy danh sách chi tiết đơn hàng để hiển thị cho người dùng thấy
            var lstChiTietDH = db.CHITIETDONDATHANGs.Where(n => n.MADDH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);
        }
        [HttpPost]
        public ActionResult DuyetDonHang(DONDATHANG ddh,int? id)
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Số lượng người truy cập từ application đã được tạo
            ViewBag.SoLuongNguoiOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();//Lấy số lượng người đang truy cập
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();//Thống kê tổng doanh thu
            ViewBag.TongDDH = ThongKeDonHang();//Thống kê dơn hàng
            ViewBag.TongThanhVien = ThongKeThanhVien();//Thống kê thành viên

            //Truy vấn lấy ra dữ liệu của đơn hàn đó 
            DONDATHANG ddhUpdate = db.DONDATHANGs.Single(n => n.MADDH == id);
            ddhUpdate.DATHANHTOAN = ddh.DATHANHTOAN;
            ddhUpdate.TINHTRANGGIAO = ddh.TINHTRANGGIAO;
            UpdateModel(ddhUpdate);
            db.SaveChanges();
         
            //Lấy danh sách chi tiết đơn hàng để hiển thị cho người dùng thấy
            var lstChiTietDH = db.CHITIETDONDATHANGs.Where(n => n.MADDH == ddh.MADDH);
            ViewBag.ListChiTietDH = lstChiTietDH;
            var lst = db.CHITIETDONDATHANGs.Where(n => n.MADDH == ddh.MADDH).ToList();
            foreach (var item in lst)
            {
                if (ddhUpdate.DATHANHTOAN == 1)
                {

                    SANPHAM sp = db.SANPHAMs.Single(n => n.MASP == item.MASP);
                    sp.SOLUONGTON = sp.SOLUONGTON - item.SOLUONG;
                    UpdateModel(sp);
                    db.SaveChanges();
                }
            }
            //Gửi khách hàng 1 mail để xác nhận việc thanh toán 
            GuiEmail("Xác đơn hàng ", "duykhanhkontum123@gmail.com", "ksshop.com.vn@gmail.com", "google123456", "Đơn hàng của bạn đã được đặt thành công!");
            return View(ddhUpdate);
        }
        public void GuiEmail(string Title, string ToEmail, string FromEmail, string PassWord, string Content)
        {
            // goi email
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail); // Địa chỉ nhận
            mail.From = new MailAddress(ToEmail); // Địa chửi gửi
            mail.Subject = Title;  // tiêu đề gửi
            mail.Body = Content;                 // Nội dung
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; // host gửi của Gmail
            smtp.Port = 587;               //port của Gmail
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            (FromEmail, PassWord);//Tài khoản password người gửi
            smtp.EnableSsl = true;   //kích hoạt giao tiếp an toàn SSL
            smtp.Send(mail);   //Gửi mail đi
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