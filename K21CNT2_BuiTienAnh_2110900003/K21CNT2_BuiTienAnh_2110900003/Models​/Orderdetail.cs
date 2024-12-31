using System;
using System.Collections.Generic;

namespace K21CNT2_BuiTienAnh_2110900003.Models​;

public partial class Orderdetail
{
    public long Id { get; set; }

    public long? Idord { get; set; }

    public int? Idproduct { get; set; }

    public decimal? Price { get; set; }

    public int? Qty { get; set; }

    public decimal? Total { get; set; }

    public int? ReturnQty { get; set; }

    public long? Status { get; set; }

    public virtual Order? IdordNavigation { get; set; }

    public virtual Product? IdproductNavigation { get; set; }
}
