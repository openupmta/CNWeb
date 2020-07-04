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
    public class DAUSACHController : Controller
    {
        private SQL_KhoaHoc db = new SQL_KhoaHoc();
        
        // GET: DAUSACH/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DAUSACH dAUSACH = db.DAUSACHes.Find(id);
            if (dAUSACH == null)
            {
                return HttpNotFound();
            }
            return View(dAUSACH);
        }
        public ActionResult SachLienQuan(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHOAHOC tl = db.KHOAHOCs.Find(id);
            if (tl == null)
            {
                return HttpNotFound();
            }
            ViewBag.sachtheoKHOAHOC = tl.DAUSACHes.ToList();
            return PartialView("_PartitalView_SachLienQuan");
        }
        [HttpPost]
        public JsonResult AddToCart(int? id, int quantity)
        {
            bool success = true;
            string error = "";
            List<CartItem> listCartItem;
            //Process Add To Cart
            if (Session["ShoppingCart"] == null)
            {
                //Create New Shopping Cart Session
                listCartItem = new List<CartItem>();
                listCartItem.Add(new CartItem { Quality = quantity, productOrder = db.DAUSACHes.Find(id) });
                Session["ShoppingCart"] = listCartItem;
            }
            else
            {
                bool flag = false;
                listCartItem = (List<CartItem>)Session["ShoppingCart"];
                foreach (CartItem item in listCartItem)
                {
                    if (item.productOrder.MaSach == id)
                    {
                        flag = true;
                        if (db.DAUSACHes.Find(id).SoLuongTon < quantity+item.Quality)
                        {
                            success = false;
                            error = "Rất tiếc, kho hàng của sản phẩm không đủ";
                            break;
                        }
                        item.Quality += quantity;
                        break;
                    }
                }
                if (!flag)
                {
                    if (db.DAUSACHes.Find(id).SoLuongTon < quantity)
                    {
                        success = false;
                        error = "Rất tiếc, kho hàng của sản phẩm không đủ";
                    }
                    else
                        listCartItem.Add(new CartItem { Quality = quantity, productOrder = db.DAUSACHes.Find(id) });
                }
                    
                Session["ShoppingCart"] = listCartItem;
            }

            //Count item in shopping cart
            int cartcount = 0;
            List<CartItem> ls = (List<CartItem>)Session["ShoppingCart"];
            foreach (CartItem item in ls)
            {
                cartcount += item.Quality;
            }
            return Json(new { Success = success, ItemAmount = cartcount, ErrorMsg = error });
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