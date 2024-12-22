using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace K21CNT2_BuiTienAnh_2110900003.Models;

public partial class Customer
{
    public long Id { get; set; }

    [Display(Name = "Họ và tên")]
    [Required(ErrorMessage = "Họ và tên không được để trống")]
    [StringLength(255, ErrorMessage = "Họ và tên không được vượt quá 255 ký tự")]
    public string? Name { get; set; }

    [Display(Name = "Tên đăng nhập")]
    [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
    [StringLength(50, ErrorMessage = "Tên đăng nhập không được vượt quá 50 ký tự")]
    public string? Username { get; set; }

    [Display(Name = "Mật khẩu")]
    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
    public string? Password { get; set; }

    [Display(Name = "Địa chỉ")]
    [StringLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự")]
    public string? Address { get; set; }

    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email không được để trống")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string? Email { get; set; }

    [Display(Name = "Số điện thoại")]
    [DataType(DataType.PhoneNumber)]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string? Phone { get; set; }

    [Display(Name = "Ảnh đại diện")]
    public string? Avatar { get; set; }

    [Display(Name = "Ngày tạo")]
    [DataType(DataType.Date)]
    public DateTime? CreatedDate { get; set; }

    [Display(Name = "Ngày cập nhật")]
    [DataType(DataType.Date)]
    public DateTime? UpdatedDate { get; set; }

    [Display(Name = "Người tạo")]
    public long? CreatedBy { get; set; }

    [Display(Name = "Người cập nhật")]
    public long? UpdateBy { get; set; }

    [Display(Name = "Đã xóa")]
    public byte? Isdelete { get; set; }

    [Display(Name = "Kích hoạt")]
    public byte? Isactive { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
