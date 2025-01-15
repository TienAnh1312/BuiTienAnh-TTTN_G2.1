using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using K21CNT2_BuiTienAnh_2110900003.Models;

namespace K21CNT2_BuiTienAnh_2110900003.Areas.Admins.Controllers
{
    //[Area("Admins")]
    public class DashboardController : BaseController
    {
        private readonly DsmmvcContext _context;

        public DashboardController(DsmmvcContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Thống kê tổng số đơn hàng
            var totalOrders = _context.Orders.Count();

            // Thống kê tổng số đơn hàng
            var totalProducts = _context.Products.Count();

            // Thống kê số lượng đơn hàng đã phê duyệt (Status = 1)
            var approvedOrders = _context.Orders.Count(o => o.Status == 1);

            // Thống kê số lượng đơn hàng chờ phê duyệt (Status = 0)
            var pendingOrders = _context.Orders.Count(o => o.Status == 0);

            //// Thống kê số lượng đơn hàng đã xóa (Isdelete = true)
            //var deletedOrders = _context.Orders.Count(o => o.Isdelete);

            // Truyền các thống kê vào ViewData để hiển thị trong view
            ViewData["TotalProducts"] = totalProducts;
            ViewData["TotalOrders"] = totalOrders;
            ViewData["ApprovedOrders"] = approvedOrders;
            ViewData["PendingOrders"] = pendingOrders;
            //ViewData["DeletedOrders"] = deletedOrders;

            return View();
        }
    }
}
