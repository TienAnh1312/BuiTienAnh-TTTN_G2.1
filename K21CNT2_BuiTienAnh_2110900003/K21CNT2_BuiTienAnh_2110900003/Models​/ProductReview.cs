using System;
using System.Collections.Generic;

namespace K21CNT2_BuiTienAnh_2110900003.Models​;

public partial class ProductReview
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public long CustomerId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CurrentPage { get; set; }

    public int? TotalPages { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
