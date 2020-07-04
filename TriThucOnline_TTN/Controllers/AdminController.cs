using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TriThucOnline_TTN.Models;

namespace TriThucOnline_TTN.Controllers
{
    public class AdminController : Controller
    {
        private SQL_KhoaHoc db = new SQL_KhoaHoc();

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            if(Request.Cookies[".ASPXAUTH"] != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection form, string ReturnUrl)
        {

            string url = Server.UrlDecode(Request.UrlReferrer.PathAndQuery);
            string[] urldecode = url.Split('=');
            string returnurl = "";
            if (urldecode.Count() > 1)
            {
                returnurl = urldecode[1];
            }
            string username = form["username"].ToString();
            string password = form["password"].ToString();
            var usr = (from u in db.QUANTRIVIENs
                       where u.Username == username && u.Password == password
                       select u).FirstOrDefault();
            if (usr != null)
            {
                //create seession/ token for loged in user
                FormsAuthentication.SetAuthCookie(usr.Username, false);

                if (!string.IsNullOrEmpty(returnurl))
                {
                    return Redirect(returnurl);

                }
                return RedirectToAction("Index");


            }
            TempData["Message"] = "Tên tài khoản hoặc mật khẩu không đúng!";
            //
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public PartialViewResult TotalMember()
        {
            return PartialView();
        }
        public PartialViewResult TotalBook()
        {
            return PartialView();
        }
        public PartialViewResult TotalMoney()
        {
            return PartialView();
        }
        public PartialViewResult TotalDHChuaThanhToan()
        {
            return PartialView();
        }
    }
}
