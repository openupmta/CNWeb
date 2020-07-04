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
    public class QuanLyKhoaHocController : Controller
    {
        // GET: QuanLyChuDe
        SQL_KhoaHoc db = new SQL_KhoaHoc();
        public ActionResult Index(int PageNum = 1, int PageSize = 5, string search = null)
        {
            ViewBag.PageSize = PageSize;
            var lst = db.Database.SqlQuery<KHOAHOC>("select tl.* from KHOAHOC as tl").ToList();

            if (search != null)
            {
                lst = lst.Where(x => x.TenKhoaHoc.ToLower().Trim().Contains(search.ToLower().Trim())).OrderByDescending(x => x.MaKhoaHoc).ToList();
            }
            return View(lst.ToList().ToPagedList<KHOAHOC>(PageNum, PageSize));
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
        public ActionResult Create(KHOAHOC cd)
        {
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                db.KHOAHOCs.Add(cd);
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
        public ActionResult Edit(int id)
        {
            //Lấy ra đối tượng sách theo mã 
            KHOAHOC cd = db.KHOAHOCs.SingleOrDefault(n => n.MaKhoaHoc == id);
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            
            return View(cd);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(KHOAHOC cd)
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
        public ActionResult Delete(int id)
        {
            KHOAHOC cd = db.KHOAHOCs.SingleOrDefault(n => n.MaKhoaHoc == id);
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.KHOAHOCs.Remove(cd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public JsonResult ExsitsName(string TenKhoaHoc, int? MaKhoaHoc)
        {
            List<KHOAHOC> KhoaHocs = db.KHOAHOCs.ToList();
            if (MaKhoaHoc != null)
            {
                KhoaHocs.Remove(db.KHOAHOCs.Find(MaKhoaHoc));
            }
            return Json(!KhoaHocs.Any(x => x.TenKhoaHoc.Trim().ToLower() == TenKhoaHoc.Trim().ToLower()), JsonRequestBehavior.AllowGet);
        }

    }
}