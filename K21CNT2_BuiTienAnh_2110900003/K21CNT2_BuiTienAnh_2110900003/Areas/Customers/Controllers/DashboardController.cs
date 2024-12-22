using K21CNT2_BuiTienAnh_2110900003.Controllers;
using K21CNT2_BuiTienAnh_2110900003.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace K21CNT2_BuiTienAnh_2110900003.Areas.Customers.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly DsmmvcContext _db;

        public DashboardController(ILogger<DashboardController> logger, DsmmvcContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index(int? id)
        {
            try
            {
                // Lấy danh sách sản phẩm có Cid = 1
                var productsCid1 = await _db.Products
                    .Where(p => p.Cid == 1)
                    .ToListAsync();

                // Lấy danh sách sản phẩm có Cid = 2
                var productsCid2 = await _db.Products
                    .Where(p => p.Cid == 2)
                    .ToListAsync();

                // Lấy danh sách Banners và Products từ database
                var banners = await _db.Banners.ToListAsync();
                var products = await _db.Products.ToListAsync();

                // Truyền dữ liệu vào ViewBag
                ViewBag.ProductsCid1 = productsCid1 ?? new List<Product>();
                ViewBag.ProductsCid2 = productsCid2 ?? new List<Product>();
                ViewBag.Banners = banners ?? new List<Banner>();
                ViewBag.Products = products ?? new List<Product>();

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra trong Dashboard/Index.");
                return RedirectToAction("Error", "Home", new { area = "" });
            }
        }
    }
}
