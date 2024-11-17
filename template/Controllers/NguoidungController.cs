using System;
using System.Linq;
using System.Web.Mvc;
using template.Models;

namespace template.Controllers
{
    public class NguoiDungController : Controller
    {
        private KHACHHANG _kh;
        private QLBanSachEntity db = new QLBanSachEntity();

        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dangky()
        {
            _kh = (KHACHHANG)Session["user"];
            if (_kh != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult DangNhap()
        {
            _kh = (KHACHHANG)Session["user"];
            if (_kh != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult DangKy(KHACHHANG khh)
        {
            _kh = (KHACHHANG)Session["user"];
            if (_kh != null)
            {
                return RedirectToAction("Index","BookStore");
            }
            if (!ModelState.IsValid)
            {
                return View(khh);
            }
            _kh = new KHACHHANG();

            _kh.HoTenKH = khh.HoTenKH;
            _kh.NgaySinh = khh.NgaySinh;
            _kh.DiaChiKH = khh.DiaChiKH;
            _kh.DienThoaiKH = khh.DienThoaiKH;
            _kh.Email = khh.Email;
            
            _kh.TenDN = khh.TenDN;
            _kh.MatKhau = khh.MatKhau;
            db.KHACHHANGs.Add(_kh);
            db.SaveChanges();
            Session.Add("user", _kh);
            return RedirectToAction("Index", "BookStore");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(Login lg)
        {
            _kh = (KHACHHANG)Session["user"];
            if (_kh != null)
            {
                return RedirectToAction("DangNhap", "Nguoidung");
            }
            if (ModelState.IsValid)
            {
                _kh = db.KHACHHANGs.SingleOrDefault(n => n.TenDN == lg.TenDN && n.MatKhau == lg.MatKhau);
                if (_kh != null)
                {
                    Session.Add("user", _kh);
                    return RedirectToAction("Index", "BookStore");
                }
                ViewBag.Fail = "Đăng nhập thất bại";
                return View("DangNhap");
            }
            return View(lg);
        }
        //public JsonResult UserExists(string username)
        //{
        //    return Json(!db.KHACHHANGs.Any(n => n.TenDN == username), JsonRequestBehavior.AllowGet);
        //}
        public ActionResult DangXuat()
        {
            Session["user"] = null;
            return RedirectToAction("Index", "BookStore");

        }
    }
}
