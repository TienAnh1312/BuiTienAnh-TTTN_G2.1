using K21CNT2_BuiTienAnh_2110900003.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using System;
using K21CNT2_BuiTienAnh_2110900003.Areas.Customers.Models;

namespace K21CNT2_BuiTienAnh_2110900003.Areas.Customers.Controllers
{
    public class CartsController : BaseController
    {
        private readonly DsmmvcContext _context;
        

        public CartsController(DsmmvcContext context)
        {
            _context = context;
            
        }

        // Hiển thị giỏ hàng
        public ActionResult Index()
        {
            var cartInSession = HttpContext.Session.GetString("My-Cart");
            var carts = cartInSession != null ? JsonConvert.DeserializeObject<List<Cart>>(cartInSession) : new List<Cart>();

            double total = carts.Sum(item => item.Quantity * item.Price);
            int cartCount = carts.Sum(c => c.Quantity);

            ViewBag.Total = total;
            ViewBag.CartCount = cartCount;
            return View(carts);
        }

        // Thêm sản phẩm vào giỏ
        public IActionResult Add(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var p = _context.Products.Find(id);
            if (p == null)
            {
                return RedirectToAction("Index");
            }

            var cartInSession = HttpContext.Session.GetString("My-Cart");
            var carts = cartInSession != null ? JsonConvert.DeserializeObject<List<Cart>>(cartInSession) : new List<Cart>();

            if (carts.Any(c => c.Id == id))
            {
                carts.First(c => c.Id == id).Quantity += 1;
            }
            else
            {
                var item = new Cart()
                {
                    Id = p.Id,
                    Name = p.Title,
                    Price = (double)p.PriceNew.Value,
                    Quantity = 1,
                    Image = p.Image,
                    Total = (double)(p.PriceNew.Value * 1)
                };
                carts.Add(item);
            }

            HttpContext.Session.SetString("My-Cart", JsonConvert.SerializeObject(carts));

            int cartCount = carts.Sum(c => c.Quantity);
            HttpContext.Session.SetInt32("CartCount", cartCount);

            return RedirectToAction("Index");
        }

        // Xóa sản phẩm khỏi giỏ
        [HttpPost]
        public IActionResult Remove(int id)
        {
            var cartInSession = HttpContext.Session.GetString("My-Cart");
            var carts = cartInSession != null ? JsonConvert.DeserializeObject<List<Cart>>(cartInSession) : new List<Cart>();

            var cartItem = carts.FirstOrDefault(c => c.Id == id);
            if (cartItem != null)
            {
                carts.Remove(cartItem);
                HttpContext.Session.SetString("My-Cart", JsonConvert.SerializeObject(carts));
            }

            return Json(new { success = true });
        }

        // Cập nhật số lượng sản phẩm trong giỏ
        [HttpPost]
        public IActionResult Update(int id, int quantity)
        {
            var cartInSession = HttpContext.Session.GetString("My-Cart");
            var carts = cartInSession != null ? JsonConvert.DeserializeObject<List<Cart>>(cartInSession) : new List<Cart>();

            var cartItem = carts.FirstOrDefault(c => c.Id == id);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                cartItem.Total = cartItem.Price * cartItem.Quantity;
                HttpContext.Session.SetString("My-Cart", JsonConvert.SerializeObject(carts));
            }

            return Json(new { success = true });
        }

