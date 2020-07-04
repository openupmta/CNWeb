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
    public class QuanLyTacGiaController : Controller
    {
        // GET: QuanLyTacGia
        SQL_TriThucOnline_BanSachEntities db = new SQL_TriThucOnline_BanSachEntities();
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(db.TACGIAs.ToList().OrderBy(n => n.MaTG).ToPagedList(pageNumber, pageSize));
        }
        public PartialViewResult IndexPartial(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return PartialView(db.TACGIAs.ToList().OrderBy(n => n.MaTG).ToPagedList(pageNumber, pageSize));
        }
        /// <summary>
        /// Tao moi
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {

            return View(new TACGIA());
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(TACGIA tg, HttpPostedFileBase fileUpload)
        {
            //kiểm tra đường dẫn ảnh
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Hãy chọn ảnh tác giả!";
                return View(tg);
            }
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                //Lưu tên file
                var fileName = Path.GetFileName(fileUpload.FileName);
                //Lưu đường dẫn của file
                var path = Path.Combine(Server.MapPath("~/Content/HinhAnhTG"), fileName);
                //Kiểm tra hình ảnh đã tồn tại chưa
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    fileUpload.SaveAs(path);
                }
                tg.PicTG = fileUpload.FileName;
                db.TACGIAs.Add(tg);
                db.SaveChanges();
                ViewBag.ThongBao = "Thêm mới thành công";
            }
            else
            {
                ViewBag.ThongBao = "Thêm mới thất bại";
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Chinh Sua
        /// </summary>
        /// <param name="MaSach"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int MaTacGia)
        {
            //Lấy ra đối tượng sách theo mã 
            TACGIA tg = db.TACGIAs.SingleOrDefault(n => n.MaTG == MaTacGia);
            if (tg == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(tg);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(TACGIA tg, HttpPostedFileBase fileUpload)
        {
            if (fileUpload != null)
            {
                //Lưu tên file
                var fileName = Path.GetFileName(fileUpload.FileName);
                //Lưu đường dẫn của file
                var path = Path.Combine(Server.MapPath("~/Content/HinhAnhTG"), fileName);
                if (!System.IO.File.Exists(path))
                {
                    fileUpload.SaveAs(path);
                }
                tg.PicTG = fileName;
            }
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(tg).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tg);

        }

        [HttpPost]
        public JsonResult XemCTTG(int matg)
        {
            TempData["matg"] = matg;
            return Json(new { Url = Url.Action("XemCTTGPartial") });
        }
        public PartialViewResult XemCTTGPartial()
        {
            int maTG = (int)TempData["matg"];
            var lstTG = db.TACGIAs.Where(n => n.MaTG == maTG).ToList();
            return PartialView(lstTG);
        }

        /////////////////////
        [HttpPost]
        public JsonResult Remove(int id)
        {
            TACGIA tg = db.TACGIAs.SingleOrDefault(n => n.MaTG == id);
            if (tg == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var path = Path.Combine(Server.MapPath("~/Content/HinhAnhTG"), tg.PicTG);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            db.TACGIAs.Remove(tg);
            db.SaveChanges();
            return Json(new { Url = Url.Action("IndexPartial") });
        }
    }
}