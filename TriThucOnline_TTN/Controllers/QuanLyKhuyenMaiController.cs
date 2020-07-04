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
    public class QuanLyKhuyenMaiController : Controller
    {
        // GET: QuanLyKhuyenMai
        SQL_KhoaHoc db = new SQL_KhoaHoc();
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(db.KHUYENMAIs.ToList().OrderBy(n => n.MaKM).ToPagedList(pageNumber, pageSize));
        }
        public PartialViewResult IndexPartial(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return PartialView(db.KHUYENMAIs.ToList().OrderBy(n => n.MaKM).ToPagedList(pageNumber, pageSize));
        }
        /// <summary>
        /// Tao moi
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            KHUYENMAI km = new KHUYENMAI();
            return View(km);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(KHUYENMAI voucher)
        {
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                db.KHUYENMAIs.Add(voucher);
                db.SaveChanges();
                ViewBag.ThongBao = "Thêm mới thành công";
            }
            else
            {
                ViewBag.ThongBao = "Thêm thất bại";
                return View(voucher);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(string VoucherCode)
        {
            //Lấy ra đối tượng sách theo mã 
            KHUYENMAI voucher = db.KHUYENMAIs.SingleOrDefault(n => n.MaKM == VoucherCode);
            if (voucher == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.SoLanConLai = voucher.SoLanConLai.ToString();
            return View(voucher);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(KHUYENMAI voucher, FormCollection f)
        {
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(voucher).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                ViewBag.ThongBao = "Lỗi";
                return View(voucher);
            }

            return RedirectToAction("Index");

        }

        [HttpPost]
        public JsonResult XemCTKM(string makm)
        {
            TempData["makm"] = makm;
            return Json(new { Url = Url.Action("XemCTKMPartial") });
        }
        public PartialViewResult XemCTKMPartial()
        {
            string maKM = (string)TempData["makm"];
            var lstKM = db.KHUYENMAIs.Where(n => n.MaKM == maKM).ToList();
            return PartialView(lstKM);
        }

        /////////////////////
        [HttpPost]
        public JsonResult Remove(string id)
        {
            KHUYENMAI km = db.KHUYENMAIs.SingleOrDefault(n => n.MaKM == id);
            if (km == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.KHUYENMAIs.Remove(km);
            db.SaveChanges();
            return Json(new { Url = Url.Action("IndexPartial") });
        }
    }
}