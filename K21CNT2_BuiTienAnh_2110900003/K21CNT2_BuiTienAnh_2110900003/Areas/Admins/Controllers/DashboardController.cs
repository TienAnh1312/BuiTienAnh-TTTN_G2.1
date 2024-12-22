using K21CNT2_BuiTienAnh_2110900003.Areas.Admins.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace K21CNT2_BuiTienAnh_2110900003.Areas.Admins.Controllers
{
    //[Area("Admins")]
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
