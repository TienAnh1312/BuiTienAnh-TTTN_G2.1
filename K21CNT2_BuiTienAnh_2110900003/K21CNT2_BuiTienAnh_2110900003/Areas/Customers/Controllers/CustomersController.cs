using K21CNT2_BuiTienAnh_2110900003.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.IO;
using System;

namespace K21CNT2_BuiTienAnh_2110900003.Areas.Customers.Controllers
{
    [Area("Customers")]
    public class CustomersController : Controller
    {
        private readonly DsmmvcContext _context;

        public CustomersController(DsmmvcContext context)
        {
            _context = context;
        }

        // Chỉnh sửa thông tin khách hàng
        public IActionResult Edit()
        {
            // Lấy CustomerID từ session
            var customerId = HttpContext.Session.GetInt32("CustomersID");

            if (customerId.HasValue)
            {
                // Tìm khách hàng theo ID
                var customer = _context.Customers.FirstOrDefault(c => c.Id == customerId.Value);

                if (customer != null)
                {
                    return View(customer); // Truyền dữ liệu khách hàng vào view
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy khách hàng!";
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập trước.";
                return RedirectToAction("Index", "Login", new { area = "Customers" });
            }
        }

        // Xử lý yêu cầu POST để chỉnh sửa thông tin khách hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Customer customerModel, IFormFile avatar)
        {
            // Kiểm tra tính hợp lệ của model
            if (!ModelState.IsValid)
            {
                // Nếu model không hợp lệ, trả về view với các lỗi validation
                TempData["ErrorMessage"] = "Thông tin bạn nhập không hợp lệ. Vui lòng kiểm tra lại.";
                return View(customerModel);
            }

            var customerId = HttpContext.Session.GetInt32("CustomersID");

            if (customerId.HasValue)
            {
                // Tìm khách hàng theo ID
                var existingCustomer = _context.Customers.FirstOrDefault(c => c.Id == customerId.Value);

                if (existingCustomer != null)
                {
                    try
                    {
                        // Cập nhật dữ liệu khách hàng
                        existingCustomer.Name = customerModel.Name;
                        existingCustomer.Email = customerModel.Email;
                        existingCustomer.Phone = customerModel.Phone;
                        existingCustomer.Address = customerModel.Address;
                        existingCustomer.Username = customerModel.Username;

                        // Kiểm tra xem có ảnh mới không
                        if (avatar != null && avatar.Length > 0)
                        {
                            // Tạo tên file và đường dẫn lưu trữ ảnh
                            var fileName = Path.GetFileName(avatar.FileName);
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "admins", "customers", fileName);

                            // Kiểm tra xem ảnh có tồn tại trong thư mục chưa
                            if (System.IO.File.Exists(filePath))
                            {
                                ModelState.AddModelError("Avatar", "Ảnh đã tồn tại trên hệ thống.");
                                return View(customerModel);
                            }

                            // Lưu ảnh vào thư mục
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                avatar.CopyTo(stream);
                            }

                            // Cập nhật đường dẫn ảnh vào cơ sở dữ liệu
                            existingCustomer.Avatar = "images/admins/customers/" + fileName;
                        }

                        // Lưu thay đổi vào cơ sở dữ liệu
                        _context.SaveChanges();

                        TempData["SuccessMessage"] = "Thông tin của bạn đã được cập nhật thành công!";
                        return RedirectToAction("Index", "Dashboard", new { area = "Customers" });
                    }
                    catch (Exception ex)
                    {
                        // Xử lý lỗi khi lưu vào cơ sở dữ liệu
                        TempData["ErrorMessage"] = "Có lỗi xảy ra khi lưu thay đổi: " + ex.Message;
                        return View(customerModel);
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy khách hàng!";
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập trước.";
                return RedirectToAction("Index", "Login", new { area = "Customers" });
            }
        }

    }
}
