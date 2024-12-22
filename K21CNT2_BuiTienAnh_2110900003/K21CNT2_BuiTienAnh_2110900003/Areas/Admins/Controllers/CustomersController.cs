using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using K21CNT2_BuiTienAnh_2110900003.Models;
using K21CNT2_BuiTienAnh_2110900003.Areas.Admins.Controllers;

namespace K21CNT2_BuiTienAnh_2110900003.Areas.Admins.Controllers
{
    public class CustomersController : BaseController
    {
        private readonly DsmmvcContext _context;

        public CustomersController(DsmmvcContext context)
        {
            _context = context;
        }

        // GET: Admins/Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        // GET: Admins/Customers/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Admins/Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admins/Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Username,Password,Address,Email,Phone,Avatar,CreatedDate,UpdatedDate,CreatedBy,UpdateBy,Isdelete,Isactive")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trùng lặp Username và Email khi tạo mới
                var existingCustomerByUserName = await _context.Customers
                    .FirstOrDefaultAsync(p => p.Username == customer.Username);

                var existingCustomerByEmail = await _context.Customers
                    .FirstOrDefaultAsync(p => p.Email == customer.Email);

                if (existingCustomerByUserName != null)
                {
                    ModelState.AddModelError("Username", "Tên Đăng Nhập này đã tồn tại.");
                }

                if (existingCustomerByEmail != null)
                {
                    ModelState.AddModelError("Email", "Email này đã tồn tại.");
                }
                // Xử lý ảnh, nếu có ảnh mới
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0 && files[0].Length > 0)
                {
                    var file = files[0];
                    var fileName = file.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "admins", "customers", fileName);

                    // Kiểm tra xem ảnh có tồn tại trong thư mục chưa (nghĩa là bạn không muốn đè ảnh cũ)
                    if (System.IO.File.Exists(path))
                    {
                        ModelState.AddModelError("Image", "Ảnh đã tồn tại trên hệ thống.");
                        return View(customer);
                    }

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        customer.Avatar = "images/admins/customers/" + fileName; // Lưu đường dẫn ảnh
                    }
                }
                // Nếu không có lỗi, thêm khách hàng mới
                if (existingCustomerByUserName == null && existingCustomerByEmail == null)
                {
                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(customer);
        }

        // GET: Admins/Customers/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Admins/Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Username,Password,Address,Email,Phone,Avatar,CreatedDate,UpdatedDate,CreatedBy,UpdateBy,Isdelete,Isactive")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra trùng lặp Username và Email khi sửa
                    var existingCustomerByUserName = await _context.Customers
                        .FirstOrDefaultAsync(p => p.Username == customer.Username && p.Id != customer.Id);

                    var existingCustomerByEmail = await _context.Customers
                        .FirstOrDefaultAsync(p => p.Email == customer.Email && p.Id != customer.Id);

                    if (existingCustomerByUserName != null)
                    {
                        ModelState.AddModelError("Username", "Tên Đăng Nhập này đã tồn tại.");
                    }

                    if (existingCustomerByEmail != null)
                    {
                        ModelState.AddModelError("Email", "Email này đã tồn tại.");
                    }

                    // Nếu không có lỗi, cập nhật khách hàng
                    if (existingCustomerByUserName == null && existingCustomerByEmail == null)
                    {
                        _context.Update(customer);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Admins/Customers/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Admins/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(long id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
