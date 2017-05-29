using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WepSiteBanHang.Models;
using PagedList;
namespace WepSiteBanHang.Controllers
{
    public class SANPHAMController : Controller
    {
        DBBanHangEntities db = new DBBanHangEntities();
        // GET: SANPHAM
        public ActionResult SANPHAMStyle1Partial()
        {

            return PartialView();
        }
        public ActionResult SANPHAMStyle2Partial()
        {

            return PartialView();
        }
        public ActionResult XemChiTiet(int ?id)
        {
            SANPHAM s = db.SANPHAMs.SingleOrDefault(n => n.MASP == id);
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                s.LUOTXEM++;
                UpdateModel(s);
                db.SaveChanges();
            }
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MASP == id );
            if(sp==null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }
        public ActionResult SANPHAM(int? MALOAISP, int? MaNSX, int? page)
        {
            //if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            //{
            //    return RedirectToAction("Index","Home");
            //}

            if (MALOAISP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            /*Load sản phẩm dựa theo 2 tiêu chí là Mã loại sản phẩm và mã nhà sản xuất (2 trường
            trong bảng sản phẩm */
            var lstSP = db.SANPHAMs.Where(n => n.MALOAISP == MALOAISP && n.MANSX == MaNSX);
            if (lstSP.Count() == 0)
            {
                //Thông báo nếu như không có sản phẩm đó
                return HttpNotFound();
            }
            //Thực hiện chức năng phân trang
            //Tạo biến số sản phẩm trên trang
            int PageSize = 6;
            //Tạo biến thứ 2: Số trang hiện tại
            
            int PageNumber = (page ?? 1);
            ViewBag.MALOAISP = MALOAISP;
            ViewBag.MaNSX = MaNSX;
        
            return View(lstSP.OrderBy(n => n.MASP).ToPagedList(PageNumber, PageSize));
        }
        public ActionResult SANPHAMLienQuan(int id)
        {
            var sp = db.SANPHAMs.SingleOrDefault(n=>n.MASP==id);
            var loai = db.SANPHAMs.Where(n=>n.MALOAISP==sp.MALOAISP).ToList();
            return PartialView(loai);
        }
        public ActionResult T10Theodoi()
        {
            var sp = db.SANPHAMs.OrderByDescending(n => n.LUOTXEM);
            return PartialView(sp);
        }
          protected void SendMail()
        {
            THANHVIEN t = (THANHVIEN)Session["Thanhvien"];
            var tv = db.THANHVIENs.Single(n => n.MATHANHVIEN == t.MATHANHVIEN);
            // Email Address from where you send the mail
            var fromAddress = "sulo2020@yahoo.com.vn";
            // any address where the email will be sending
            var toAddress = tv.EMAIL;
            //Password of your Email address
            const string fromPassword = "7418495869io";
            // Passing the values and make a email formate to display
            string subject = "Đơn hàng từ công ty Luxuxy";
            string body = "From: Đơn hàng";
            // smtp settings
            var smtp = new System.Net.Mail.SmtpClient();
            {
                smtp.Host = "mail.tenmiencuaban.comm";
                smtp.Port = 25;
                smtp.EnableSsl = false;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                smtp.Timeout = 20000;
            }
            // Passing values to smtp object
            smtp.Send(fromAddress, toAddress, subject, body);
           
        }
    }


}