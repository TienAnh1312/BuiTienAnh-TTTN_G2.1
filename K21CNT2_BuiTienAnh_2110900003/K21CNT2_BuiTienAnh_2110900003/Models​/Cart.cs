using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace K21CNT2_BuiTienAnh_2110900003.Models;

public partial class Cart
{
    public int Id { get; set; }

    [Display(Name = "Tên sản phẩm")]
    public string Name { get; set; } = null!;

    [Display(Name = "Hình ảnh")]
    public string? Image { get; set; }

    [Display(Name = "Số lượng")]
    public int Quantity { get; set; }

    [Display(Name = "Đơn giá")]
    [DataType(DataType.Currency)]
    public double Price { get; set; }

    [Display(Name = "Thành tiền")]
    [DataType(DataType.Currency)]
    public double Total { get; set; }
}
