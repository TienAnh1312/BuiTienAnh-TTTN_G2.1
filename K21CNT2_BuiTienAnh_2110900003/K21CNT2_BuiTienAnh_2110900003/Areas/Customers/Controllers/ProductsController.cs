using K21CNT2_BuiTienAnh_2110900003.Areas.Customers.Models;
using K21CNT2_BuiTienAnh_2110900003.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace K21CNT2_BuiTienAnh_2110900003.Areas.Customers.Controllers
{
    [Area("Customers")]
    public class ProductsController : Controller
    {
        private readonly DsmmvcContext _context;

        // Constructor nhận context từ dependency injection
        public ProductsController(DsmmvcContext context)
        {
            _context = context;
        }

        // GET: Products (Hiển thị danh sách sản phẩm theo danh mục)
        public async Task<IActionResult> Index()
        {
            // Lấy danh sách sản phẩm theo danh mục
            var productsCid1 = await _context.Products.Where(p => p.Cid == 1).ToListAsync(); // Sản phẩm mới
            var productsCid2 = await _context.Products.Where(p => p.Cid == 2).ToListAsync(); // Sản phẩm nổi
            var productsCid3 = await _context.Products.Where(p => p.Cid == 3).ToListAsync(); // Sản phẩm cũ

            // Truyền danh sách vào ViewBag để hiển thị trong view
            ViewBag.ProductsCid1 = productsCid1;
            ViewBag.ProductsCid2 = productsCid2;
            ViewBag.ProductsCid3 = productsCid3;

            return View();
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id, int page = 1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductReviews)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            int pageSize = 3; // Define the number of reviews per page
            var reviews = _context.ProductReviews
                .Where(r => r.ProductId == id)
                .Include(r => r.Customer)
                .OrderByDescending(r => r.CreatedAt) // Optional: Sort reviews by creation date
                .Skip((page - 1) * pageSize) // Skip the reviews from the previous pages
                .Take(pageSize); // Take the reviews for the current page

            var totalReviews = await _context.ProductReviews.CountAsync(r => r.ProductId == id);
            var totalPages = (int)Math.Ceiling(totalReviews / (double)pageSize);

            // Pass the reviews and pagination data to the view
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            product.ProductReviews = await reviews.ToListAsync();
            return View(product);
        }

    }
}
