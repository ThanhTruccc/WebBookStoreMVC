using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using template.Models;

using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using System.Reflection;
namespace template.Controllers
{
    public class BookStoreController : Controller
    {
        
        private QLBanSachEntity data= new QLBanSachEntity();
        // GET: BookStore
        private List<SACH> Laysach(int count)
        {
            return data.SACHes.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }

        public ActionResult Index()
        {


            var sachmoi = Laysach(5);

            if (sachmoi != null)
            {
                return View(sachmoi);
            }
            else
            {
                return View("Error");

            }
        }
        public ActionResult Login()
        {
            return View();

        }
        public ActionResult Detail( int id)
        {

            var sach = from s in data.SACHes
                       where s.MaSach == id
                       select s;
            return View(sach.Single());
        }
        public ActionResult Chude()
        {
            var chude= from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        public ActionResult Nhaxuatban()
        {
            var nhaxb = from cd in data.NHAXUATBANs select cd;
            return PartialView(nhaxb);
        }
        public ActionResult SPtheochude(int id)
        {
            var sach = from s in data.SACHes where s.MaCD==id select s;
            return PartialView(sach);
        }
        public ActionResult SPtheoNXB(int id)
        {
            var sach = from s in data.SACHes where s.MaNXB == id select s;
            return PartialView(sach);
        }
    }
}