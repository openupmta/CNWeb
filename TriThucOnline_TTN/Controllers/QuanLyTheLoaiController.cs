using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TriThucOnline_TTN.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
namespace TriThucOnline_TTN.Controllers
{
    [Authorize]
    public class QuanLyTheLoaiController : Controller
    {
        // GET: QuanLyChuDe
        SQL_TriThucOnline_BanSachEntities db = new SQL_TriThucOnline_BanSachEntities();
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(db.THELOAIs.ToList().OrderBy(n => n.TenTL).ToPagedList(pageNumber, pageSize));
        }
        public PartialViewResult IndexPartial(int ? page)
        {
            int pageNumber = ( page ?? 1);
            int pageSize = 10;
            return PartialView(db.THELOAIs.ToList().OrderBy(n => n.TenTL).ToPagedList(pageNumber, pageSize));
        }
        /// <summary>
        /// Tao moi
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(THELOAI cd)
        {
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                db.THELOAIs.Add(cd);
                db.SaveChanges();
                ViewBag.ThongBao = "Thêm mới thành công";
            }
            else {
                ViewBag.ThongBao = "Thêm mới thất bại";
                return View(cd);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int MaTheLoai)
        {
            //Lấy ra đối tượng sách theo mã 
            THELOAI cd = db.THELOAIs.SingleOrDefault(n => n.MaTL == MaTheLoai);
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            
            return View(cd);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(THELOAI cd, FormCollection f)
        {
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(cd).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                ViewBag.ThongBao = "Lỗi";
                return View(cd);
            }

            return RedirectToAction("Index");

        }

        [HttpPost]
        public JsonResult XemCTCD(int macd)
        {
            TempData["macd"] = macd;
            return Json(new { Url = Url.Action("XemCTCDPartial") });
        }
        public PartialViewResult XemCTCDPartial()
        {
            int maCD = (int)TempData["macd"];
            var lstCD = db.THELOAIs.Where(n => n.MaTL == maCD).ToList();
            return PartialView(lstCD);
        }
        
        /////////////////////
        [HttpPost]
        public JsonResult Remove(int id)
        {
            THELOAI cd = db.THELOAIs.SingleOrDefault(n => n.MaTL == id);
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.THELOAIs.Remove(cd);
            db.SaveChanges();
            return Json(new { Url = Url.Action("IndexPartial") });
        }

    }
}