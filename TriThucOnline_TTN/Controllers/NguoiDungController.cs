using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TriThucOnline_TTN.Models;

namespace TriThucOnline_TTN.Controllers
{
    public class NguoiDungController : Controller
    {
        private SQL_TriThucOnline_BanSachEntities db = new SQL_TriThucOnline_BanSachEntities();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            if(Session["TaiKhoan"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public ActionResult XemDonHang()
        {
            return View();
        }
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return Redirect("/");
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            string sTaiKhoan = f["username"].ToString();
            string sMatKhau = f.Get("password").ToString();
            string urlString = f.Get("urlString").ToString();
            var usr = (from u in db.KHACHHANGs
                       where u.Username == sTaiKhoan && u.Password == sMatKhau
                       select u).FirstOrDefault();
            //TempData["UserName"] = usr.TaiKhoan;
            if (usr != null)
            {
                //create seession/ token for loged in user
                // FormsAuthentication.SetAuthCookie(usr.TaiKhoan, false);
                Session["TaiKhoan"] = usr;
                //lay gio hang cua khach hang 
                if (urlString.Trim() != "")
                {
                    string[] url = urlString.Split('/');
                    if (url[url.Length - 1].Contains("Login") || url[url.Length - 1].Contains("dang-ky-he-thong") || url[url.Length - 1].Contains("register"))
                        return RedirectToAction("Index", "Home");
                    if (url[url.Length - 1].Contains("GioHang"))
                    {
                        KHACHHANG kh = Session["TaiKhoan"] as KHACHHANG;

                        return RedirectToAction("Confirm", "GioHang");
                    }
                    else
                        return Redirect(urlString);
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            usr = (from u in db.KHACHHANGs
                   where u.Username == sTaiKhoan
                   select u).FirstOrDefault();
            if (usr == null)
            {
                TempData["Message"] = "The username was not found or was deleted due to a violation";
                ViewBag.ThongBao = "Tài khoản không tồn tại hoặc đã bị xóa vì vi phạm!";
                return View();
            }

            TempData["Message"] = "Username or password is wrong";
            ViewBag.ThongBao = "Tên tài khoản hoặc mật khẩu không đúng!";

            return View();
            //KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            //if (kh != null)
            //{
            //    //<script type="text/javascript"> alert('Xss done');</script>

            //    ViewBag.ThongBao = "Chúc mừng bạn đăng nhập thành công !";
            //    Session["TaiKhoan"] = kh;
            //    return RedirectToAction("Index","Home");
            //}
            //ViewBag.ThongBao = "Tên tài khoản hoặc mật khẩu không đúng!";
            //return View();

        }

        [HttpGet]
        public ActionResult Register()
        {
            if (Session["TaiKhoan"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            KHACHHANG kh = new KHACHHANG();
            return View(kh);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Register(KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                KHACHHANG customer = db.KHACHHANGs.SingleOrDefault(user => user.Username == kh.Username);
                if (customer != null)
                {
                    ViewBag.ThongBao = "Lỗi: Tên tài khoản đã tồn tại";
                    return View(kh);
                }
                //Chèn dữ liệu vào bảng khách hàng
                db.KHACHHANGs.Add(kh);
                //Lưu vào csdl 
                db.SaveChanges();

                ViewBag.ThongBao = "Đăng ký thành công";
                return RedirectToAction("Login");
            }
            return View(kh);
        }
        public ActionResult Logout(string urlString)
        {
            //FormsAuthentication.SignOut();
            Session["TaiKhoan"] = null;
            if (urlString.Trim() != "")
                return Redirect(urlString);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Details()
        {
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("Login");
            }
            KHACHHANG kh = Session["TaiKhoan"] as KHACHHANG;
            kh = db.KHACHHANGs.Find(kh.MaKH);
            return View(kh);
        }

        [HttpPost]
        public ActionResult Details(KHACHHANG kh, HttpPostedFileBase fileUpload)
        {
            if (fileUpload != null)
            {
                //Lưu tên file
                var fileName = Path.GetFileName(fileUpload.FileName);
                //Lưu đường dẫn của file
                var path = Path.Combine(Server.MapPath("~/Content/HinhAnhKH"), fileName);
                if (!System.IO.File.Exists(path))
                {
                    fileUpload.SaveAs(path);
                }
                kh.PicUser = fileName;
            }
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(kh).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details");
            }

            return View(kh);
        }

        public PartialViewResult IndexPartial(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            KHACHHANG kh = Session["TaiKhoan"] as KHACHHANG;
            return PartialView(db.DONHANGs.Where(n => n.MaKH == kh.MaKH).OrderBy(n => n.MaDH).ToPagedList(pageNumber, pageSize));
        }

        public PartialViewResult XemCTDHPartial(int? madh)
        {
            var lstCTDH = db.CT_DONHANG.Where(n => n.MaDH == madh).ToList();
            return PartialView(lstCTDH);
        }
    }
}