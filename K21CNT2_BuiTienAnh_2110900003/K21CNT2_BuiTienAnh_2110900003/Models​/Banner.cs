using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace K21CNT2_BuiTienAnh_2110900003.Models;

public partial class Banner
{
    public int Id { get; set; }

    [Display(Name = "Hình ảnh")]
    //[Required(ErrorMessage = "Hình ảnh không được để trống")]
    public string? Image { get; set; }

    [Display(Name = "Tiêu đề chính")]
    [Required(ErrorMessage = "Tiêu đề chính không được để trống")]
    [StringLength(255, ErrorMessage = "Tiêu đề chính không được vượt quá 255 ký tự")]
    public string? Title { get; set; }

    [Display(Name = "Tiêu đề phụ")]
    [StringLength(255, ErrorMessage = "Tiêu đề phụ không được vượt quá 255 ký tự")]
    public string? SubTitle { get; set; }

    [Display(Name = "Đường dẫn")]
    //[Required(ErrorMessage = "Đường dẫn không được để trống")]
    [Url(ErrorMessage = "Đường dẫn không hợp lệ")]
    public string? Urls { get; set; }

    [Display(Name = "Thứ tự")]
    [Required(ErrorMessage = "Thứ tự không được để trống")]
    public int Orders { get; set; }

    [Display(Name = "Loại banner")]
    [Required(ErrorMessage = "Loại banner không được để trống")]
    public string? Type { get; set; }

    [Display(Name = "Ngày tạo")]
    [DataType(DataType.Date)]
    public DateTime? CreatedDate { get; set; }

    [Display(Name = "Ngày cập nhật")]
    [DataType(DataType.Date)]
    public DateTime? UpdatedDate { get; set; }

    [Display(Name = "Người tạo")]
    public string? AdminCreated { get; set; }

    [Display(Name = "Người cập nhật")]
    public string? AdminUpdated { get; set; }

    [Display(Name = "Ghi chú")]
    [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự")]
    public string? Notes { get; set; }

    [Display(Name = "Trạng thái")]
    [Required(ErrorMessage = "Trạng thái không được để trống")]
    public byte Status { get; set; }

    [Display(Name = "Đã xóa")]
    public bool Isdelete { get; set; }
}
