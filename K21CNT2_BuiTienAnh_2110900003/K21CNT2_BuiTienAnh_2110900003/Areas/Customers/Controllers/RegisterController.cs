using K21CNT2_BuiTienAnh_2110900003.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace K21CNT2_BuiTienAnh_2110900003.Areas.Customers.Controllers
{
    [Area("Customers")]
    public class RegisterController : Controller
    {
        private readonly DsmmvcContext _context;

        public RegisterController(DsmmvcContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([Bind("Name,Username,Password,Email,Phone,Address,Avatar")] Customer model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra nếu email đã tồn tại trong cơ sở dữ liệu (so sánh không phân biệt hoa/thường)
            var existingEmail = _context.Customers
                                        .FirstOrDefault(x => x.Email.ToLower() == model.Email.ToLower());

            if (existingEmail != null)
            {
                ModelState.AddModelError(string.Empty, "Email này đã được đăng ký.");
                return View(model);
            }

            // Kiểm tra nếu tên đăng nhập đã tồn tại trong cơ sở dữ liệu
            var existingUsername = _context.Customers
                                           .FirstOrDefault(x => x.Username.ToLower() == model.Username.ToLower());

            if (existingUsername != null)
            {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập này đã có người sử dụng.");
                return View(model);
            }

            // Mã hóa mật khẩu
            var passwordHasher = new PasswordHasher<Customer>();
            model.Password = passwordHasher.HashPassword(model, model.Password);

            // Lưu thông tin khách hàng mới vào cơ sở dữ liệu
            model.CreatedDate = DateTime.Now;
            model.Isactive = 1; // Kích hoạt tài khoản
            model.Isdelete = 0; // Chưa xóa
            _context.Customers.Add(model);
            _context.SaveChanges();

            // Sau khi đăng ký thành công, chuyển hướng đến trang đăng nhập
            return RedirectToAction("Index", "Login");
        }
    }
}
