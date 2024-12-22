using System;
using System.Collections.Generic;

namespace K21CNT2_BuiTienAnh_2110900003.Models​;

public partial class Login
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool Remember { get; set; }
}