        // Xóa toàn bộ giỏ hàng
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("My-Cart");
            HttpContext.Session.Remove("CartCount");
            return RedirectToAction("Index");
        }

        // Trang đặt hàng
        public IActionResult Orders()
        {
            var customerIdSession = HttpContext.Session.GetInt32("CustomersID");
            if (!customerIdSession.HasValue)
            {
                return Redirect("/customers/Login/index/?url=/carts/orders");
            }

            var dataMember = _context.Customers.FirstOrDefault(x => x.Id == customerIdSession.Value);
            if (dataMember == null)
            {
                return Redirect("/customers/Login/index/?url=/carts/orders");
            }

            ViewBag.Customer = dataMember;

            var cartInSession = HttpContext.Session.GetString("My-Cart");
            var carts = cartInSession != null ? JsonConvert.DeserializeObject<List<Cart>>(cartInSession) : new List<Cart>();

            if (carts.Count == 0)
            {
                TempData["EmptyCartMessage"] = "Giỏ hàng của bạn đang trống. Vui lòng thêm sản phẩm!";
            }

            double total = carts.Sum(item => item.Quantity * item.Price);
            ViewBag.total = total;

            var dataPay = _context.PaymentMethods.ToList();
            ViewData["IdPayment"] = new SelectList(dataPay, "Id", "Name", 1);

            return View(carts);
        }

        // Thanh toán đơn hàng

        public async Task<IActionResult> OrderPay(IFormCollection form)
        {
            try
            {
                var cartInSession = HttpContext.Session.GetString("My-Cart");
                var carts = cartInSession != null ? JsonConvert.DeserializeObject<List<Cart>>(cartInSession) : new List<Cart>();

                if (carts.Count == 0)
                {
                    return RedirectToAction("Index");
                }

                var order = new Order
                {
                    NameReciver = form["NameReciver"],
                    Email = form["Email"],
                    Phone = form["Phone"],
                    Address = form["Address"],
                    Notes = form["Notes"],
                    OrdersDate = DateTime.Now,
                    Idpayment = !string.IsNullOrEmpty(form["IdPayment"]) ? long.Parse(form["IdPayment"]) : 1,
                    Status = 0 // Trạng thái 0: Chờ phê duyệt
                };

                var customerIdSession = HttpContext.Session.GetInt32("CustomersID");

                if (customerIdSession.HasValue)
                {
                    order.Idcustomer = customerIdSession.Value;
                }
                else
                {
                    // Nếu không có khách hàng trong session, yêu cầu đăng nhập
                    return RedirectToAction("Index");
                }

                decimal total = carts.Sum(item => item.Quantity * (decimal)item.Price);
                order.TotalMoney = total;

                var strOrderId = "DH." + DateTime.Now.ToString("yyyy-MM-dd.HH-mm-ss.fff");
                order.Idorders = strOrderId;

                _context.Add(order);
                await _context.SaveChangesAsync();

                var dataOrder = _context.Orders.OrderByDescending(x => x.Id).FirstOrDefault();

                foreach (var item in carts)
                {
                    var od = new Orderdetail
                    {
                        Idord = dataOrder.Id,
                        Idproduct = item.Id,
                        Qty = item.Quantity,
                        Price = (decimal)item.Price,
                        Total = (decimal)item.Total,
                        ReturnQty = 0
                    };

                    _context.Add(od);
                    await _context.SaveChangesAsync();
                }

                HttpContext.Session.Remove("My-Cart");
                HttpContext.Session.Remove("CartCount");

                TempData["SuccessMessage"] = "Đơn hàng của bạn đã được gửi đi và đang chờ phê duyệt!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã có lỗi xảy ra trong quá trình thanh toán.";
            }

            return View();
        }
        // Trang hiển thị danh sách đơn hàng của khách hàng
        public IActionResult OrderDetails()
        {
            var customerIdSession = HttpContext.Session.GetInt32("CustomersID");
            if (!customerIdSession.HasValue)
            {
                return Redirect("/customers/Login/index/?url=/carts/orderdetails");
            }

            var dataMember = _context.Customers.FirstOrDefault(x => x.Id == customerIdSession.Value);
            if (dataMember == null)
            {
                return Redirect("/customers/Login/index/?url=/carts/orderdetails");
            }

            var orders = _context.Orders
                .Where(o => o.Idcustomer == customerIdSession.Value)
                .OrderByDescending(o => o.OrdersDate)
                .ToList();

            var pendingOrders = orders.Where(o => o.Status == 0).ToList();  // Chờ phê duyệt
            var approvedOrders = orders.Where(o => o.Status == 1).ToList(); // Đang giao
            var deliveredOrders = orders.Where(o => o.Status == 2).ToList(); // Đã giao

            ViewBag.PendingOrders = pendingOrders;
            ViewBag.ApprovedOrders = approvedOrders;
            ViewBag.DeliveredOrders = deliveredOrders;

            return View();
        }

    }
}