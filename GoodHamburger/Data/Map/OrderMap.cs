using GoodHamburger.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace GoodHamburger.Data.Map
{
    public class OrderMap : IEntityTypeConfiguration<OrderModel>
    {
        public void Configure(EntityTypeBuilder<OrderModel> builder)
        {

            builder.HasMany(x => x.Extras)
            .WithMany(p => p.Orders)
            .UsingEntity<OrderExtraModel>(
            p => p.HasOne(pa => pa.Extra).WithMany(),
            p => p.HasOne(pa => pa.Order).WithMany().HasForeignKey(pa => pa.OrderId));
        }
    }
}
