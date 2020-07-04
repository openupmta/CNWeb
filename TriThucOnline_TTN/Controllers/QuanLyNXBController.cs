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
        SQL_KhoaHoc db = new SQL_KhoaHoc();
        public ActionResult Index(int PageNum = 1, int PageSize = 5, string search = null)
        {
            ViewBag.PageSize = PageSize;
            var lst = db.Database.SqlQuery<NXB>("select tl.* from NXB as tl").ToList();

            if (search != null)
            {
                lst = lst.Where(x => x.TenNXB.ToLower().Trim().Contains(search.ToLower().Trim())).OrderByDescending(x => x.TenNXB).ToList();
            }
            return View(lst.ToList().ToPagedList<NXB>(PageNum, PageSize));
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
        public ActionResult Edit(int id)
        {
            //Lấy ra đối tượng sách theo mã 
            NXB nxb = db.NXBs.SingleOrDefault(n => n.MaNXB == id);
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

        public ActionResult Delete(int id)
        {
            NXB nxb = db.NXBs.SingleOrDefault(n => n.MaNXB == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.NXBs.Remove(nxb);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}