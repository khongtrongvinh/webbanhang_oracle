using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WepSiteBanHang.Models;
using PagedList;
namespace WepSiteBanHang.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        DBBanHangEntities db = new DBBanHangEntities();
        public ActionResult KQTimKiem(string sTuKhoa, int? page)
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            //tìm kiếm theo ten sản phẩm
            var lstSP = db.SANPHAMs.Where(n => n.TENSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return View(lstSP.OrderBy(n => n.TENSP).ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult LayTuKhoaTimKiem(string sTuKhoa)
        {
            //Gọi về hàm get tìm kiếm
            return RedirectToAction("KQTimKiem", new { @sTuKhoa = sTuKhoa });
        }
        public ActionResult KQTimKiemPartial(string sTuKhoa,FormCollection col)
        {   
            int? gia = int.Parse(col["sGial"]);
            int gian = int.Parse(col["sGian"]);
            if (gia == gian)
            {
                var lstSP = db.SANPHAMs.Where(n => n.TENSP.Contains(sTuKhoa));
                ViewBag.TuKhoa = sTuKhoa;
                return PartialView(lstSP.OrderBy(n => n.DONGIA));
            }
            else
            {
                if (gian > gia)
                {
                    var lstSP = db.SANPHAMs.Where(n => n.TENSP.Contains(sTuKhoa)).Where(n => n.DONGIA > gia).Where(n => n.DONGIA < gian);
                    ViewBag.TuKhoa = sTuKhoa;
                    return PartialView(lstSP.OrderBy(n => n.DONGIA));
                }
                else
                {
                    var lstSP = db.SANPHAMs.Where(n => n.TENSP.Contains(sTuKhoa)).Where(n => n.DONGIA > gia);
                    ViewBag.TuKhoa = sTuKhoa;
                    return PartialView(lstSP.OrderBy(n => n.DONGIA));
                }
            }

            //tìm kiếm theo ten sản phẩm
     
            
        }
    }
    
}