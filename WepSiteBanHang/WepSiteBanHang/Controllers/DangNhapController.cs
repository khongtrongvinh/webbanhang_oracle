using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WepSiteBanHang.Models;

namespace WepSiteBanHang.Controllers
{
    public class DangNhapController : Controller
    {
        // GET: DangNhap
        DBBanHangEntities db = new DBBanHangEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {   
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
                        return RedirectToAction("Index", "TrangAdmin");
                    }
                    else
                    {
                        ViewData["Loi2"] = "Tài khoản hoặc mật khẩu sai";
                    }
                }
            }

            return View();
        }
        public ActionResult HienThiTK()
        {
            var ad = (QUANLY)Session["Admin"];
            return PartialView(ad);
        }
        public ActionResult DangXuat()
        {
            Session["Admin"] = null;
            return RedirectToAction("DangNhap", "DangNhap");
        }
    }
}