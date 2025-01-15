namespace K21CNT2_BuiTienAnh_2110900003.Areas.Customers.Models
{
    public class MoMoPaymentRequest
    {
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
        public string orderId { get; set; }
        public string amount { get; set; }
    }
}
