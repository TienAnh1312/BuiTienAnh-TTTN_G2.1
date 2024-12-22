using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace K21CNT2_BuiTienAnh_2110900003.Models;

public partial class Order
{
    public long Id { get; set; }

    [Display(Name = "Mã đơn hàng")]
    [Required(ErrorMessage = "Mã đơn hàng không được để trống")]
    [StringLength(50, ErrorMessage = "Mã đơn hàng không được vượt quá 50 ký tự")]
    public string? Idorders { get; set; }

    [Display(Name = "Ngày đặt hàng")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Ngày đặt hàng không được để trống")]
    public DateTime? OrdersDate { get; set; }

    [Display(Name = "Khách hàng")]
    [Required(ErrorMessage = "Khách hàng không được để trống")]
    public long? Idcustomer { get; set; }

    [Display(Name = "Phương thức thanh toán")]
    [Required(ErrorMessage = "Phương thức thanh toán không được để trống")]
    public long? Idpayment { get; set; }

    [Display(Name = "Tổng tiền")]
    [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền phải là một số không âm")]
    public decimal? TotalMoney { get; set; }

    [Display(Name = "Ghi chú")]
    [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự")]
    public string? Notes { get; set; }

    [Display(Name = "Tên người nhận")]
    [StringLength(255, ErrorMessage = "Tên người nhận không được vượt quá 255 ký tự")]
    public string? NameReciver { get; set; }

    [Display(Name = "Địa chỉ")]
    [StringLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự")]
    public string? Address { get; set; }

    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string? Email { get; set; }

    [Display(Name = "Số điện thoại")]
    [DataType(DataType.PhoneNumber)]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string? Phone { get; set; }

    [Display(Name = "Đã xóa")]
    public byte? Isdelete { get; set; }

    [Display(Name = "Trạng thái")]
    public byte? Isactive { get; set; }

    public virtual Customer? IdcustomerNavigation { get; set; }

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();
}
