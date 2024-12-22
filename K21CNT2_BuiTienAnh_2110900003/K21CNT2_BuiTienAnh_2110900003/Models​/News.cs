using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace K21CNT2_BuiTienAnh_2110900003.Models;

public partial class News
{
    public int Id { get; set; }

    [Display(Name = "Mã tin")]
    [Required(ErrorMessage = "Mã tin không được để trống")]
    [StringLength(50, ErrorMessage = "Mã tin không được vượt quá 50 ký tự")]
    public string? Code { get; set; }

    [Display(Name = "Tiêu đề")]
    [Required(ErrorMessage = "Tiêu đề không được để trống")]
    [StringLength(255, ErrorMessage = "Tiêu đề không được vượt quá 255 ký tự")]
    public string? Title { get; set; }

    [Display(Name = "Mô tả")]
    [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
    public string? Description { get; set; }

    [Display(Name = "Nội dung")]
    [Required(ErrorMessage = "Nội dung không được để trống")]
    public string? Content { get; set; }

    [Display(Name = "Hình ảnh")]
    [StringLength(255, ErrorMessage = "Đường dẫn hình ảnh không được vượt quá 255 ký tự")]
    public string? Image { get; set; }

    [Display(Name = "Tiêu đề meta")]
    [StringLength(255, ErrorMessage = "Tiêu đề meta không được vượt quá 255 ký tự")]
    public string? MetaTitle { get; set; }

    [Display(Name = "Từ khóa chính")]
    [StringLength(255, ErrorMessage = "Từ khóa chính không được vượt quá 255 ký tự")]
    public string? MainKeyword { get; set; }

    [Display(Name = "Từ khóa meta")]
    [StringLength(255, ErrorMessage = "Từ khóa meta không được vượt quá 255 ký tự")]
    public string? MetaKeyword { get; set; }

    [Display(Name = "Mô tả meta")]
    [StringLength(500, ErrorMessage = "Mô tả meta không được vượt quá 500 ký tự")]
    public string? MetaDescription { get; set; }

    [Display(Name = "Slug")]
    [StringLength(255, ErrorMessage = "Slug không được vượt quá 255 ký tự")]
    public string? Slug { get; set; }

    [Display(Name = "Số lượt xem")]
    [Range(0, int.MaxValue, ErrorMessage = "Số lượt xem phải là một số không âm")]
    public int? Views { get; set; }

    [Display(Name = "Số lượt thích")]
    [Range(0, int.MaxValue, ErrorMessage = "Số lượt thích phải là một số không âm")]
    public int? Likes { get; set; }

    [Display(Name = "Số sao")]
    [Range(0, 5, ErrorMessage = "Số sao phải trong khoảng từ 0 đến 5")]
    public double? Star { get; set; }

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

    [Display(Name = "Trạng thái")]
    public byte? Status { get; set; }

    [Display(Name = "Đã xóa")]
    public bool? Isdelete { get; set; }
}
