using K21CNT2_BuiTienAnh_2110900003.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace K21CNT2_BuiTienAnh_2110900003.Areas.Customers.Controllers
{
    [Area("Customers")]
    public class MoMoController : Controller
    {
        private readonly DsmmvcContext _context;

        public MoMoController(DsmmvcContext context)
        {
            _context = context;
        }

        // Phương thức gửi yêu cầu thanh toán MoMo
        public async Task<IActionResult> CreatePayment(long orderId, decimal amount)
        {
            try
            {
                // Chuẩn bị yêu cầu thanh toán cho MoMo
                var momoRequest = new MoMoPaymentRequest
                {
                    OrderId = orderId.ToString(),
                    Amount = amount,
                    ReturnUrl = "https://localhost:7093/Checkout/PaymentCallBack", // URL callback của MoMo
                    NotifyUrl = "https://localhost:7093/Checkout/MomoNotify", // URL thông báo của MoMo
                    RequestType = "captureMoMoWallet",
                    PartnerCode = "MOMO", // Mã đối tác của bạn trên MoMo
                    AccessKey = "JNCJQ8K7", // Khóa truy cập của bạn trên MoMo
                    SecretKey = "K951B6PE1waDMi640xX08PD3vg6EkVlz", // Khóa bí mật của bạn trên MoMo
                    HashSecret = "F8BBA842ECF85" // Hash secret của MoMo
                };

                // Gọi API MoMo để khởi tạo thanh toán
                var momoResponse = await CallMoMoPaymentApi(momoRequest);

                if (momoResponse.Success)
                {
                    // Nếu thanh toán thành công, chuyển hướng người dùng đến trang thanh toán của MoMo
                    return Redirect(momoResponse.PaymentUrl);
                }
                else
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi tạo đơn thanh toán MoMo!";
                    return RedirectToAction("Index", "Carts");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã có lỗi xảy ra trong quá trình thanh toán.";
                return RedirectToAction("Index", "Carts");
            }
        }

        // Phương thức gọi API MoMo (POST request)
        private async Task<MoMoPaymentResponse> CallMoMoPaymentApi(MoMoPaymentRequest request)
        {
            using (var client = new HttpClient())
            {
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://test-payment.momo.vn/gw_payment/transactionProcessor", content);
                var responseString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<MoMoPaymentResponse>(responseString);
            }
        }

        // Phương thức nhận thông báo kết quả thanh toán từ MoMo (NotifyUrl)
        [HttpPost]
        public IActionResult MomoNotify()
        {
            var result = Request.Form["resultCode"].ToString();
            if (result == "0")  // Nếu thanh toán thành công
            {
                var orderId = Request.Form["orderId"].ToString();
                var transactionId = Request.Form["transId"].ToString();

                // Cập nhật trạng thái đơn hàng là đã thanh toán
                var order = _context.Orders.FirstOrDefault(o => o.Idorders == orderId);
                if (order != null)
                {
                    order.Status = 1; // Đã thanh toán
                    _context.SaveChanges();
                }

                TempData["SuccessMessage"] = "Thanh toán thành công!";
                return RedirectToAction("OrderDetails", "Carts");
            }

            TempData["ErrorMessage"] = "Thanh toán không thành công!";
            return RedirectToAction("OrderDetails", "Carts");
        }

        // Phương thức nhận phản hồi callback từ MoMo (ReturnUrl)
        public IActionResult PaymentCallBack()
        {
            var result = Request.Query["resultCode"].ToString();
            if (result == "0")  // Nếu thanh toán thành công
            {
                var orderId = Request.Query["orderId"].ToString();
                var transactionId = Request.Query["transId"].ToString();

                // Cập nhật trạng thái đơn hàng là đã thanh toán
                var order = _context.Orders.FirstOrDefault(o => o.Idorders == orderId);
                if (order != null)
                {
                    order.Status = 1; // Đã thanh toán
                    _context.SaveChanges();
                }

                TempData["SuccessMessage"] = "Thanh toán thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Thanh toán không thành công!";
            }

            return RedirectToAction("Index", "Carts");
        }
    }

    // Mô hình yêu cầu thanh toán MoMo
    public class MoMoPaymentRequest
    {
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public string ReturnUrl { get; set; }
        public string NotifyUrl { get; set; }
        public string RequestType { get; set; }
        public string PartnerCode { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string HashSecret { get; set; }
    }

    // Mô hình phản hồi từ MoMo
    public class MoMoPaymentResponse
    {
        public bool Success { get; set; }
        public string PaymentUrl { get; set; }
        public string Message { get; set; }
    }
}
