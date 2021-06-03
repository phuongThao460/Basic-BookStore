using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        DBBookStoreEntity db = new DBBookStoreEntity();
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["Hoten"];
            var tendn = collection["Taikhoan"];
            var matkhau = collection["Matkhau"];
            var mathaunhaplai = collection["Matkhaunhaplai"];
            var diachi = collection["DiachiKH"];
            var dienthoai = collection["DienthoaiKH"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}",collection["Ngaysinh"]);
            var email = collection["Email"];

            if (string.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Name not empty";
            }
            else if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Username not empty";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Password not empty";
            }
            else if (string.Compare(matkhau,mathaunhaplai) != 0)
            {
                ViewData["Loi4"] = "Confirm password not correct";
            }
            else if (string.IsNullOrEmpty(diachi))
            {
                ViewData["Loi5"] = "Address not empty";
            }
            else if (string.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Phone number not empty";
            }
            else if (string.IsNullOrEmpty(ngaysinh))
            {
                ViewData["Loi7"] = "Date of birth not empty";
            }
            else if (string.IsNullOrEmpty(email))
            {
                ViewData["Loi8"] = "Email not empty";
            }
            else
            {
                db.KHACHHANGs.Add(kh);
                db.SaveChanges();
            }
            return Redirect("/");
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendn = collection["Taikhoan"];
            var matkhau = collection["Matkhau"];
            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Username not empty";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Password not empty";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau == matkhau);
                if(kh != null)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    Session["taikhoan"] = kh;
                    return RedirectToAction("Index", "BookStore");
                }
                else
                {
                    ViewBag.ThongBao = "Username or password is incorrect";
                }
            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session["taikhoan"] = null;
            return Redirect("/");
        }
    }
} 