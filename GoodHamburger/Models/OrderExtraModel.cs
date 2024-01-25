namespace GoodHamburger.Models
{
    public class OrderExtraModel
    {
        public int OrderId { get; set; }
        public OrderModel? Order { get; set; }
        public int ExtraId { get; set; }
        public ExtraModel? Extra { get; set; }
    }
}
