using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WepSiteBanHang.Models;
namespace WepSiteBanHang.Controllers
{
    public class HomeController : Controller
    {
        DBBanHangEntities db = new DBBanHangEntities();
        public ActionResult Index()
        {
            //List LapTop mới nhất
            var lstLT = db.SANPHAMs.Where(n => n.MALOAISP == 3 && n.MOI == 1);
            //Gán vào ViewBag
            ViewBag.ListLTM = lstLT;
            var lstDTM = db.SANPHAMs.Where(n => n.MALOAISP == 1 && n.MOI == 1 );
            //Gán vào ViewBag
            ViewBag.ListDTM = lstDTM;
            var lstMTB = db.SANPHAMs.Where(n => n.MALOAISP == 2 && n.MOI == 1 );
            //Gán vào ViewBag
            ViewBag.ListMTBM = lstMTB;
            return View();
        }
        public ActionResult MenuPartial()
        {
            //Truy vấn lấy về 1 list các sản phẩm
            var lstSP = db.SANPHAMs;
            return PartialView(lstSP);
        }
        public ActionResult About()
        {
            return View();
        }



    }
}