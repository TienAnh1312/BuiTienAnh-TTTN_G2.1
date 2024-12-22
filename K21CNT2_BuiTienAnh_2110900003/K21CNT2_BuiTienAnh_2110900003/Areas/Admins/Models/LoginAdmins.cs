namespace K21CNT2_BuiTienAnh_2110900003.Areas.Admins.Models
{
    public class LoginAdmins
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public bool Remember { get; set; }
    }
}
