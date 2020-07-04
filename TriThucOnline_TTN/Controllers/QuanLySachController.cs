using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TriThucOnline_TTN.Models;
using System.IO;
using PagedList;
using TriThucOnline_TTN.Models.ViewModel;

namespace TriThucOnline_TTN.Controllers
{
    [Authorize]
    public class QuanLySachController : Controller
    {
        // GET: QuanLyDAUSACH
        SQL_KhoaHoc db = new SQL_KhoaHoc();
        
        public ActionResult Index(int PageNum = 1, int PageSize = 5, string search = null)
        {
            ViewBag.PageSize = PageSize;
            var lst = db.Database.SqlQuery<DAUSACHViewModel>("select ds.*,tg.TenTG as TenTG, tl.TenKhoaHoc as TenKhoaHoc, nxb.TenNXB as TenNXB" +
               " from DAUSACH as ds" +
               " left join TACGIA as tg on ds.MaTG = tg.MaTG " +
               "left join KHOAHOC as tl on ds.MaKhoaHoc = tl.MaKhoaHoc " +
               "left join NXB as nxb on ds.MaNXB = nxb.MaNXB order by ds.MaSach desc").ToList();

            if (search != null)
            {
                lst = lst.Where(x => x.TenNXB.ToLower().Trim().Contains(search.ToLower().Trim())
                                || x.TenSach.ToLower().Trim().Contains(search.ToLower().Trim())
                                || x.SoLuongTon.ToString().ToLower().Trim().Contains(search.ToLower().Trim())
                                || x.TenKhoaHoc.ToLower().Trim().Contains(search.ToLower().Trim())
                                || x.MaSach.ToString().ToLower().Trim().Contains(search.ToLower().Trim())
                ).OrderByDescending(x => x.MaSach).ToList();
            }
            return View(lst.ToList().ToPagedList<DAUSACHViewModel>(PageNum, PageSize));
        }
        
        //Thêm mới 
        [HttpGet]
        public ActionResult ThemMoi()
        {
            DAUSACH sach = new DAUSACH();
            //Đưa dữ liệu vào dropdownlist
            List<DropDownList> KHOAHOC = new List<DropDownList>();
            List<KHOAHOC> DBKHOAHOC = db.KHOAHOCs.ToList();
            foreach (KHOAHOC tl in DBKHOAHOC)
            {
                DropDownList add = new DropDownList();
                add.ID = tl.MaKhoaHoc;
                add.Name = tl.TenKhoaHoc;
                KHOAHOC.Add(add);
            }
            ViewBag.ViewKHOAHOC = KHOAHOC;

            List<DropDownList> NXB = new List<DropDownList>();
            List<NXB> DBNXB = db.NXBs.ToList();
            foreach (NXB tl in DBNXB)
            {
                DropDownList add = new DropDownList();
                add.ID = tl.MaNXB;
                add.Name = tl.TenNXB;
                NXB.Add(add);
            }
            ViewBag.ViewNXB = NXB;

            List<DropDownList> TacGia = new List<DropDownList>();
            List<TACGIA> DBTacGia = db.TACGIAs.ToList();
            foreach (TACGIA tl in DBTacGia)
            {
                DropDownList add = new DropDownList();
                add.ID = tl.MaTG;
                add.Name = tl.TenTG;
                TacGia.Add(add);
            }
            ViewBag.ViewTacGia = TacGia;

            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(DAUSACH sach, HttpPostedFileBase fileUpload)
        {


            //Đưa dữ liệu vào dropdownlist
            List<DropDownList> KHOAHOC = new List<DropDownList>();
            List<KHOAHOC> DBKHOAHOC = db.KHOAHOCs.ToList();
            foreach (KHOAHOC tl in DBKHOAHOC)
            {
                DropDownList add = new DropDownList();
                add.ID = tl.MaKhoaHoc;
                add.Name = tl.TenKhoaHoc;
                KHOAHOC.Add(add);
            }
            ViewBag.ViewKHOAHOC = KHOAHOC;

            List<DropDownList> NXB = new List<DropDownList>();
            List<NXB> DBNXB = db.NXBs.ToList();
            foreach (NXB tl in DBNXB)
            {
                DropDownList add = new DropDownList();
                add.ID = tl.MaNXB;
                add.Name = tl.TenNXB;
                NXB.Add(add);
            }
            ViewBag.ViewNXB = NXB;

            List<DropDownList> TacGia = new List<DropDownList>();
            List<TACGIA> DBTacGia = db.TACGIAs.ToList();
            foreach (TACGIA tl in DBTacGia)
            {
                DropDownList add = new DropDownList();
                add.ID = tl.MaTG;
                add.Name = tl.TenTG;
                NXB.Add(add);
            }
            ViewBag.ViewTacGia = TacGia;

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
            List<DropDownList> KHOAHOC = new List<DropDownList>();
            List<KHOAHOC> DBKHOAHOC = db.KHOAHOCs.ToList();
            foreach(KHOAHOC tl in DBKHOAHOC)
            {
                DropDownList add = new DropDownList();
                add.ID = tl.MaKhoaHoc;
                add.Name = tl.TenKhoaHoc;
                KHOAHOC.Add(add);
            }
            ViewBag.ViewKHOAHOC = KHOAHOC;

            List<DropDownList> NXB = new List<DropDownList>();
            List<NXB> DBNXB= db.NXBs.ToList();
            foreach (NXB tl in DBNXB)
            {
                DropDownList add = new DropDownList();
                add.ID = tl.MaNXB;
                add.Name = tl.TenNXB;
                NXB.Add(add);
            }
            ViewBag.ViewNXB = NXB;

            List<DropDownList> TacGia = new List<DropDownList>();
            List<TACGIA> DBTacGia = db.TACGIAs.ToList();
            foreach (TACGIA tl in DBTacGia)
            {
                DropDownList add = new DropDownList();
                add.ID = tl.MaTG;
                add.Name = tl.TenTG;
                TacGia.Add(add);
            }
            ViewBag.ViewTacGia = TacGia;

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
            List<DropDownList> KHOAHOC = new List<DropDownList>();
            List<KHOAHOC> DBKHOAHOC = db.KHOAHOCs.ToList();
            foreach (KHOAHOC tl in DBKHOAHOC)
            {
                DropDownList add = new DropDownList();
                add.ID = tl.MaKhoaHoc;
                add.Name = tl.TenKhoaHoc;
                KHOAHOC.Add(add);
            }
            ViewBag.ViewKHOAHOC = KHOAHOC;

            List<DropDownList> NXB = new List<DropDownList>();
            List<NXB> DBNXB = db.NXBs.ToList();
            foreach (NXB tl in DBNXB)
            {
                DropDownList add = new DropDownList();
                add.ID = tl.MaNXB;
                add.Name = tl.TenNXB;
                NXB.Add(add);
            }
            ViewBag.ViewNXB = NXB;

            List<DropDownList> TacGia = new List<DropDownList>();
            List<TACGIA> DBTacGia = db.TACGIAs.ToList();
            foreach (TACGIA tl in DBTacGia)
            {
                DropDownList add = new DropDownList();
                add.ID = tl.MaTG;
                add.Name = tl.TenTG;
                TacGia.Add(add);
            }
            ViewBag.ViewTacGia = TacGia;
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

        public ActionResult Delete(int id)
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
            return RedirectToAction("Index");
        }
    }
}