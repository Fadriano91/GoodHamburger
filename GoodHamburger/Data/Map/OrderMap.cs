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




            //builder.HasMany(ex => ex.Extras)
            //    .WithMany(e => e.Orders)
            //    .UsingEntity<OrderExtraModel>(j => j.ToTable("OrderExtras"));

            builder.HasMany(x => x.Extras)
            .WithMany(p => p.Orders)
            .UsingEntity<OrderExtraModel>(
            p => p.HasOne(pa => pa.Extra).WithMany(),
            p => p.HasOne(pa => pa.Order).WithMany().HasForeignKey(pa => pa.OrderId));


            // Indica que é uma relação muitos-para-muitos
            //.UsingEntity<Dictionary<string, object>>(
            //    "OrderExtra",
            //    j => j.HasOne<ExtraModel>().WithMany().HasForeignKey("ExtraId"),
            //    j => j.HasOne<OrderModel>().WithMany().HasForeignKey("OrderId"),
            //    j =>
            //    {
            //        j.HasKey("OrderId", "ExtraId");
            //    }

        }
    }
}
