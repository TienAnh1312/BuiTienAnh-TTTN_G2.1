using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace K21CNT2_BuiTienAnh_2110900003.Models;

public partial class Orderdetail
{
    public long Id { get; set; }

    [Display(Name = "Mã đơn hàng")]
    public long? Idord { get; set; }

    [Display(Name = "Mã sản phẩm")]
    public int? Idproduct { get; set; }

    [Display(Name = "Giá")]
    [DataType(DataType.Currency)]
    public decimal? Price { get; set; }

    [Display(Name = "Số lượng")]
    public int? Qty { get; set; }

    [Display(Name = "Tổng tiền")]
    [DataType(DataType.Currency)]
    public decimal? Total { get; set; }

    [Display(Name = "Số lượng trả lại")]
    public int? ReturnQty { get; set; }

    public virtual Order? IdordNavigation { get; set; }

    public virtual Product? IdproductNavigation { get; set; }
}
