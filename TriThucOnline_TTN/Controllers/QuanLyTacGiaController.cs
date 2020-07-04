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
        SQL_KhoaHoc db = new SQL_KhoaHoc();
        public ActionResult Index(int PageNum = 1, int PageSize = 5, string search = null)
        {
            ViewBag.PageSize = PageSize;
            var lst = db.Database.SqlQuery<TACGIA>("select tl.* from TACGIA as tl").ToList();

            if (search != null)
            {
                lst = lst.Where(x => x.TenTG.ToLower().Trim().Contains(search.ToLower().Trim())).OrderByDescending(x => x.MaTG).ToList();
            }
            return View(lst.ToList().ToPagedList<TACGIA>(PageNum, PageSize));
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
        public ActionResult Edit(int id)
        {
            //Lấy ra đối tượng sách theo mã 
            TACGIA tg = db.TACGIAs.SingleOrDefault(n => n.MaTG == id);
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

        public ActionResult Delete(int id)
        {
            TACGIA cd = db.TACGIAs.SingleOrDefault(n => n.MaTG == id);
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.TACGIAs.Remove(cd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult ExsitsName(string TenTG, int? MaTG)
        {
            List<TACGIA> TacGias = db.TACGIAs.ToList();
            if (MaTG != null)
            {
                TacGias.Remove(db.TACGIAs.Find(MaTG));
            }
            return Json(!TacGias.Any(x => x.TenTG.Trim().ToLower() == TenTG.Trim().ToLower()), JsonRequestBehavior.AllowGet);
        }

    }
}