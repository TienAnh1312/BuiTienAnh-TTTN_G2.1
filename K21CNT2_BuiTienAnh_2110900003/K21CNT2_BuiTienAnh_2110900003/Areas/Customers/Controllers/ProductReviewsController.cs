using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using K21CNT2_BuiTienAnh_2110900003.Models;

namespace K21CNT2_BuiTienAnh_2110900003.Areas.Customers.Controllers
{
    
    public class ProductReviewsController : BaseController
    {
        private readonly DsmmvcContext _context;

        public ProductReviewsController(DsmmvcContext context)
        {
            _context = context;
        }

        // GET: Customers/ProductReviews
        // GET: Customers/ProductReviews
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5; // Define the number of reviews per page
            var productReviews = _context.ProductReviews
                .Include(p => p.Customer)
                .Include(p => p.Product)
                .OrderByDescending(r => r.CreatedAt) // Optional: Order by date or rating
                .Skip((page - 1) * pageSize) // Skip previous pages' reviews
                .Take(pageSize); // Take reviews for the current page

            // Calculate total pages for pagination
            var totalReviews = await _context.ProductReviews.CountAsync();
            var totalPages = (int)Math.Ceiling(totalReviews / (double)pageSize);

            // Pass pagination data to the view
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(await productReviews.ToListAsync());
        }



        // GET: Customers/ProductReviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productReview = await _context.ProductReviews
                .Include(p => p.Customer)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productReview == null)
            {
                return NotFound();
            }

            return View(productReview);
        }

        // GET: Customers/ProductReviews/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            return View();
        }

        // POST: Customers/ProductReviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,CustomerId,Rating,Comment,CreatedAt")] ProductReview productReview)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productReview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", productReview.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", productReview.ProductId);
            return View(productReview);
        }

        // GET: Customers/ProductReviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productReview = await _context.ProductReviews.FindAsync(id);
            if (productReview == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", productReview.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", productReview.ProductId);
            return View(productReview);
        }

        // POST: Customers/ProductReviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,CustomerId,Rating,Comment,CreatedAt")] ProductReview productReview)
        {
            if (id != productReview.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductReviewExists(productReview.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", productReview.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", productReview.ProductId);
            return View(productReview);
        }

        // GET: Customers/ProductReviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productReview = await _context.ProductReviews
                .Include(p => p.Customer)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productReview == null)
            {
                return NotFound();
            }

            return View(productReview);
        }

        // POST: Customers/ProductReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productReview = await _context.ProductReviews.FindAsync(id);
            if (productReview != null)
            {
                _context.ProductReviews.Remove(productReview);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductReviewExists(int id)
        {
            return _context.ProductReviews.Any(e => e.Id == id);
        }

        // Thêm hành động POST để xử lý gửi đánh giá và bình luận
        [HttpPost]
        public async Task<IActionResult> AddReview(int productId, int rating, string comment)
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            var customerId = HttpContext.Session.GetInt32("CustomersID");
            if (customerId == null)
            {
                ModelState.AddModelError("", "Bạn cần đăng nhập để đánh giá.");
                return RedirectToAction("Login", "Login");
            }

            // Tạo đối tượng đánh giá mới
            var productReview = new ProductReview
            {
                ProductId = productId,
                CustomerId = customerId.Value,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.Now
            };

            // Thêm đánh giá vào cơ sở dữ liệu
            _context.ProductReviews.Add(productReview);
            await _context.SaveChangesAsync();

            // Quay lại trang chi tiết sản phẩm
            return RedirectToAction("Details", "Products", new { id = productId });
        }

    }
}
