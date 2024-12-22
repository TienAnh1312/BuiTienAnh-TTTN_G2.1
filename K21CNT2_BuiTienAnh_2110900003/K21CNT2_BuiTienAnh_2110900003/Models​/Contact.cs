using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace K21CNT2_BuiTienAnh_2110900003.Models;

public partial class Contact
{
    public int Id { get; set; }

    [Display(Name = "Tiêu đề")]
    [Required(ErrorMessage = "Tiêu đề không được để trống")]
    [StringLength(255, ErrorMessage = "Tiêu đề không được vượt quá 255 ký tự")]
    public string? Title { get; set; }

    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string? Email { get; set; }

    [Display(Name = "Số điện thoại")]
    [DataType(DataType.PhoneNumber)]
    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string? Phone { get; set; }

    [Display(Name = "Địa chỉ")]
    [StringLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự")]
    public string? Address { get; set; }

    [Display(Name = "Nội dung")]
    [DataType(DataType.MultilineText)]
    [Required(ErrorMessage = "Nội dung không được để trống")]
    public string? Content { get; set; }

    [Display(Name = "Ngày tạo")]
    [DataType(DataType.Date)]
    public DateTime? CreatedDate { get; set; }

    [Display(Name = "Ngày cập nhật")]
    [DataType(DataType.Date)]
    public DateTime? UpdatedDate { get; set; }

    [Display(Name = "Người tạo")]
    [StringLength(255, ErrorMessage = "Tên người tạo không được vượt quá 255 ký tự")]
    public string? AdminCreated { get; set; }

    [Display(Name = "Người cập nhật")]
    [StringLength(255, ErrorMessage = "Tên người cập nhật không được vượt quá 255 ký tự")]
    public string? AdminUpdated { get; set; }

    [Display(Name = "Trạng thái")]
    public byte? Status { get; set; }

    [Display(Name = "Đã xóa")]
    public bool? Isdelete { get; set; }
}
