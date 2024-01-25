using GoodHamburger.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburger.Data.Map
{
    public class ExtraMap : IEntityTypeConfiguration<ExtraModel>
    {
        public void Configure(EntityTypeBuilder<ExtraModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Price).IsRequired();
        }
    }
}
