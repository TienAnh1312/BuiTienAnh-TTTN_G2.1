using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using K21CNT2_BuiTienAnh_2110900003.Models;
using K21CNT2_BuiTienAnh_2110900003.Areas.Admins.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace K21CNT2_BuiTienAnh_2110900003.Areas.Admins.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly DsmmvcContext _context;

        public ProductsController(DsmmvcContext context)
        {
            _context = context;
        }

        // GET: Admins/Products
        public async Task<IActionResult> Index(int? cid)
        {
            // Tạo danh sách các danh mục giả (hoặc bạn có thể lấy từ CSDL)
            var categoriess = new List<SelectListItem>
            {
                new SelectListItem { Text = "Sản phẩm mới", Value = "1" },
                new SelectListItem { Text = "Sản phẩm Nổi", Value = "2" },
                new SelectListItem { Text = "Sản phẩm cũ", Value = "3" }
            };

            // Truyền danh sách vào View
            ViewData["Categoriess"] = categoriess;

            // Lấy tất cả sản phẩm
            var products = _context.Products.AsQueryable();

            // Nếu có giá trị cid được chọn, lọc sản phẩm theo cid
            if (cid.HasValue)
            {
                products = products.Where(p => p.Cid == cid.Value);
            }

            return View(await products.ToListAsync());
        }


        // GET: Admins/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admins/Products/Create
        public IActionResult Create()
        {
            // Tạo danh sách các danh mục giả (hoặc bạn có thể lấy từ CSDL)
            var categoriess = new List<SelectListItem>
            {
                new SelectListItem { Text = "Sản phẩm mới", Value = "1" },
                new SelectListItem { Text = "Sản phẩm Nổi", Value = "2" },
                new SelectListItem { Text = "Sản phẩm cũ", Value = "3" }
            };

            // Truyền danh sách vào ViewData
            ViewData["Categoriess"] = categoriess;

            return View();
        }

        // POST: Admins/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cid,Code,Title,Description,Content,Image,MetaTitle,MetaKeyword,MetaDescription,Slug,PriceOld,PriceNew,Size,Views,Likes,Star,Home,Hot,AdminCreated,AdminUpdated,Status,Isdelete,Quantity")] Product product)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trùng lặp Code khi sửa
                var existingProductByCode = await _context.Products
                    .FirstOrDefaultAsync(p => p.Code == product.Code && p.Id != product.Id);

                if (existingProductByCode != null)
                {
                    ModelState.AddModelError("Code", "Sản phẩm này đã tồn tại.");
                }

                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                // Xử lý ảnh, nếu có ảnh mới
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0 && files[0].Length > 0)
                {
                    var file = files[0];
                    var fileName = file.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "admins", "products", fileName);

                    // Kiểm tra xem ảnh có tồn tại trong thư mục chưa (nghĩa là bạn không muốn đè ảnh cũ)
                    if (System.IO.File.Exists(path))
                    {
                        ModelState.AddModelError("Image", "Ảnh đã tồn tại trên hệ thống.");
                        return View(product);
                    }

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        product.Image = "images/admins/products/" + fileName; // Lưu đường dẫn ảnh
                    }
                }

                // Thiết lập ngày tạo và ngày cập nhật tự động
                product.CreatedDate = DateTime.Now;
                product.UpdatedDate = DateTime.Now;

                // Thêm sản phẩm vào cơ sở dữ liệu
                _context.Add(product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Admins/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Prepare categories for the dropdown (or fetch from DB)
            var categoriess = new List<SelectListItem>
            {
                new SelectListItem { Text = "Sản phẩm mới", Value = "1" },
                new SelectListItem { Text = "Sản phẩm Nổi", Value = "2" },
                new SelectListItem { Text = "Sản phẩm cũ", Value = "3" }
            };

            // Pass the categories to the view
            ViewData["Categoriess"] = categoriess;

            return View(product);
        }

        // POST: Admins/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cid,Code,Title,Description,Content,Image,MetaTitle,MetaKeyword,MetaDescription,Slug,PriceOld,PriceNew,Size,Views,Likes,Star,Home,Hot,AdminCreated,AdminUpdated,Status,Isdelete,Quantity")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check for duplicate product by Code and Image
                    var existingProductByCode = await _context.Products
                        .FirstOrDefaultAsync(p => p.Code == product.Code && p.Id != product.Id);

                    var existingProductByImage = await _context.Products
                        .FirstOrDefaultAsync(p => p.Image == product.Image && p.Id != product.Id);

                    if (existingProductByCode != null && existingProductByImage != null)
                    {
                        ModelState.AddModelError(string.Empty, "Sản phẩm với Mã sản phẩm và Ảnh này đã tồn tại.");
                    }
                    else if (existingProductByCode != null)
                    {
                        ModelState.AddModelError("Code", "Mã Sản phẩm này đã tồn tại.");
                    }
                    else if (existingProductByImage != null)
                    {
                        ModelState.AddModelError("Image", "Sản phẩm với Ảnh này đã tồn tại.");
                    }

                    // Process the image if a new file is uploaded
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count() > 0 && files[0].Length > 0)
                    {
                        var file = files[0];
                        var fileName = file.FileName;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "admins", "products", fileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            product.Image = "images/admins/products/" + fileName; // Save the image path
                        }
                    }

                    // Set the updated date automatically
                    product.UpdatedDate = DateTime.Now;

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
        // GET: Admins/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admins/Products/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Tìm sản phẩm cần xóa
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(); // Trả về NotFound nếu sản phẩm không tồn tại
            }

            // Chỉ xóa sản phẩm trong bảng Products
            _context.Products.Remove(product);

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Xử lý lỗi khi xóa nếu có liên kết với dữ liệu khác hoặc vấn đề cơ sở dữ liệu
                ModelState.AddModelError(string.Empty, "Không thể xóa sản phẩm vì có lỗi xảy ra.");
                return View(product); // Trả lại view với thông báo lỗi
            }

            // Điều hướng về trang Index sau khi xóa thành công
            return RedirectToAction(nameof(Index));
        }

    }
}
