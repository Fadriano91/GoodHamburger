namespace GoodHamburger.Models
{
    public class ExtraModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public decimal Price{ get; set; }
        public ICollection<OrderModel>? Orders { get; set; }
    }
}
