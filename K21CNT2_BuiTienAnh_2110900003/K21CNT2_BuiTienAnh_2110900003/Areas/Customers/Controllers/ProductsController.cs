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
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductReviews)
                    .ThenInclude(r => r.Customer) // Để hiển thị tên khách hàng
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

    }
}
