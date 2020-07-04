using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TriThucOnline_TTN.Models;
using System.IO;
using PagedList;

namespace TriThucOnline_TTN.Controllers
{
    [Authorize]
    public class QuanLySachController : Controller
    {
        // GET: QuanLyDAUSACH
        SQL_TriThucOnline_BanSachEntities db = new SQL_TriThucOnline_BanSachEntities();
        
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(db.DAUSACHes.ToList().OrderBy(n => n.MaSach).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Search(string search = "")
        {
            ViewBag.search = search;
            return View(db.DAUSACHes.Where(temp => temp.TenSach.Contains(search)).ToList());
        }
        public PartialViewResult IndexPartial(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return PartialView(db.DAUSACHes.ToList().OrderBy(n => n.MaSach).ToPagedList(pageNumber, pageSize));
        }
        //Thêm mới 
        [HttpGet]
        public ActionResult ThemMoi()
        {
            DAUSACH sach = new DAUSACH();
            //Đưa dữ liệu vào dropdownlist
            ViewBag.MaTL = new SelectList(db.THELOAIs.ToList().OrderBy(n => n.TenTL), "MaTL", "TenTL");
            ViewBag.MaNXB = new SelectList(db.NXBs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            ViewBag.MaTG = new SelectList(db.TACGIAs.ToList().OrderBy(n => n.TenTG), "MaTG", "TenTG");
            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(DAUSACH sach, HttpPostedFileBase fileUpload)
        {


            //Đưa dữ liệu vào dropdownlist
            ViewBag.MaTL = new SelectList(db.THELOAIs.ToList().OrderBy(n => n.TenTL), "MaTL", "TenTL", sach.MaTL);
            ViewBag.MaNXB = new SelectList(db.NXBs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            ViewBag.MaTG = new SelectList(db.TACGIAs.ToList().OrderBy(n => n.TenTG), "MaTG", "TenTG", sach.MaTG);
            //kiểm tra đường dẫn ảnh bìa
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Hãy chọn ảnh bìa!";
                return View(sach);
            }
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                //Lưu tên file
                var fileName = Path.GetFileName(fileUpload.FileName);
                //Lưu đường dẫn của file
                var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), fileName);
                //Kiểm tra hình ảnh đã tồn tại chưa
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    fileUpload.SaveAs(path);
                }
                sach.PicBook = fileUpload.FileName;
                db.DAUSACHes.Add(sach);
                db.SaveChanges();
            }
            else
            {
                return View(sach);
            }
            
            return RedirectToAction("Index");
            //return View();
        }
        //Chỉnh sửa sản phẩm
        [HttpGet]
        public ActionResult ChinhSua(int MaSach)
        {
            
            //Lấy ra đối tượng sách theo mã 
            DAUSACH sach = db.DAUSACHes.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Đưa dữ liệu vào dropdownlist
            ViewBag.MaTL = new SelectList(db.THELOAIs.ToList().OrderBy(n => n.TenTL), "MaTL", "TenTL", sach.MaTL);
            ViewBag.MaNXB = new SelectList(db.NXBs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            ViewBag.MaTG = new SelectList(db.TACGIAs.ToList().OrderBy(n => n.TenTG), "MaTG", "TenTG", sach.MaTG);
            ViewBag.Moi = sach.Moi;
            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(DAUSACH sach, HttpPostedFileBase fileUpload)
        {
            if (fileUpload != null)
            {
                //Lưu tên file
                var fileName = Path.GetFileName(fileUpload.FileName);
                //Lưu đường dẫn của file
                var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), fileName);
                if (!System.IO.File.Exists(path))
                {
                    fileUpload.SaveAs(path);
                }
                sach.PicBook = fileName;
            }
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(sach).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //Đưa dữ liệu vào dropdownlist
            ViewBag.MaTL = new SelectList(db.THELOAIs.ToList().OrderBy(n => n.TenTL), "MaTL", "TenTL", sach.MaTL);
            ViewBag.MaNXB = new SelectList(db.NXBs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            ViewBag.MaTG = new SelectList(db.TACGIAs.ToList().OrderBy(n => n.TenTG), "MaTG", "TenTG", sach.MaTG);
            return View(sach);
        }
        //Hiển thị sản phẩm
        public ActionResult HienThi(int MaSach)
        {

            //Lấy ra đối tượng sách theo mã 
            DAUSACH sach = db.DAUSACHes.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(sach);

        }

        [HttpPost]
        public JsonResult XemCTSACH(int masach)
        {
            TempData["masach"] = masach;
            return Json(new { Url = Url.Action("XemCTSACHPartial") });
        }
        public PartialViewResult XemCTSACHPartial()
        {
            int maDAUSACH = (int)TempData["masach"];
            var lstDAUSACH = db.DAUSACHes.Where(n => n.MaSach == maDAUSACH).ToList();
            return PartialView(lstDAUSACH);
        }

        [HttpPost]
        public JsonResult Remove(int id)
        {
            DAUSACH sach = db.DAUSACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), sach.PicBook);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            db.DAUSACHes.Remove(sach);
            db.SaveChanges();
            return Json(new { Url = Url.Action("IndexPartial") });
        }
    }
}