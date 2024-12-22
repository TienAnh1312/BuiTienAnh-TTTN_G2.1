using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using K21CNT2_BuiTienAnh_2110900003.Models;
using K21CNT2_BuiTienAnh_2110900003.Areas.Admins.Controllers;

namespace K21CNT2_BuiTienAnh_2110900003.Areas.Admins.Controllers
{
    public class AdminUsersController : BaseController
    {
        private readonly DsmmvcContext _context;

        public AdminUsersController(DsmmvcContext context)
        {
            _context = context;
        }

        // GET: Admins/AdminUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.AdminUsers.ToListAsync());
        }

        // GET: Admins/AdminUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminUser = await _context.AdminUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adminUser == null)
            {
                return NotFound();
            }

            return View(adminUser);
        }

        // GET: Admins/AdminUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admins/AdminUsers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Account,Password,MaNhanSu,Name,Phone,Email,Avatar,IdPhongBan,NgayTao,NguoiTao,NgayCapNhat,NguoiCapNhat,SessionToken,Salt,IsAdmin,TrangThai,IsDelete")] AdminUser adminUser)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trùng lặp Account và Email khi tạo mới
                var existingAccount = await _context.AdminUsers
                    .FirstOrDefaultAsync(p => p.Account == adminUser.Account);

                var existingEmail = await _context.AdminUsers
                    .FirstOrDefaultAsync(p => p.Email == adminUser.Email);

                if (existingAccount != null)
                {
                    ModelState.AddModelError("Account", "Tài khoản này đã tồn tại.");
                }

                if (existingEmail != null)
                {
                    ModelState.AddModelError("Email", "Email này đã tồn tại.");
                }

                // Nếu không có lỗi, thêm người dùng mới
                if (existingAccount == null && existingEmail == null)
                {
                    _context.Add(adminUser);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(adminUser);
        }

        // GET: Admins/AdminUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminUser = await _context.AdminUsers.FindAsync(id);
            if (adminUser == null)
            {
                return NotFound();
            }
            return View(adminUser);
        }

        // POST: Admins/AdminUsers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Account,Password,MaNhanSu,Name,Phone,Email,Avatar,IdPhongBan,NgayTao,NguoiTao,NgayCapNhat,NguoiCapNhat,SessionToken,Salt,IsAdmin,TrangThai,IsDelete")] AdminUser adminUser)
        {
            if (id != adminUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra trùng lặp Account và Email khi sửa
                    var existingAccount = await _context.AdminUsers
                        .FirstOrDefaultAsync(p => p.Account == adminUser.Account && p.Id != adminUser.Id);

                    var existingEmail = await _context.AdminUsers
                        .FirstOrDefaultAsync(p => p.Email == adminUser.Email && p.Id != adminUser.Id);

                    if (existingAccount != null)
                    {
                        ModelState.AddModelError("Account", "Tài khoản này đã tồn tại.");
                    }

                    if (existingEmail != null)
                    {
                        ModelState.AddModelError("Email", "Email này đã tồn tại.");
                    }

                    // Nếu không có lỗi, cập nhật người dùng
                    if (existingAccount == null && existingEmail == null)
                    {
                        _context.Update(adminUser);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminUserExists(adminUser.Id))
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
            return View(adminUser);
        }

        // GET: Admins/AdminUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminUser = await _context.AdminUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adminUser == null)
            {
                return NotFound();
            }

            return View(adminUser);
        }

        // POST: Admins/AdminUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adminUser = await _context.AdminUsers.FindAsync(id);
            if (adminUser != null)
            {
                _context.AdminUsers.Remove(adminUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminUserExists(int id)
        {
            return _context.AdminUsers.Any(e => e.Id == id);
        }
    }
}
