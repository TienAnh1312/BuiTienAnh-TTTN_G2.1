using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using K21CNT2_BuiTienAnh_2110900003.Models;

namespace K21CNT2_BuiTienAnh_2110900003.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class OrdersController : BaseController
    {
        private readonly DsmmvcContext _context;

        public OrdersController(DsmmvcContext context)
        {
            _context = context;
        }

        // GET: Admins/Orders
        public IActionResult Index(int? status, string searchString)
        {
            // Khởi tạo danh sách đơn hàng
            var orders = _context.Orders.AsQueryable();

            // Nếu có từ khóa tìm kiếm, lọc theo Idorders hoặc NameReciver
            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o => o.Idorders.Contains(searchString) || o.NameReciver.Contains(searchString));
            }

            // Nếu có tham số status, lọc đơn hàng theo trạng thái
            if (status.HasValue)
            {
                orders = orders.Where(o => o.Status == status.Value);
            }
            else
            {
                // Nếu không có tham số status, lấy tất cả đơn hàng (cả chờ phê duyệt và đã phê duyệt)
                orders = orders.Where(o => o.Status == 0 || o.Status == 1); // Lọc cả đơn hàng chờ và đã phê duyệt
            }

            // Truyền từ khóa tìm kiếm vào ViewData để hiển thị lại trên form
            ViewData["SearchString"] = searchString;

            // Trả về danh sách đơn hàng đã lọc
            return View(orders.ToList());
        }




        // Chi tiết đơn hàng
        public IActionResult Details(long id)
        {
            // Tìm đơn hàng theo Id
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            {
            if (order == null)
                return NotFound(); // Nếu không tìm thấy đơn hàng, trả về lỗi 404
            }

            // Lấy chi tiết đơn hàng từ bảng Orderdetails bằng Id
            var orderDetails = _context.Orderdetails.Where(od => od.Idord == order.Id).ToList();
            ViewBag.OrderDetails = orderDetails; // Chuyển chi tiết đơn hàng cho view

            return View(order); // Trả về view với đơn hàng và chi tiết đơn hàng
        }


        // GET: Admins/Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admins/Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Idorders,OrdersDate,Idcustomer,Idpayment,TotalMoney,Notes,NameReciver,Address,Email,Phone,Isdelete,Isactive,Status")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Admins/Orders/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Admins/Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Idorders,OrdersDate,Idcustomer,Idpayment,TotalMoney,Notes,NameReciver,Address,Email,Phone,Isdelete,Isactive,Status")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Admins/Orders/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Orderdetails) // Bao gồm Orderdetails
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admins/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var order = await _context.Orders
                .Include(o => o.Orderdetails) // Bao gồm Orderdetails
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order != null)
            {
                try
                {
                    // Xóa các chi tiết đơn hàng liên quan
                    var orderDetails = order.Orderdetails.ToList();
                    _context.Orderdetails.RemoveRange(orderDetails);  // Xóa tất cả các Orderdetail liên quan

                    // Sau đó, xóa đơn hàng
                    _context.Orders.Remove(order);  // Xóa đơn hàng
                    await _context.SaveChangesAsync();  // Lưu thay đổi

                    TempData["SuccessMessage"] = "Đơn hàng và các chi tiết đã được xóa thành công!";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xóa đơn hàng: " + ex.Message;
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng để xóa!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(long id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        // Phê duyệt đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(long id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            // Cập nhật trạng thái đơn hàng thành "Đã phê duyệt" (Status = 1)
            order.Status = 1;
            _context.Update(order);
            await _context.SaveChangesAsync();

            // Thông báo khi phê duyệt thành công
            TempData["SuccessMessage"] = "Đơn hàng đã được phê duyệt thành công!";

            // Quay lại danh sách các đơn hàng đã phê duyệt mà không cần tải lại trang
            return RedirectToAction(nameof(Index), new { status = 1 });  // Trả về danh sách đơn hàng đã phê duyệt
        }
    }
}
