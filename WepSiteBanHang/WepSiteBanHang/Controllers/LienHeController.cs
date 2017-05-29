using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WepSiteBanHang.Models;
namespace WepSiteBanHang.Controllers
{
    
    public class LienHeController : Controller
    {
        // GET: LienHe
        DBBanHangEntities db = new DBBanHangEntities();
        public ActionResult LienHe()
        {
            var lh = db.LIENHEs.Single(n => n.TRANGTHAI==1);
      
            return View(lh);
        }
        [HttpPost]
        public ActionResult LienHe(FormCollection colection,LIENHE lh)
        {

            var hoten = colection["HOTEN"];
            var dienthoai = colection["DIENTHOAI"];
            var diachi = colection["DIACHI"];
            var email = colection["EMAIL"];
            var yeucau = colection["YEUCAU"];
            lh.HOTEN = hoten;
            lh.DIENTHOAI = dienthoai;
            lh.DIACHI= diachi;
            lh.EMAIL = email;
            lh.YEUCAU = yeucau;
            db.LIENHEs.Add(lh);
            db.SaveChanges();
            return View(lh);

        }



    }
}