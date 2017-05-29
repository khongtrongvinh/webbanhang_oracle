using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WepSiteBanHang.Models;
namespace WepSiteBanHang.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        DBBanHangEntities db = new DBBanHangEntities();

        // GET: Admin/Admin

        public ActionResult Index()
        {
            
                ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Số lượng người truy cập từ application đã được tạo
                ViewBag.SoLuongNguoiOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();//Lấy số lượng người đang truy cập
                ViewBag.TongDoanhThu = ThongKeTongDoanhThu();//Thống kê tổng doanh thu
                ViewBag.TongDDH = ThongKeDonHang();//Thống kê dơn hàng
                ViewBag.TongThanhVien = ThongKeThanhVien();//Thống kê thành viên

                return View(db.SANPHAMs.OrderByDescending(n => n.MASP));

            



        }
        [HttpGet]
        public ActionResult TaoMoi()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Số lượng người truy cập từ application đã được tạo
            ViewBag.SoLuongNguoiOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();//Lấy số lượng người đang truy cập
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();//Thống kê tổng doanh thu
            ViewBag.TongDDH = ThongKeDonHang();//Thống kê dơn hàng
            ViewBag.TongThanhVien = ThongKeThanhVien();//Thống kê thành viên

            //
            //Load dropdownlist nhà cung cấp và dropdownlist loại SP
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.OrderBy(n => n.TENNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LOAISANPHAMs.OrderBy(n => n.MALOAISP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.OrderBy(n => n.MANSX), "MaNSX", "TenNSX");
            //Kiểm tra hình ảnh tồn tại chưa

            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(SANPHAM sp, HttpPostedFileBase[] HinhAnh)
        {
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.OrderBy(n => n.TENNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LOAISANPHAMs.OrderBy(n => n.MALOAISP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.OrderBy(n => n.MANSX), "MaNSX", "TenNSX");
            //Kiểm tra hình ảnh tồn tại chưa
            if (HinhAnh[0].ContentLength > 0)
            {
                //Lấy tên hình ảnh
                var fileName = Path.GetFileName(HinhAnh[0].FileName);
                //Lấy hình ảnh chuyển vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), fileName);
                //Nếu hình ảnh có rồi thì xuất ra thoonh báo
                if (System.IO.File.Exists(path))
                {
                    ViewBag.upload = "Hình ảnh đã tồn tại";
                    return View();

                }
                else
                {
                    //Lây hình ảnh đưa vào thư mục hình ảnh SP
                    HinhAnh[0].SaveAs(path);

                    sp.HINHANH = fileName;
                }


            }
            db.SANPHAMs.Add(sp);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Số lượng người truy cập từ application đã được tạo
            ViewBag.SoLuongNguoiOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();//Lấy số lượng người đang truy cập
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();//Thống kê tổng doanh thu
            ViewBag.TongDDH = ThongKeDonHang();//Thống kê dơn hàng
            ViewBag.TongThanhVien = ThongKeThanhVien();//Thống kê thành viên

            //Lấy sản phẩm cần chỉnh sửa dựa vào id
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

            //Load dropdownlist nhà cung cấp và dropdownlist loại sp, mã nhà sản xuất
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.OrderBy(n => n.TENNCC), "MaNCC", "TenNCC", sp.MANCC);
            ViewBag.MaLoaiSP = new SelectList(db.LOAISANPHAMs.OrderBy(n => n.MALOAISP), "MaLoaiSP", "TenLoai", sp.MALOAISP);
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.OrderBy(n => n.MANSX), "MaNSX", "TenNSX", sp.MANSX);
            return View(sp);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SANPHAM model, HttpPostedFileBase fileUpload, int id)
        {
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.OrderBy(n => n.TENNCC), "MaNCC", "TenNCC", model.MANCC);
            ViewBag.MaLoaiSP = new SelectList(db.LOAISANPHAMs.OrderBy(n => n.MALOAISP), "MaLoaiSP", "TenLoai", model.MALOAISP);
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.OrderBy(n => n.MANSX), "MaNSX", "TenNSX", model.MANSX);
            //Nếu dữ liệu đầu vào chắn chắn ok 
            var sua = db.SANPHAMs.First(n => n.MASP == id);


            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                UpdateModel(sua);
                db.SaveChanges();
                return this.ChinhSua(id);
            }

            else
            {
                if (ModelState.IsValid)
                {

                    var fileName = Path.GetFileName(fileUpload.FileName);

                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), fileName);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";

                    }
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    sua.HINHANH = fileName;
                    sua.MASP = id;
                    UpdateModel(sua);
                    db.SaveChanges();
                }

                return this.ChinhSua(id);


            }
        }


        [HttpGet]
        public ActionResult Xoa(int? id)
        {

            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Số lượng người truy cập từ application đã được tạo
            ViewBag.SoLuongNguoiOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();//Lấy số lượng người đang truy cập
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();//Thống kê tổng doanh thu
            ViewBag.TongDDH = ThongKeDonHang();//Thống kê dơn hàng
            ViewBag.TongThanhVien = ThongKeThanhVien();//Thống kê thành viên
            //Lấy sản phẩm cần chỉnh sửa dựa vào id
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

            //Load dropdownlist nhà cung cấp và dropdownlist loại sp, mã nhà sản xuất
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.OrderBy(n => n.TENNCC), "MaNCC", "TenNCC", sp.MANCC);
            ViewBag.MaLoaiSP = new SelectList(db.LOAISANPHAMs.OrderBy(n => n.MALOAISP), "MaLoaiSP", "TenLoai", sp.MALOAISP);
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.OrderBy(n => n.MANSX), "MaNSX", "TenNSX", sp.MANSX);
            return View(sp);
        }

        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM model = db.SANPHAMs.SingleOrDefault(n => n.MASP == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.SANPHAMs.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    
    [HttpGet]
        public ActionResult Login()
        {

            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Số lượng người truy cập từ application đã được tạo
            ViewBag.SoLuongNguoiOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();//Lấy số lượng người đang truy cập
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();//Thống kê tổng doanh thu
            ViewBag.TongDDH = ThongKeDonHang();//Thống kê dơn hàng
            ViewBag.TongThanhVien = ThongKeThanhVien();//Thống kê thành viên

            return View();
        }
       [HttpPost]
        public ActionResult Login(FormCollection collection)
        {

            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Số lượng người truy cập từ application đã được tạo
            ViewBag.SoLuongNguoiOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();//Lấy số lượng người đang truy cập
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();//Thống kê tổng doanh thu
            ViewBag.TongDDH = ThongKeDonHang();//Thống kê dơn hàng
            ViewBag.TongThanhVien = ThongKeThanhVien();//Thống kê thành viên

            var tk = collection["taikhoan"];
            var mk = collection["password"];
            if (String.IsNullOrEmpty(tk))
            {
                ViewData["Loi1"] = "Chưa nhập tài khoản";
            }
            else
            {
                if (String.IsNullOrEmpty(mk))
                {
                    ViewData["Loi2"] = "Chưa nhập mật khẩu";
                }
                else
                {
                    QUANLY ad = db.QUANLies.SingleOrDefault(m => m.TENTK == tk && m.PASS == mk);
                    if (ad != null)
                    {
                        Session["Admin"] = ad;
                        return RedirectToAction("Index","Admin");
                    }
                    else
                    {
                        ViewData["Loi2"] = "Tài khoản hoặc mật khẩu sai";
                    }
                }
            }

            return RedirectToAction("Index", "Admin");
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
        public decimal ThongKeTongDoanhThuThang(int Thang, int Nam)
        {
            //Thống kê theo tất cả doanh thu
            //List ra những đơn hàng nào có tháng năm tương ứng
            var lstDDH = db.DONDATHANGs.Where(n => n.NGAYDAT.Value.Month == Thang && n.NGAYDAT.Value.Year == Nam);
            decimal TongTien = 0;
            //Duyệt chi tiết của đơn đặt hàng và lấy tổng tiền  của tất cả sản phẩm thuộc đơn hàng đó
            foreach (var item in lstDDH)
            {
                TongTien += decimal.Parse(item.CHITIETDONDATHANGs.Sum(n => n.SOLUONG * n.DONGIA).ToString());
            }
            return TongTien;
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