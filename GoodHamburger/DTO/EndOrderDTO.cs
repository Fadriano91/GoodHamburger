using GoodHamburger.Models;

namespace GoodHamburger.DTO
{
    public class EndOrderDTO
    {
        public int OrderID { get; set; }
        public SandwichModel? Sandwich { get; set; }
        public List<ExtraEndDTO>? Extras { get; set; }
        public decimal Total { get; set; }
    }
}
