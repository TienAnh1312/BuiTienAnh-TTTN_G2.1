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
        public async Task<IActionResult> Index()
        {
            var dsmmvcContext = _context.Orderdetails
                .Include(o => o.IdordNavigation)
                .Include(o => o.IdproductNavigation);  // Bao gồm thông tin sản phẩm (chứa ảnh)
            return View(await dsmmvcContext.ToListAsync());
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
    }
}
