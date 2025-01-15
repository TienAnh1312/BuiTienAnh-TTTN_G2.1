using K21CNT2_BuiTienAnh_2110900003.Models;
using K21CNT2_BuiTienAnh_2110900003.Areas.Customers.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace K21CNT2_BuiTienAnh_2110900003.Areas.Customers.Controllers
{
    [Area("Customers")]
    public class ThanhToanController : Controller
    {
        private readonly DsmmvcContext _context;
        private readonly string _vnp_TmnCode = "X49PI1WG"; // Mã TmnCode VNPAY
        private readonly string _vnp_HashSecret = "LKSOAAPCIUWUKVZKDMXZVYPUTOQGXXPH"; // Hash secret VNPAY
        private readonly string _vnp_ReturnUrl = "https://localhost:7093/ThanhToan/Return"; // URL trả về sau khi thanh toán

        public ThanhToanController(DsmmvcContext context)
        {
            _context = context;
        }

        // Trang thanh toán
        public IActionResult Index()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            var customerIdSession = HttpContext.Session.GetInt32("CustomersID");
            if (!customerIdSession.HasValue)
            {
                return Redirect("/customers/Login/index/?url=/ThanhToan/Index");
            }

            var dataMember = _context.Customers.FirstOrDefault(x => x.Id == customerIdSession.Value);
            if (dataMember == null)
            {
                return Redirect("/customers/Login/index/?url=/ThanhToan/Index");
            }

            ViewBag.Customer = dataMember;

            // Lấy giỏ hàng từ session
            var cartInSession = HttpContext.Session.GetString("My-Cart");
            var carts = cartInSession != null ? JsonConvert.DeserializeObject<List<Cart>>(cartInSession) : new List<Cart>();

            // Nếu giỏ hàng trống
            if (carts.Count == 0)
            {
                TempData["EmptyCartMessage"] = "Giỏ hàng của bạn đang trống. Vui lòng thêm sản phẩm!";
                return RedirectToAction("Index", "Carts");
            }

            double total = carts.Sum(item => item.Quantity * item.Price);
            ViewBag.total = total;

            // Danh sách phương thức thanh toán
            var dataPay = _context.PaymentMethods.ToList();
            ViewData["IdPayment"] = new SelectList(dataPay, "Id", "Name", 1);

            return View(carts);
        }

        // Tạo yêu cầu thanh toán VNPAY
        public IActionResult CreateVnPayRequest()
        {
            // Lấy giỏ hàng từ session
            var cartInSession = HttpContext.Session.GetString("My-Cart");
            var carts = cartInSession != null ? JsonConvert.DeserializeObject<List<Cart>>(cartInSession) : new List<Cart>();

            if (carts.Count == 0)
            {
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống. Vui lòng thêm sản phẩm trước khi thanh toán!";
                return RedirectToAction("Index", "Carts");
            }

            // Tính tổng số tiền cần thanh toán
            var totalAmount = carts.Sum(item => item.Quantity * item.Price);

            // Tạo thông tin đơn hàng (OrderId)
            var orderId = "DH." + DateTime.Now.ToString("yyyy-MM-dd.HH-mm-ss.fff");

            // Lưu đơn hàng vào cơ sở dữ liệu
            var order = new Order
            {
                Idorders = orderId,
                OrdersDate = DateTime.Now,
                TotalMoney = (decimal?)totalAmount,
                Status = 0 // Trạng thái chờ thanh toán
            };

            var customerIdSession = HttpContext.Session.GetInt32("CustomersID");
            if (customerIdSession.HasValue)
            {
                order.Idcustomer = customerIdSession.Value;
            }
            else
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để thực hiện thanh toán!";
                return RedirectToAction("Index", "Login");
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            // Tạo các tham số yêu cầu thanh toán VNPAY
            var vnpayLibrary = new VnPayLibrary();
            vnpayLibrary.AddRequestData("vnp_TmnCode", _vnp_TmnCode);
            vnpayLibrary.AddRequestData("vnp_Amount", (totalAmount * 100).ToString());
            vnpayLibrary.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng " + orderId);
            vnpayLibrary.AddRequestData("vnp_OrderId", orderId);
            vnpayLibrary.AddRequestData("vnp_ReturnUrl", _vnp_ReturnUrl);
            vnpayLibrary.AddRequestData("vnp_CurrCode", "VND");
            vnpayLibrary.AddRequestData("vnp_TxnRef", orderId);

            // Tạo URL thanh toán và chuyển hướng đến trang thanh toán VNPAY
            string vnpayUrl = vnpayLibrary.CreateRequestUrl("https://sandbox.vnpayment.vn/paymentv2/vpcpay.html", _vnp_HashSecret);
            return Redirect(vnpayUrl);
        }

        // Nhận kết quả thanh toán từ VNPAY
        public IActionResult Return()
        {
            try
            {
                var vnp_SecureHash = Request.Query["vnp_SecureHash"];
                var vnp_ResponseCode = Request.Query["vnp_ResponseCode"];
                var vnp_TxnRef = Request.Query["vnp_TxnRef"];

                // Kiểm tra chữ ký trả về
                var vnpayLibrary = new VnPayLibrary();
                bool isValidSignature = vnpayLibrary.ValidateSignature(vnp_SecureHash, _vnp_HashSecret);

                if (isValidSignature)
                {
                    if (vnp_ResponseCode == "00") // Nếu giao dịch thành công
                    {
                        // Cập nhật trạng thái đơn hàng
                        var order = _context.Orders.FirstOrDefault(o => o.Idorders == vnp_TxnRef);
                        if (order != null)
                        {
                            order.Status = 1; // Đơn hàng đã thanh toán thành công
                            _context.SaveChanges();

                            // Xóa giỏ hàng khỏi session
                            HttpContext.Session.Remove("My-Cart");
                            HttpContext.Session.Remove("CartCount");

                            TempData["SuccessMessage"] = "Thanh toán thành công!";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Thanh toán không thành công!";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Dữ liệu bị thay đổi! Chữ ký không hợp lệ.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã có lỗi xảy ra trong quá trình thanh toán.";
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
