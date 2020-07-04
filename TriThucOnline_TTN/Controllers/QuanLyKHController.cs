using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TriThucOnline_TTN.Models;
using PagedList;
using PagedList.Mvc;
using System.Web.Security;
using System.IO;

namespace TriThucOnline_TTN.Controllers
{
    [Authorize]
    public class QuanLyKHController : Controller
    {
        // GET: QuanLyKH
        SQL_TriThucOnline_BanSachEntities db = new SQL_TriThucOnline_BanSachEntities();
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(db.KHACHHANGs.ToList().OrderBy(n => n.MaKH).ToPagedList(pageNumber, pageSize));
        }
        public PartialViewResult IndexPartial(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return PartialView(db.KHACHHANGs.ToList().OrderBy(n => n.MaKH).ToPagedList(pageNumber, pageSize));
        }
        
        [HttpPost]
        public JsonResult XemCTKH(int makh)
        {
            TempData["makh"] = makh;
            return Json(new { Url = Url.Action("XemCTKHPartial") });
        }
        public PartialViewResult XemCTKHPartial()
        {
            int maKH = (int)TempData["makh"];
            var lstKH = db.KHACHHANGs.Where(n => n.MaKH == maKH).ToList();
            return PartialView(lstKH);
        }

        [HttpPost]
        public JsonResult Remove(int id)
        {
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            if (kh.PicUser != null)
            {
                var path = Path.Combine(Server.MapPath("~/Content/HinhAnhKH"), kh.PicUser);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            db.KHACHHANGs.Remove(kh);
            db.SaveChanges();
            return Json(new { Url = Url.Action("IndexPartial") });
        }

        [HttpPost]
        public JsonResult XemCTDHKH(int makh)
        {
            TempData["makh"] = makh;
            return Json(new { Url = Url.Action("XemCTDHKHPartial") });
        }
        public PartialViewResult XemCTDHKHPartial()
        {
            int maKH = (int)TempData["makh"];
            var lstKH = db.DONHANGs.Where(n => n.MaKH == maKH).ToList();
            return PartialView(lstKH);
        }
    }
}