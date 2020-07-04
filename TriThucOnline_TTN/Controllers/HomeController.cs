using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TriThucOnline_TTN.Models;


namespace TriThucOnline_TTN.Controllers
{
    public class HomeController : Controller
    {
        private SQL_TriThucOnline_BanSachEntities db = new SQL_TriThucOnline_BanSachEntities();
       
        public ActionResult Index(int PageNo = 1)
        {

            // phân loại sách theo thể loại (Home)
            ViewBag.vanhoc = db.DAUSACHes.Where(temp => temp.MaTL == 1).ToList();
            ViewBag.thieunhi = db.DAUSACHes.Where(temp => temp.MaTL == 2).ToList();
            ViewBag.hoiki = db.DAUSACHes.Where(temp => temp.MaTL == 3).ToList();
            ViewBag.giaokhoa = db.DAUSACHes.Where(temp => temp.MaTL == 4).ToList();
            ViewBag.ngoaingu = db.DAUSACHes.Where(temp => temp.MaTL == 5).ToList();
            ViewBag.kinhte = db.DAUSACHes.Where(temp => temp.MaTL == 6).ToList();
            ViewBag.tamli = db.DAUSACHes.Where(temp => temp.MaTL == 7).ToList();
            ViewBag.nuoidaycon = db.DAUSACHes.Where(temp => temp.MaTL ==8).ToList();
            ViewBag.vanphongpham = db.DAUSACHes.Where(temp => temp.MaTL == 9).ToList();


            //Paging
            List<DAUSACH> dausach = db.DAUSACHes.ToList();
            int NoOfRecordPerPage = 6;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(dausach.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (PageNo - 1) * NoOfRecordPerPage;
            ViewBag.PageNo = PageNo;
            ViewBag.NoOfPage = NoOfPages;
            dausach = dausach.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();

            /// giới thiệu sách mới
            ViewBag.SachMoi = dausach;
            /// sách bán chạy 
            ViewBag.SachBanChay = dausach;

            return View();
        }
        public ActionResult Paging()
        {
            return PartialView("_Paging");
        }
        public ActionResult vanhoc()
        { 
            return PartialView("_vanhoc");
        }
        public ActionResult thieunhi()
        {
            return PartialView("_thieunhi");
        }
        public ActionResult hoiki()
        {
            return PartialView("_hoiki");
        }
        public ActionResult giaokhoa()
        {
            return PartialView("_giaokhoa");
        }
        public ActionResult ngoaingu()
        {
            return PartialView("_ngoaingu");
        }
        public ActionResult kinhte()
        {
            return PartialView("_kinhte");
        }

        public ActionResult tamli()
        {
            return PartialView("_tamli");
        }
        public ActionResult nuoidaycon()
        {
            return PartialView("_nuoidaycon");
        }
        public ActionResult vanphongpham()
        {
            return PartialView("_vanphongpham");
        }
        public ActionResult SachMoi()
        {
            return PartialView("_PartitalView_SachMoi");
        }
        public ActionResult SachBanChay()
        {
            return PartialView("_PartitalView_SachBanChay");
        }

        public ActionResult Search( string search="" )
        {
            ViewBag.search = search;
            return View(db.DAUSACHes.Where(temp => temp.TenSach.Contains(search)).ToList());

            //Sorting book

        //    ViewBag.SortColum = SortColum;
        //    ViewBag.IconClass = IconClass;
        //    if(ViewBag.SortColum == "TenSach")
        //    {
        //        if (ViewBag.IconClass == "fa-sort-amount-up")
        //        {
        //            db.DAUSACHes.OrderBy(temp => temp.MaSach).ToList();
        //        }
        //        else db.DAUSACHes.OrderByDescending(temp => temp.MaSach).ToList();
        //    }
        //    else if(ViewBag.SortColum == "GiaTien")
        //    {
        //        if (ViewBag.IconClass == "fa-sort-amount-up")
        //        {
        //            db.DAUSACHes.OrderBy(temp => temp.MaSach).ToList();
        //        }
        //        else db.DAUSACHes.OrderByDescending(temp => temp.MaSach).ToList();
        //    }
        //    else if (ViewBag.SortColum == "Moi")
        //    {
        //        if (ViewBag.IconClass == "fa-sort-amount-up")
        //        {
        //            db.DAUSACHes.OrderBy(temp => temp.MaSach).ToList();
        //        }
        //        else db.DAUSACHes.OrderByDescending(temp => temp.MaSach).ToList();
        //    }
        }
    }
}