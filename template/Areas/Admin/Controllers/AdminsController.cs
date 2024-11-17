using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using template.Models;

namespace template.Areas.Admin.Controllers
{
    public class AdminsController : Controller
    {
        QLBanSachEntity db = new QLBanSachEntity();
        private ADMIN _adm;
        // GET: Admin/Admin
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login lg)
        {

            _adm = (ADMIN)Session["userdadmin"];


            if (ModelState.IsValid)
            {
                ADMIN ad = db.ADMINs.SingleOrDefault(x => x.TenDNAdmin == lg.TenDN && x.MatKhauAdmin == lg.MatKhau);
                if (ad != null)
                {
                    Session.Add("useradmin", _adm);
                    return RedirectToAction("Index", "Admins");
                }
                else
                {
                    ViewBag.thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                    return View("Login");
                }
            }

            return View();
        }
    }
}