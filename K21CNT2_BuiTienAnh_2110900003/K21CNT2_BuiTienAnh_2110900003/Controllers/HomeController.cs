using System.Diagnostics;
using K21CNT2_BuiTienAnh_2110900003.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace K21CNT2_BuiTienAnh_2110900003.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DsmmvcContext _db;

        public HomeController(ILogger<HomeController> logger, DsmmvcContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Lấy danh sách Banners và Products từ database
                var banners = await _db.Banners.ToListAsync();
                var products = await _db.Products.ToListAsync();

                // Lấy danh sách sản phẩm có Cid = 1
                var productsCid1 = await _db.Products
                    .Where(p => p.Cid == 1)
                    .ToListAsync();

                // Lấy danh sách sản phẩm có Cid = 2
                var productsCid2 = await _db.Products
                    .Where(p => p.Cid == 2)
                    .ToListAsync();

                // Truyền dữ liệu vào ViewBag
                ViewBag.Banners = banners ?? new List<Banner>();
                ViewBag.Products = products ?? new List<Product>();
                ViewBag.ProductsCid1 = productsCid1 ?? new List<Product>();
                ViewBag.ProductsCid2 = productsCid2 ?? new List<Product>();

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra khi truy vấn dữ liệu từ Index.");
                return RedirectToAction("Error");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
