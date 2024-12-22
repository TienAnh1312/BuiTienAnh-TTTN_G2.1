using K21CNT2_BuiTienAnh_2110900003.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace K21CNT2_BuiTienAnh_2110900003.Areas.Customers.Controllers
{
    [Area("Customers")]
    public class ProductsController : Controller
    {
        private readonly DsmmvcContext _context;

        public ProductsController(DsmmvcContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            // Lấy danh sách sản phẩm theo danh mục từ cơ sở dữ liệu
            var productsCid1 = await _context.Products.Where(p => p.Cid == 1).ToListAsync();
            var productsCid2 = await _context.Products.Where(p => p.Cid == 2).ToListAsync();
            var productsCid3 = await _context.Products.Where(p => p.Cid == 3).ToListAsync();

            // Gán danh sách sản phẩm vào ViewBag để truyền sang view
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

    }
}
