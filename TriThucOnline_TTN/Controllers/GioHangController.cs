using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TriThucOnline_TTN.Models;

namespace TriThucOnline_TTN.Controllers
{
    public class GioHangController : Controller
    {
        SQL_KhoaHoc db = new SQL_KhoaHoc();
        public List<CartItem> LayGioHang()
        {
            List<CartItem> lstGioHang = Session["ShoppingCart"] as List<CartItem>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<CartItem>();
                Session["ShoppingCart"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult GioHangTrong()
        {
            return View();
        }
        public ActionResult Index()
        {

            if (Session["ShoppingCart"] != null)
            {
                if (((List<CartItem>)Session["ShoppingCart"]).Count > 0)
                {
                    List<CartItem> lstGioHang = LayGioHang();
                    return View(lstGioHang);
                }
            }
            //return Giohang Trống
            return RedirectToAction("GioHangTrong");
        }
        
        [HttpGet]
        public ActionResult Confirm()
        {
            if (Session["ShoppingCart"] == null || ((List<CartItem>)Session["ShoppingCart"]).Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "NguoiDung");
            }

            KHACHHANG kh = Session["TaiKhoan"] as KHACHHANG;

            return View(kh);
        }
        //dem so luong san pham
        public ActionResult GioHangPartial()
        {
            int cartcount = 0;
            if (Session["ShoppingCart"] != null)
            {
                List<CartItem> ls = (List<CartItem>)Session["ShoppingCart"];
                foreach (CartItem item in ls)
                {
                    cartcount += item.Quality;
                }
            }
            ViewBag.count = cartcount;


            return PartialView();
        }
        [HttpPost]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "NguoiDung");
            }

            return Json(new { Url = Url.Action("DatHangPartial") });

            return View();
        }

        [HttpPost]
        public ActionResult DatHangPartial()
        {
            return PartialView("DatHangPartial");
        }

        [HttpPost]
        public ActionResult DatHangSubmit(string NgayGiao, string MaKM)
        {
            try
            {
                DONHANG dh = new DONHANG();
                KHUYENMAI km = db.KHUYENMAIs.Find(MaKM);
                if (Session["TaiKhoan"] != null || Session["TaiKhoan"].ToString() == "")
                {
                    KHACHHANG customer = (KHACHHANG)Session["TaiKhoan"];
                    if (ModelState.IsValid)
                    {
                        dh.MaKH = customer.MaKH;
                        dh.NgayMuaHang = DateTime.Now;
                        if (NgayGiao.Trim() != "")
                            dh.NgayGiao = Convert.ToDateTime(NgayGiao);

                        dh.TrangThaiGiao = 0;
                        dh.TrangThaiThanhToan = "Chưa thanh toán";
                        if (!string.IsNullOrEmpty(MaKM))
                        {
                            dh.MaKM = MaKM;
                            dh.KHUYENMAI = db.KHUYENMAIs.Find(MaKM);
                        }

                        db.DONHANGs.Add(dh);

                        db.SaveChanges();
                        if (!string.IsNullOrEmpty(MaKM))
                            foreach (var item in db.KHUYENMAIs)
                            {
                                if (item.MaKM.ToLower() == MaKM.ToLower())
                                {
                                    item.SoLanConLai--;
                                    break;
                                }
                            }
                        db.SaveChanges();
                    }
                }
                if (Session["ShoppingCart"] != null)
                {
                    List<CartItem> ls = (List<CartItem>)Session["ShoppingCart"];
                    foreach (CartItem item in ls)
                    {
                        CT_DONHANG CTDH = new CT_DONHANG();
                        CTDH.MaDH = dh.MaDH;
                        CTDH.MaSach = item.productOrder.MaSach;
                        CTDH.SoLuong = item.Quality;
                        if (!string.IsNullOrEmpty(MaKM))
                        {
                            CTDH.DonGia = item.productOrder.GiaTien - (item.productOrder.GiaTien * (km.GiaTriKM / 100));
                        }
                        else
                        {
                            CTDH.DonGia = item.productOrder.GiaTien;
                        }
                        db.CT_DONHANG.Add(CTDH);

                        DAUSACH sach = db.DAUSACHes.Find(item.productOrder.MaSach);
                        sach.SoLuongTon -= item.Quality;
                        db.SaveChanges();
                    }
                }
                Session["ShoppingCart"] = null;
            }
            catch (Exception)
            {
                return Json(new { success = false, msg = "Đặt hàng thất bại, vui lòng thử lại!" });
            }
            return Json(new { success = true, msg = "Đặt hàng thành công!" });
        }

        [HttpPost]
        public ActionResult CheckoutCart()
        {
            return Json(new { Url = Url.Action("Checkout") });
        }

        [HttpPost]
        public ActionResult Checkout()
        {
            List<CartItem> lstGioHang = LayGioHang();
            return PartialView(lstGioHang);
        }

        //cap nhat gio hang
        [HttpPost]
        public ActionResult CapNhat(int id, int sl)
        {
            bool success = true;
            string error = "";
            List<CartItem> listCartItem = (List<CartItem>)Session["ShoppingCart"];
            //nếu người dùng thêm hàng vào giỏ và lại trở về trang chủ thêm hàng tiếp 
            //thì session shoppingcart này có đang giữ tất cả sách trong giỏ hàng hiện tại hay không ?
            //có. cập nhật vào Session["ShoppingCart"] mà
            int cartcount = 0;
            foreach (var item in listCartItem)
            {
                if (item.productOrder.MaSach == id)
                {
                    if (db.DAUSACHes.Find(id).SoLuongTon < sl)
                    {
                        success = false;
                        error = "Rất tiếc, kho hàng của sản phẩm không đủ";
                    }
                    else
                        item.Quality = sl;
                    // break;

                }
                cartcount += item.Quality;
            }
            Session["ShoppingCart"] = listCartItem;

            return Json(new { Success = success, ErrorMsg = error, Url = Url.Action("Success"), sl = cartcount });
        }

        [HttpPost]
        public ActionResult Success()
        {
            List<CartItem> lstGioHang = LayGioHang();
            return PartialView(lstGioHang);
        }

        //xoa gio hang
        [HttpPost]
        public ActionResult Remove(int id)
        {
            int cartCount = 0;
            List<CartItem> listCartItem = (List<CartItem>)Session["ShoppingCart"];
            foreach (var item in listCartItem)
            {
                if (item.productOrder.MaSach == id)
                {
                    listCartItem.Remove(item);
                    break;
                }
            }
            foreach (var item in listCartItem)
            {
                cartCount += item.Quality;
            }
            Session["ShoppingCart"] = listCartItem;
            return Json(new { Url = Url.Action("Success"), sl = cartCount });
        }

        [HttpPost]
        public ActionResult KhuyenMai(string id)
        {
            KHUYENMAI km = db.KHUYENMAIs.Find(id);
            if (km == null || km.NgayBD > DateTime.Now || km.NgayKT < DateTime.Now || km.SoLanConLai <= 0)
            {
                return Json(new { tb = "Lỗi!", id = "", gt = "" });
            }
            return Json(new { tb = "Mã khuyến mãi có hiệu lực: ", id = id, gt = km.GiaTriKM + "%" });
        }
    }
}