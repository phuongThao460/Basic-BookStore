using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
        public class Cart
        {
        DBBookStoreEntity db = new DBBookStoreEntity();
        public int idMasach { get; set; }
        public string sTensach { get; set; }
        public string sAnhbia { get; set; }
        public Double dDongia { get; set; }
        public int iSoluong { get; set; }
        public Double dThanhtien { get { return iSoluong * dDongia; } }

        public Cart(int masach)
        {
            idMasach = masach;
            SACH sach = db.SACHes.Single(n => n.Masach == idMasach);
            sTensach = sach.Tensach;
            sAnhbia = sach.Anhbia;
            dDongia = double.Parse(sach.Giaban.ToString());
            iSoluong = 1;
        }

    }
}