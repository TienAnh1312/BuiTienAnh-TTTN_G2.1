using K21CNT2_BuiTienAnh_2110900003.Areas.Customers.Models;
using K21CNT2_BuiTienAnh_2110900003.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace K21CNT2_BuiTienAnh_2110900003.Areas.Customers.Controllers
{
    [Area("Customers")]
    public class LoginController : Controller
    {
        public DsmmvcContext _context;
        public LoginController(DsmmvcContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost] // POST -> khi submit form
        public IActionResult Index(LoginCustomers model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không hợp lệ.");
                return View(model);
            }

            var pass = model.Password;
            var dataLogin = _context.Customers.FirstOrDefault(x => x.Email.Equals(model.Email) && x.Password.Equals(pass));
            if (dataLogin != null)
            {
                HttpContext.Session.SetString("CustomersLogin", model.Email);
                //HttpContext.Session.SetInt32("CustomersID", model.CustomersID);
                HttpContext.Session.SetInt32("CustomersID", (int)dataLogin.Id);

                return RedirectToAction("Index", "Dashboard", new { CustomersID = dataLogin.Id });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không chính xác.");
                return View(model);
            }

        }
        [HttpGet]// thoát đăng nhập, huỷ session
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("CustomersLogin"); // huỷ session với key  đã lưu trước đó

            return RedirectToAction("Index");
        }
    }
}