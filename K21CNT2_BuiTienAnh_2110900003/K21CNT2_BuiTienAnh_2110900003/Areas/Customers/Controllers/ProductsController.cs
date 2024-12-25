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

        // GET: Products/Details/5 (Hiển thị chi tiết sản phẩm)
        public async Task<IActionResult> Details(int? id)
        {
            // Kiểm tra nếu id bị null
            if (id == null)
            {
                return NotFound();
            }

            // Lấy sản phẩm từ cơ sở dữ liệu theo id
            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

            // Kiểm tra nếu không tìm thấy sản phẩm
            if (product == null)
            {
                return NotFound();
            }

            // Truyền sản phẩm vào view
            return View(product);
        }
    }
}
