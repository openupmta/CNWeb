using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TriThucOnline_TTN.Models;

namespace TriThucOnline_TTN.Controllers
{
    public class KHOAHOCController : Controller
    {
        private SQL_KhoaHoc db = new SQL_KhoaHoc();

        // GET: KHOAHOC
        public ActionResult Index()
        {
            return View(db.KHOAHOCs.ToList());
        }

        // GET: KHOAHOC/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHOAHOC KHOAHOC = db.KHOAHOCs.Find(id);
            if (KHOAHOC == null)
            {
                return HttpNotFound();
            }
            ViewBag.TenKhoaHoc = KHOAHOC.TenKhoaHoc;
            return View(db.DAUSACHes.Where(temp => temp.MaKhoaHoc == id).ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
