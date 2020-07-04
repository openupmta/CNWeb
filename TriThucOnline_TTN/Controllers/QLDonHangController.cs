using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TriThucOnline_TTN.Models;
using PagedList;
using PagedList.Mvc;
using System.Net;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TriThucOnline_TTN.Controllers
{
    [Authorize]
    public class QLDonHangController : Controller
    {
        // GET: QLDonHang
        SQL_TriThucOnline_BanSachEntities db = new SQL_TriThucOnline_BanSachEntities();
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(db.DONHANGs.ToList().OrderBy(n => n.MaDH).ToPagedList(pageNumber, pageSize));
        }
        public PartialViewResult IndexPartial(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return PartialView(db.DONHANGs.ToList().OrderBy(n => n.MaDH).ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public JsonResult Remove(int id)
        {
            DONHANG dh = db.DONHANGs.SingleOrDefault(n => n.MaDH == id);
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.DONHANGs.Remove(dh);
            db.SaveChanges();
            return Json(new { Url = Url.Action("IndexPartial") });
        }

        public ActionResult Edit(int? madh)
        {
            ////Lấy ra đối tượng sách theo mã 
            //DONHANG cd = db.DONHANGs.SingleOrDefault(n => n.MaDH == madh);
            //if (cd == null)
            //{
            //    Response.StatusCode = 404;
            //    return null;
            //}
            //return View();
            if (madh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONHANG book = db.DONHANGs.SingleOrDefault(s => s.MaDH == madh);
            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(int madh, FormCollection f)
        {
            ////Thêm vào cơ sở dữ liệu
            //if (ModelState.IsValid)
            //{
            //    //Thực hiện cập nhận trong model
            //    db.Entry(dh).State = System.Data.Entity.EntityState.Modified;
            //    db.SaveChanges();
            //}

            //return RedirectToAction("Index");
            var bookUpdate = db.DONHANGs.Find(madh);
            if (TryUpdateModel(bookUpdate, "", new string[] { /*"NgayGiao", "NgayDat",*/ "TrangThaiThanhToan", "TrangThaiGiao"/*, "MaKH"*/ }))
            {
                try
                {
                    db.Entry(bookUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Error Save Data");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult XemCTDH(int madh)
        {
            TempData["madh"] = madh;
            return Json(new { Url = Url.Action("XemCTDHPartial") });
        }

        public PartialViewResult XemCTDHPartial()
        {
            int madh=(int)TempData["madh"];
            var lstCTDH = db.CT_DONHANG.Where(n => n.MaDH == madh).ToList();
            return PartialView(lstCTDH);
        }

        public ActionResult ExportClientsListToExcel()
        {
            var products = (from s in db.DONHANGs
                            select s).ToList();

            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DONHANG.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View("MyView");

        }
    }
}