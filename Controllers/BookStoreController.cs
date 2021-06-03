using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class BookStoreController : Controller
    {
        // GET: BookStore
        DBBookStoreEntity db = new DBBookStoreEntity();

        private List<SACH> Laysachmoi(int soluong)
        {
            return db.SACHes.OrderByDescending(n => n.Ngaycapnhat).Take(soluong).ToList();
        }
        public ActionResult Index()
        {
            return View(Laysachmoi(4));
        }
        public ActionResult ChuDe()
        {
            var cd = db.CHUDEs.OrderBy(n => n.TenChuDe).ToList();
            return PartialView(cd);
        }
        public ActionResult NhaXuatBan()
        {
            var nxb = db.NHAXUATBANs.OrderBy(n => n.TenNXB).ToList();
            return PartialView(nxb);
        }
        public ActionResult Details (int id)
        {
            var sach = (from s in db.SACHes
                        join nxb in db.NHAXUATBANs on s.MaNXB equals nxb.MaNXB
                        where s.Masach == id
                        select new ViewSanPham()
                        {
                            Masach = s.Masach,
                            Tensach = s.Tensach,
                            Mota = s.Mota,
                            Anhbia=s.Anhbia,
                            Giaban=s.Giaban,
                            TenNXB=nxb.TenNXB
                        }).SingleOrDefault();
            return View(sach);
        }

        public ActionResult SachTheoChuDe(int id)
        {
            var s = db.SACHes.Where(n => n.MaCD == id).ToList();
            return View(s);
        }
        public ActionResult SachTheoNXB(int id)
        {
            var s = db.SACHes.Where(n => n.MaNXB == id).ToList();
            return View(s);
        }
    }
}