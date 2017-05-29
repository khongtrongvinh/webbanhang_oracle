using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WepSiteBanHang.Models;
namespace WepSiteBanHang.Controllers
{
    public class GioHangController : Controller
    {
        DBBanHangEntities db = new DBBanHangEntities();

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AddToCart(int id)
        {
            List<CartItem> listCartItem;
            //Process Add To Cart
            if (Session["ShoppingCart"] == null)
            {
                //Create New Shopping Cart Session
                listCartItem = new List<CartItem>();
                listCartItem.Add(new CartItem
                {
                    Quality = 1,
                    productOrder = db.SANPHAMs.Find(id)
                });
                Session["ShoppingCart"] = listCartItem;
            }
            else
            {
                bool flag = false;
                listCartItem = (List<CartItem>)Session["ShoppingCart"];
                foreach (CartItem item in listCartItem)
                {
                    if (item.productOrder.MASP == id)
                    {
                        item.Quality++;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                    listCartItem.Add(new CartItem
                    {
                        Quality = 1,
                        productOrder = db.SANPHAMs.Find(id)
                    });
                Session["ShoppingCart"] = listCartItem;
            }
            //Count item in shopping cart
            int cartcount = 0;
            List<CartItem> ls = (List<CartItem>)Session["ShoppingCart"];
            foreach (CartItem item in ls)
            {
                cartcount += item.Quality;
            }
            return Json(new { ItemAmount = cartcount});
        }
        public ActionResult Giohang()
        {
            return View();
        }
        public ActionResult Update(int id,FormCollection col)
        {
            List<CartItem> ls = (List<CartItem>)Session["ShoppingCart"];
            CartItem sp = ls.SingleOrDefault(n => n.productOrder.MASP == id);
            if(sp != null)
            { 
                sp.Quality = int.Parse(col["Quality"].ToString());
            }
       
            return RedirectToAction("Giohang");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            //Kiem tra dang nhap
            if (Session["Thanhvien"] == null || Session["Thanhvien"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Khachhang");
            }
            if (Session["ShoppingCart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }


            return View();
        }
        //Xay dung chuc nang Dathang
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            DONDATHANG ddh = new DONDATHANG();
            THANHVIEN kh = (THANHVIEN)Session["ThanhVien"];
            List<CartItem> gh = (List<CartItem>)Session["ShoppingCart"];
            ddh.MATHANHVIEN = kh.MATHANHVIEN;
            var ngaydat = DateTime.Now;
            ddh.NGAYDAT = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.NGAYGIAO = DateTime.Parse(ngaygiao);
            if (ddh.NGAYGIAO < DateTime.Now)
            {
                ViewData["Loi"] = "Ngày Giao hàng không đúng";
                return View();

            }
            else
            {
                ddh.TINHTRANGGIAO = 1;
                ddh.DATHANHTOAN = 1;
                db.DONDATHANGs.Add(ddh);
                db.SaveChanges();
                //Them chi tiet don hang            
                foreach (var item in gh)
                {
                    CHITIETDONDATHANG ctdh = new CHITIETDONDATHANG();
                    ctdh.MADDH = ddh.MADDH;
                    ctdh.MASP = item.productOrder.MASP;
                    ctdh.SOLUONG = item.Quality;
                    ctdh.DONGIA = (long)item.productOrder.DONGIA;
                    db.CHITIETDONDATHANGs.Add(ctdh);
                }
                SendMail();
                db.SaveChanges();
                Session["ShoppingCart"] = null;
                return RedirectToAction("Xacnhandonhang", "Giohang");

            }

        }
        public ActionResult Hienkh()
        {
            if (Session["Thanhvien"] == null)
            {
                return RedirectToAction("Dangnhap", "Khachhang");
            }
            else
            {   
                List<KHACHHANG> kh = (List<KHACHHANG>)Session["Thanhvien"];
             
            }
            return PartialView();
        }
        public ActionResult XoaGiohang(int id)
        {
            //Lay gio hang tu Session
            List<CartItem> gh = (List<CartItem>)Session["ShoppingCart"];
            //Kiem tra sach da co trong Session["Giohang"]
            CartItem sp = gh.SingleOrDefault(n => n.productOrder.MASP == id);
            //Neu ton tai thi cho sua Soluong
            if (sp != null)
            {
                gh.RemoveAll(n => n.productOrder.MASP == id);
                return RedirectToAction("GioHang");

            }
            if (gh.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }
        //Xoa tat ca thong tin trong Gio hang
        public ActionResult XoaTatcaGiohang()
        {
            //Lay gio hang tu Session
            List<CartItem> gh = (List<CartItem>)Session["ShoppingCart"];
            gh.Clear();
         
            return RedirectToAction("Index", "Home");
        }
        protected void SendMail()
        {
            THANHVIEN t = (THANHVIEN)Session["Thanhvien"];
            var tv = db.THANHVIENs.Single(n => n.MATHANHVIEN == t.MATHANHVIEN);
            // Email Address from where you send the mail
            var fromAddress = "sulo42229@gmail.com";
            // any address where the email will be sending
            var toAddress = tv.EMAIL;
            //Password of your Email address
            const string fromPassword = "nhan2066";
            // Passing the values and make a email formate to display
            string subject = "Đơn hàng từ công ty Luxuxy";
            string body = "From: Đơn hàng Của " + tv.HOTEN +"\n" ;
            List<CartItem> gh = (List<CartItem>)Session["ShoppingCart"];
            foreach(var item in gh)
            { 
            body += "Sản Phẩm  :" + item.productOrder.TENSP + "\n";
            body += "Số Lượng  :" + item.Quality + "\n";
            body += "Giá :" + item.productOrder.DONGIA + "\n";
            }
            body += "Chúng tôi sẽ gửi hàng 1 cách sớm nhất cho bạn";
            // smtp settings
            var smtp = new System.Net.Mail.SmtpClient();
            {
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(fromAddress,fromPassword);
                smtp.Timeout = 20000;
            }
            // Passing values to smtp object
            smtp.Send(fromAddress,toAddress,subject,body);
            

        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }

    }
}