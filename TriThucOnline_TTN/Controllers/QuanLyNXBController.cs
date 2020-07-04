using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TriThucOnline_TTN.Models;
using PagedList;
using PagedList.Mvc;
namespace TriThucOnline_TTN.Controllers
{
    [Authorize]
    public class QuanLyNXBController : Controller
    {
        // GET: QuanLyNXB
        SQL_TriThucOnline_BanSachEntities db = new SQL_TriThucOnline_BanSachEntities();
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(db.NXBs.ToList().OrderBy(n => n.MaNXB).ToPagedList(pageNumber, pageSize));
        }
        public PartialViewResult IndexPartial(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return PartialView(db.NXBs.ToList().OrderBy(n => n.MaNXB).ToPagedList(pageNumber, pageSize));
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
        public ActionResult Create(NXB nxb)
        {
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                db.NXBs.Add(nxb);
                db.SaveChanges();
                ViewBag.ThongBao = "Thêm mới thành công";
            }
            else
            {
                ViewBag.ThongBao = "Thêm mới thất bại";
                return View(nxb);
            }
            return RedirectToAction("Index", new { page = db.NXBs.Count()/10});
        }

        [HttpGet]
        public ActionResult Edit(int MaNXB)
        {
            //Lấy ra đối tượng sách theo mã 
            NXB nxb = db.NXBs.SingleOrDefault(n => n.MaNXB == MaNXB);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(nxb);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(NXB nxb, FormCollection f)
        {
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(nxb).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                ViewBag.ThongBao = "Lỗi";
                return View(nxb);
            }

            return RedirectToAction("Index");

        }

        [HttpPost]
        public JsonResult XemCTNXB(int manxb)
        {
            TempData["manxb"] = manxb;
            return Json(new { Url = Url.Action("XemCTNXBPartial") });
        }
        public PartialViewResult XemCTNXBPartial()
        {
            int maNXB = (int)TempData["manxb"];
            var lstNXB = db.NXBs.Where(n => n.MaNXB == maNXB).ToList();
            return PartialView(lstNXB);
        }

        /////////////////////
        [HttpPost]
        public JsonResult Remove(int id)
        {
            NXB nxb = db.NXBs.SingleOrDefault(n => n.MaNXB == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.NXBs.Remove(nxb);
            db.SaveChanges();
            return Json(new { Url = Url.Action("IndexPartial") });
        }
    }
}