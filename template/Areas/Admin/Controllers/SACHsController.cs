using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using template.Models;
using System.Data.Entity;
namespace template.Areas.Admin.Controllers
{
    public class SACHsController : Controller
    {
        QLBanSachEntity db = new QLBanSachEntity();
        private ADMIN _adm;
        // GET: Admin/SACH
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SACH(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;

            return View(db.SACHes.ToList().OrderBy(n => n.MaSach).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Themmoisach()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoisach(SACH sach, HttpPostedFileBase fileupload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui long chon anh";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var file = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/Images/Sach"), file);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hinh anh da ton tai";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sach.HinhMinhHoa = file;

                    db.SACHes.Add(sach);
                    db.SaveChanges();
                }
                return RedirectToAction("SACH");
            }
        }
        //Xem chi tiet
        public ActionResult Chitiet(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            ViewBag.Masach = sach?.MaSach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        //Xoa san pham
        [HttpGet]
        public ActionResult Xoasach(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            ViewBag.Masach = sach?.MaSach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpPost, ActionName("Xoasach")]
        public ActionResult Xacnhanxoa(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            ViewBag.Masach = sach?.MaSach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SACHes.Remove(sach);
            db.SaveChanges();
            return RedirectToAction("SACH");
        }

        //chinh sua san pham 
        [HttpGet]
        public ActionResult Suasach(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                //Response.StatusCode = 404;
                //return null;
                return HttpNotFound();
            }
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            return View(sach);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult Suasach(SACH sach, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.OrderBy(n => n.TenChuDe).ToList(), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.OrderBy(n => n.TenNXB).ToList(), "MaNXB", "TenNXB", sach.MaNXB);

            if (fileUpload != null && fileUpload.ContentLength > 0) // Check for file presence and content
            {

                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" }; // Allowed image extensions
                if (!allowedExtensions.Contains(Path.GetExtension(fileUpload.FileName).ToLower()))
                {
                    ViewBag.Thongbao = "Định dạng file không hợp lệ. Vui lòng chọn file image (jpg, jpeg, png, gif).";
                    return View(sach);
                }

                // Generate unique filename
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileUpload.FileName);
                string path = Path.Combine(Server.MapPath("~/Images/Images/Sach"), fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    fileUpload.InputStream.CopyTo(stream); // Use InputStream for better performance
                }

                sach.HinhMinhHoa = fileName; // Store only the filename
            }

            if (ModelState.IsValid)
            {
                db.Entry(sach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SACH");
            }

            return View(sach);
        }

    }
}