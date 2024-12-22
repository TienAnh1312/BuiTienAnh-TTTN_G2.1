using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace K21CNT2_BuiTienAnh_2110900003.Models;

public partial class Partner
{
    public int Id { get; set; }

    [Display(Name = "Tên đối tác")]
    [Required(ErrorMessage = "Tên đối tác không được để trống")]
    [StringLength(255, ErrorMessage = "Tên đối tác không được vượt quá 255 ký tự")]
    public string? Title { get; set; }

    [Display(Name = "Logo")]
    [Required(ErrorMessage = "Logo không được để trống")]
    [StringLength(500, ErrorMessage = "Đường dẫn logo không được vượt quá 500 ký tự")]
    public string? Logo { get; set; }

    [Display(Name = "Địa chỉ URL")]
    [Required(ErrorMessage = "Địa chỉ URL không được để trống")]
    [Url(ErrorMessage = "Địa chỉ URL không hợp lệ")]
    public string? Url { get; set; }

    [Display(Name = "Thứ tự")]
    [Range(0, byte.MaxValue, ErrorMessage = "Thứ tự phải nằm trong phạm vi hợp lệ")]
    public byte? Orders { get; set; }

    [Display(Name = "Ngày tạo")]
    [DataType(DataType.Date)]
    public DateTime? CreatedDate { get; set; }

    [Display(Name = "Ngày cập nhật")]
    [DataType(DataType.Date)]
    public DateTime? UpdatedDate { get; set; }

    [Display(Name = "Người tạo")]
    [StringLength(100, ErrorMessage = "Tên người tạo không được vượt quá 100 ký tự")]
    public string? AdminCreated { get; set; }

    [Display(Name = "Người cập nhật")]
    [StringLength(100, ErrorMessage = "Tên người cập nhật không được vượt quá 100 ký tự")]
    public string? AdminUpdated { get; set; }

    [Display(Name = "Nội dung")]
    [StringLength(1000, ErrorMessage = "Nội dung không được vượt quá 1000 ký tự")]
    public string? Content { get; set; }

    [Display(Name = "Trạng thái")]
    public byte? Status { get; set; }

    [Display(Name = "Đã xóa")]
    public bool? Isdelete { get; set; }
}
