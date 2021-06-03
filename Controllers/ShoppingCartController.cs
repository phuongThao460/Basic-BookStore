using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        DBBookStoreEntity database = new DBBookStoreEntity();
        public List<Cart> Laygiohang()
        {
            List<Cart> lstCart = Session["Giohang"] as List<Cart>;
            if(lstCart == null)
            {
                lstCart = new List<Cart>();
                Session["Giohang"] = lstCart;
            }
            return lstCart;
        }

        public ActionResult Themgiohang(int iMasach, string strUrl)
        {
            List<Cart> lstCart = Laygiohang();
            Cart sp = lstCart.Find(n => n.idMasach == iMasach);
            if(sp == null)
            {
                sp = new Cart(iMasach);
                lstCart.Add(sp);
                return Redirect(strUrl);
            }
            else
            {
                sp.iSoluong++;
                return Redirect(strUrl);
            }
        }
        private int Tongsoluong()
        {
            int iTongsoluong = 0;
            List<Cart> lstCart = Session["Giohang"] as List<Cart>;
            if(lstCart != null)
            {
                iTongsoluong = lstCart.Sum(n => n.iSoluong);
            }
            return iTongsoluong;
        }

        private double Tongtien()
        {
            double iTongtien = 0;
            List<Cart> lstCart = Session["Giohang"] as List<Cart>;
            if (lstCart != null)
            {
                iTongtien = lstCart.Sum(n => n.dThanhtien);
            }
            return iTongtien;
        }
        public ActionResult CapNhatGioHang(int iMasach, FormCollection f)
        {
            List<Cart> lstCart = Laygiohang();
            Cart cart = lstCart.SingleOrDefault(n => n.idMasach == iMasach);
            if (cart != null)
            {
                cart.iSoluong = int.Parse(f["txtSoLuong"].ToString());
            }
            return View("GioHang");
        }

        public ActionResult XoaGioHang(int iMaSP)
        {
            List<Cart> lstCart = Laygiohang();
            Cart cart = lstCart.SingleOrDefault(n => n.idMasach == iMaSP);
            if (cart == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Cart> lstGiohang = Laygiohang();
            Cart sanpham = lstGiohang.SingleOrDefault(n => n.idMasach == iMaSP);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.idMasach == iMaSP);
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult RemoveAll()
        {
            List<Cart> lstCart = Laygiohang();
            lstCart.Clear();
            return RedirectToAction("Index", "BookStore");
        }
        public ActionResult GioHang()
        {
            List<Cart> lstCart = Laygiohang();
            if(lstCart.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            ViewBag.TongSoLuong = Tongsoluong();
            ViewBag.TongTien = Tongtien();
            return View(lstCart);
        }
        public ActionResult DatHang()
        {
            if (Session["Taikhoan"] == null)
                return RedirectToAction("DangNhap", "KhachHang");
            else if (Session["Giohang"] == null)
                return RedirectToAction("Index", "BookStore");
            List<Cart> lstCart = Laygiohang();
            ViewBag.TongSoLuong = Tongsoluong();
            ViewBag.TongTien = Tongtien();
            return View(lstCart);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Cart> cart = Laygiohang();
            var ngayGiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            if (string.IsNullOrEmpty(ngayGiao))
            {
                ViewData["Loi1"] = "ngay giao khong duoc rong";
            }
            else
            {
                ddh.MaKH = kh.MaKH;
                ddh.Ngaydat = DateTime.Now;
                ddh.Ngaygiao = DateTime.Parse(ngayGiao);
                ddh.Tinhtranggiaohang = "not";
                ddh.Dathanhtoan = false;
                database.DONDATHANGs.Add(ddh);
                database.SaveChanges();
                foreach (var item in cart)
                {
                    CHITIETDONHANG ct = new CHITIETDONHANG();
                    ct.MaDonHang = ddh.MaDonHang;
                    ct.Masach = item.idMasach;
                    ct.Soluong = item.iSoluong;
                    ct.Dongia = item.dDongia;
                    database.CHITIETDONHANGs.Add(ct);
                    database.SaveChanges();
                }
                Session["GioHang"] = null;
                return RedirectToAction("XacNhanDonHang", "GioHang");
            }
            return View(cart);
        }
        public ActionResult XacNhanMuaHang()
        {
            return View();
        }
        public ActionResult GioHangPartial()
        {
            if (Tongsoluong() == 0)
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = Tongsoluong();
            ViewBag.TongTien = Tongtien();
            return PartialView();
        }
        public ActionResult CheckOut_Success()
        {
            return View();
        }
    }
}