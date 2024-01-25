namespace GoodHamburger.DTO
{
    public class OrderUpdateDTO
    {
        public int Id { get; set; }
        public int SandwichID { get; set; }
        public ICollection<int>? ExtraID{ get; set; }
    }
}
