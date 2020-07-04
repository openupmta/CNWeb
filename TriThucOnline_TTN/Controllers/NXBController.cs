using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TriThucOnline_TTN.Models;

namespace TriThucOnline_TTN.Controllers
{
    public class NXBController : Controller
    {
        private SQL_KhoaHoc db = new SQL_KhoaHoc();


        // GET: NXB
        public ActionResult Index()
        {
            return View(db.NXBs.ToList());
        }

        // GET: NXB/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NXB nXB = db.NXBs.Find(id);
            if (nXB == null)
            {
                return HttpNotFound();
            }
            ViewBag.TenNXB = nXB.TenNXB;
            return View(db.DAUSACHes.Where(temp => temp.MaNXB == id).ToList());
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
