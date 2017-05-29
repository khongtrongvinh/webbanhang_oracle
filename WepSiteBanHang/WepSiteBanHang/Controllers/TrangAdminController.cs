using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WepSiteBanHang.Models;

namespace WepSiteBanHang.Controllers
{
    public class TrangAdminController : Controller
    {
        // GET: TrangAdmin
        DBBanHangEntities db = new DBBanHangEntities();
        public ActionResult Index()
        {
            return View();
        }
    }
}