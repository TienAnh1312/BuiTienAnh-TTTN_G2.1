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
    public class BannersController : BaseController
    {
        private readonly DsmmvcContext _context;

        public BannersController(DsmmvcContext context)
        {
            _context = context;
        }

        // GET: Admins/Banners
        public async Task<IActionResult> Index()
        {          
            return View(await _context.Banners.ToListAsync());
        }

        // GET: Admins/Banners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // GET: Admins/Banners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admins/Banners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Image,Title,SubTitle,Urls,Orders,Type,CreatedDate,UpdatedDate,AdminCreated,AdminUpdated,Notes,Status,Isdelete")] Banner banner)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;

                // Kiểm tra nếu có ảnh tải lên
                if (files.Count > 0 && files[0].Length > 0)
                {
                    var file = files[0];
                    var fileName = Path.GetFileName(file.FileName);
                    var fileExtension = Path.GetExtension(fileName).ToLower();

                    // Kiểm tra định dạng file (nếu cần)
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".jfif" };
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("Image", "Chỉ chấp nhận các định dạng ảnh .jpg, .jpeg, .png. , .jfif");
                        return View(banner);
                    }

                    // Tạo tên file duy nhất (thêm GUID vào tên file)
                    var newFileName = Guid.NewGuid().ToString() + fileExtension;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "admins", "banners", newFileName);

                    // Kiểm tra nếu tệp đã tồn tại
                    if (System.IO.File.Exists(path))
                    {
                        ModelState.AddModelError("Image", "Ảnh đã tồn tại trên hệ thống.");
                        return View(banner);
                    }

                    // Lưu tệp vào thư mục
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn ảnh vào thuộc tính Image trong model (dạng string)
                    banner.Image = "images/admins/banners/" + newFileName;
                }

                // Thiết lập ngày tạo và ngày cập nhật tự động
                banner.CreatedDate = DateTime.Now;
                banner.UpdatedDate = DateTime.Now;

                // Lưu model vào cơ sở dữ liệu
                _context.Add(banner);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(banner);
        }


        // GET: Admins/Banners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }

        // POST: Admins/Banners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,Title,SubTitle,Urls,Orders,Type,CreatedDate,UpdatedDate,AdminCreated,AdminUpdated,Notes,Status,Isdelete")] Banner banner)
        {
            if (id != banner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;

                    // Kiểm tra nếu có ảnh tải lên
                    if (files.Count > 0 && files[0].Length > 0)
                    {
                        var file = files[0];
                        var fileName = Path.GetFileName(file.FileName);
                        var fileExtension = Path.GetExtension(fileName).ToLower();

                        // Kiểm tra định dạng file (nếu cần)
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".jfif" };
                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("Image", "Chỉ chấp nhận các định dạng ảnh .jpg, .jpeg, .png. , .jfif");
                            return View(banner);
                        }

                        // Tạo tên file duy nhất (thêm GUID vào tên file)
                        var newFileName = Guid.NewGuid().ToString() + fileExtension;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "admins", "banners", newFileName);

                        // Kiểm tra nếu tệp đã tồn tại
                        if (System.IO.File.Exists(path))
                        {
                            ModelState.AddModelError("Image", "Ảnh đã tồn tại trên hệ thống.");
                            return View(banner);
                        }

                        // Lưu tệp vào thư mục
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Cập nhật đường dẫn ảnh vào thuộc tính Image trong model (dạng string)
                        banner.Image = "images/admins/banners/" + newFileName;
                    }

                    // Cập nhật ngày cập nhật
                    banner.UpdatedDate = DateTime.Now;

                    // Cập nhật model vào cơ sở dữ liệu
                    _context.Update(banner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannerExists(banner.Id))
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
            return View(banner);
        }


        // GET: Admins/Banners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // POST: Admins/Banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner != null)
            {
                _context.Banners.Remove(banner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BannerExists(int id)
        {
            return _context.Banners.Any(e => e.Id == id);
        }
    }
}
