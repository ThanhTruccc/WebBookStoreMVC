using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using template.Models;
using template.Models.Payment;
namespace template.Controllers
{
    public class GioHangController : Controller
    {
        QLBanSachEntity db = new QLBanSachEntity();
        // GET: GioHang

        public List<GIOHANG> LayGioHang()
        {
            List<GIOHANG> lstGioHang = Session["GIOHANG"] as List<GIOHANG>;
            if (lstGioHang == null)
            {
                //Nếu giỏ hàng chưa tồn tại thì mình tiến hành khởi tao list giỏ hàng (sessionGioHang)
                lstGioHang = new List<GIOHANG>();
                Session["GIOHANG"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int iMasp, string strURL)
        {
            SACH sp = db.SACHes.SingleOrDefault(n => n.MaSach == iMasp);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy ra session giỏ hàng
            List<GIOHANG> lstGioHang = LayGioHang();
            //Kiểm tra sp này đã tồn tại trong session[giohang] chưa
            GIOHANG sanpham = lstGioHang.Find(n => n.iMasp == iMasp);
            if (sanpham == null)
            {
                sanpham = new GIOHANG(iMasp);
                //Add sản phẩm mới thêm vào list
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }
        }
        public ActionResult CapNhatGioHang(int iMaSP, FormCollection f)
        {
            //Kiểm tra masp
            SACH sp = db.SACHes.SingleOrDefault(n => n.MaSach == iMaSP);
            //Nếu get sai masp thì sẽ trả về trang lỗi 404
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng ra từ session
            List<GIOHANG> lstGioHang = LayGioHang();
            //Kiểm tra sp có tồn tại trong session["GioHang"]
            GIOHANG sanpham = lstGioHang.SingleOrDefault(n => n.iMasp == iMaSP);
            //Nếu mà tồn tại thì chúng ta cho sửa số lượng
            if (sanpham != null)
            {
                sanpham.iSoLuong = int.Parse(f["txtSoLuong"].ToString());

            }
            return RedirectToAction("GioHang");
        }
        //Xóa giỏ hàng
        public ActionResult XoaGioHang(int iMaSP)
        {
            //Kiểm tra masp
            SACH sp = db.SACHes.SingleOrDefault(n => n.MaSach == iMaSP);
            //Nếu get sai masp thì sẽ trả về trang lỗi 404
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng ra từ session
            List<GIOHANG> lstGioHang = LayGioHang();
            GIOHANG sanpham = lstGioHang.SingleOrDefault(n => n.iMasp == iMaSP);
            //Nếu mà tồn tại thì chúng ta cho sửa số lượng
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.iMasp == iMaSP);

            }
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("GioHang", "GioHang");
            }
            return RedirectToAction("GioHang");
        }
        // GET: GioHang
        public ActionResult GioHang()
        {

            if (Session["GIOHANG"] == null)
            {
                return RedirectToAction("Index", "BookStore");
            }
            List<GIOHANG> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GIOHANG> lstGioHang = Session["GIOHANG"] as List<GIOHANG>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }
        //Tính tổng thành tiền
        private double TongTien()
        {
            double dTongTien = 0;
            List<GIOHANG> lstGioHang = Session["GIOHANG"] as List<GIOHANG>;
            if (lstGioHang != null)
            {
                dTongTien = (double)lstGioHang.Sum(n => n.ThanhTien);
            }
            return dTongTien;
        }
        //tạo partial giỏ hàng

        public ActionResult GioHangPartial()
        {
            if (TongSoLuong() == 0)
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        //Xây dựng 1 view cho người dùng chỉnh sửa giỏ hàng

        public ActionResult SuaGioHang()
        {
            if (Session["GIOHANG"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GIOHANG> lstGioHang = LayGioHang();
            return View(lstGioHang);

        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["user"] == null || Session["user"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Nguoidung");
            }
            if (Session["GIOHANG"] == null)
            {
                return RedirectToAction("Index", "BookStore");
            }

            List<GIOHANG> lstGioHang = LayGioHang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGioHang);
        }



        public ActionResult DatHang(FormCollection collection)
        {
            // Check if the user is logged in
            if (Session["user"] == null || string.IsNullOrEmpty(Session["user"].ToString()))
            {
                return RedirectToAction("DangNhap", "Nguoidung");
            }

            // Retrieve the user from the session
            KHACHHANG kh = (KHACHHANG)Session["user"];
            if (kh == null)
            {
                return RedirectToAction("DangNhap", "Nguoidung");
            }

            // Create a new order
            DONDATHANG ddh = new DONDATHANG
            {
                MaKH = kh.MaKH,
                NgayDH = DateTime.Now,
                HTGiaoHang = false,
                HTThanhToan = false
            };

            // Get the shopping cart items
            List<GIOHANG> lstGioHang = LayGioHang();

            // Validate and parse delivery date
            if (!DateTime.TryParseExact(collection["Ngaygiao"], "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ngayGiaoHang))
            {
                ModelState.AddModelError("Ngaygiao", "Invalid date format. Please enter the date in MM/dd/yyyy format.");
                return View(lstGioHang);
            }

            ddh.NgayGiaoHang = ngayGiaoHang;

            // Add the order to the database
            db.DONDATHANGs.Add(ddh);
            db.SaveChanges();

            // Add order details
            foreach (var item in lstGioHang)
            {
                var ctdh = new CTDATHANG
                {
                    SoDH = ddh.SoDH,
                    MaSach = item.iMasp,
                    SoLuong = item.iSoLuong,
                    DonGia = (decimal)item.dDonGia
                };

                db.CTDATHANGs.Add(ctdh);
            }

            db.SaveChanges();
            Session["GIOHANG"] = null;
            return RedirectToAction("Xacnhandonhang", "GioHang");
        }

        public ActionResult Xacnhandonhang()
        {
            return View();
        }
        public ActionResult VnpayReturn()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        var itemOrder = db.DONDATHANGs.FirstOrDefault(x => x.TenNguoiNhan == orderCode);
                        if (itemOrder != null)
                        {
                            itemOrder.Status = 2;//đã thanh toán
                            db.DONDATHANGs.Attach(itemOrder);
                            db.Entry(itemOrder).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        //Thanh toan thanh cong
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    ViewBag.ThanhToanThanhCong = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                    //displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;
                }
            }
            //var a = UrlPayment(0, "DH3574");
            return View();
        }
    }
}