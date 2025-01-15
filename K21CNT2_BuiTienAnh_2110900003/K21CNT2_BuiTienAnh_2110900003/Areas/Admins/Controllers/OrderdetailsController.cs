using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using K21CNT2_BuiTienAnh_2110900003.Models;

namespace K21CNT2_BuiTienAnh_2110900003.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class OrderdetailsController : Controller
    {
        private readonly DsmmvcContext _context;

        public OrderdetailsController(DsmmvcContext context)
        {
            _context = context;
        }

        // GET: Admins/Orderdetails
        public async Task<IActionResult> Index(int? status)
        {
            var dsmmvcContext = _context.Orderdetails
                .Include(o => o.IdordNavigation)  // Lấy thông tin đơn hàng
                .Include(o => o.IdproductNavigation)  // Lấy thông tin sản phẩm
                //.Where(od => od.IdordNavigation.Status == 1)  // Chỉ lấy các đơn hàng đã phê duyệt
                .AsQueryable();  // Chuyển sang IQueryable để có thể linh hoạt lọc

            // Kiểm tra nếu tham số status có giá trị và lọc theo trạng thái đơn hàng
            if (status.HasValue)
            {
                dsmmvcContext = dsmmvcContext.Where(od => od.IdordNavigation.Status == status.Value);
            }

            return View(await dsmmvcContext.ToListAsync());  // Trả về danh sách chi tiết đơn hàng đã lọc
        }

        // GET: Admins/Orderdetails/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderdetail = await _context.Orderdetails
                .Include(o => o.IdordNavigation)
                .Include(o => o.IdproductNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderdetail == null)
            {
                return NotFound();
            }

            return View(orderdetail);
        }

        // GET: Admins/Orderdetails/Create
        public IActionResult Create()
        {
            ViewData["Idord"] = new SelectList(_context.Orders, "Id", "Id");
            ViewData["Idproduct"] = new SelectList(_context.Products, "Id", "Id");
            return View();
        }

        // POST: Admins/Orderdetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Idord,Idproduct,Price,Qty,Total,ReturnQty")] Orderdetail orderdetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderdetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idord"] = new SelectList(_context.Orders, "Id", "Id", orderdetail.Idord);
            ViewData["Idproduct"] = new SelectList(_context.Products, "Id", "Id", orderdetail.Idproduct);
            return View(orderdetail);
        }

        // GET: Admins/Orderdetails/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderdetail = await _context.Orderdetails.FindAsync(id);
            if (orderdetail == null)
            {
                return NotFound();
            }
            ViewData["Idord"] = new SelectList(_context.Orders, "Id", "Id", orderdetail.Idord);
            ViewData["Idproduct"] = new SelectList(_context.Products, "Id", "Id", orderdetail.Idproduct);
            return View(orderdetail);
        }

        // POST: Admins/Orderdetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Idord,Idproduct,Price,Qty,Total,ReturnQty")] Orderdetail orderdetail)
        {
            if (id != orderdetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderdetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderdetailExists(orderdetail.Id))
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
            ViewData["Idord"] = new SelectList(_context.Orders, "Id", "Id", orderdetail.Idord);
            ViewData["Idproduct"] = new SelectList(_context.Products, "Id", "Id", orderdetail.Idproduct);
            return View(orderdetail);
        }

        private bool OrderdetailExists(long id)
        {
            return _context.Orderdetails.Any(e => e.Id == id);
        }

        // GET: Admins/Orderdetails/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderdetail = await _context.Orderdetails
                .Include(o => o.IdordNavigation)
                .Include(o => o.IdproductNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderdetail == null)
            {
                return NotFound();
            }

            return View(orderdetail);
        }

        // POST: Admins/Orderdetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var orderdetail = await _context.Orderdetails
                .Include(od => od.IdordNavigation) // Bao gồm Order
                .FirstOrDefaultAsync(m => m.Id == id);

            if (orderdetail != null)
            {
                try
                {
                    var order = orderdetail.IdordNavigation;  // Lấy đơn hàng liên quan

                    // Xóa Orderdetail trước
                    _context.Orderdetails.Remove(orderdetail);
                    await _context.SaveChangesAsync();

                    // Kiểm tra xem có còn Orderdetail nào khác trong đơn hàng không, nếu không thì xóa Order
                    if (order != null && !order.Orderdetails.Any())
                    {
                        _context.Orders.Remove(order);  // Nếu không còn chi tiết đơn hàng, xóa Order
                        await _context.SaveChangesAsync();
                    }

                    TempData["SuccessMessage"] = "Chi tiết đơn hàng đã được xóa thành công!";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xóa chi tiết đơn hàng: " + ex.Message;
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy chi tiết đơn hàng để xóa!";
            }

            return RedirectToAction(nameof(Index));
        }

        // Thêm hành động MarkAsDelivered để đánh dấu trạng thái "Đã giao"
        public async Task<IActionResult> MarkAsDelivered(long id)
        {
            // Lấy thông tin chi tiết đơn hàng
            var orderDetail = await _context.Orderdetails.FirstOrDefaultAsync(od => od.Id == id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            // Lấy thông tin đơn hàng liên quan
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderDetail.Idord);

            if (order == null)
            {
                return NotFound();
            }

            // Kiểm tra nếu đơn hàng đang giao, thay đổi trạng thái
            if (order.Status == 1) // 1 là trạng thái "Đang giao"
            {
                order.Status = 2; // 2 là trạng thái "Đã giao"
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                // Thông báo thành công
                TempData["SuccessMessage"] = "Đơn hàng đã được đánh dấu là đã giao!";
            }

            // Sau khi thay đổi trạng thái, chuyển hướng về trang Index với tham số status = 2 (Đã giao)
            return RedirectToAction("Index", new { status = 2 });
        }

    }
}
