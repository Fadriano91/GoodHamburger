using System.ComponentModel.DataAnnotations.Schema;

namespace GoodHamburger.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        [ForeignKey(nameof(SandwichId))]
        public int SandwichId { get; set; }
        public SandwichModel? Sandwich { get; set; }
        public ICollection<ExtraModel>? Extras { get; set; }
        public decimal Total { get; set; }
    }
}
