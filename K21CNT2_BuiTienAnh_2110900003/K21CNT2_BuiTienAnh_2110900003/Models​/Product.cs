using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace K21CNT2_BuiTienAnh_2110900003.Models;

public partial class Product
{
    public int Id { get; set; }

    [Display(Name = "Mã danh mục")]
    public int? Cid { get; set; }

    [Display(Name = "Mã sản phẩm")]
    public string? Code { get; set; }

    [Display(Name = "Tên sản phẩm")]
    public string? Title { get; set; }

    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    [Display(Name = "Nội dung")]
    public string? Content { get; set; }

    [Display(Name = "Hình ảnh")]
    public string? Image { get; set; }

    [Display(Name = "Tiêu đề meta")]
    public string? MetaTitle { get; set; }

    [Display(Name = "Từ khóa meta")]
    public string? MetaKeyword { get; set; }

    [Display(Name = "Mô tả meta")]
    public string? MetaDescription { get; set; }

    [Display(Name = "Slug")]
    public string? Slug { get; set; }

    [Display(Name = "Giá cũ")]
    [DataType(DataType.Currency)]
    public decimal? PriceOld { get; set; }

    [Display(Name = "Giá mới")]
    [DataType(DataType.Currency)]
    public decimal? PriceNew { get; set; }

    [Display(Name = "Kích thước")]
    public string? Size { get; set; }

    [Display(Name = "Lượt xem")]
    public int? Views { get; set; }

    [Display(Name = "Lượt thích")]
    public int? Likes { get; set; }

    [Display(Name = "Đánh giá sao")]
    public double? Star { get; set; }

    [Display(Name = "Sản phẩm nổi bật")]
    public byte? Home { get; set; }

    [Display(Name = "Sản phẩm hot")]
    public byte? Hot { get; set; }

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

    [Display(Name = "Trạng thái")]
    public byte? Status { get; set; }

    [Display(Name = "Đã xóa")]
    public bool? Isdelete { get; set; }

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();
}
