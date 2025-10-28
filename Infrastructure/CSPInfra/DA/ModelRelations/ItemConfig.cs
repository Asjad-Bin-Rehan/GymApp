using DM.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.ModelRelations
{
    public class ItemConfig : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IntCode)
                   .ValueGeneratedOnAdd().HasAnnotation("DatabaseGenerated", "Identity");

            builder.HasMany(x => x.Item_Inspection_Cards)
                   .WithOne(x => x.Item)
                   .HasForeignKey(x => x.ItemId);

            builder.HasOne(x => x.Item_Sample)
                   .WithOne(x => x.Item)
                   .HasForeignKey<Item_Sample>(x => x.ItemId);

            builder.HasMany(x => x.Purchase_QC)
                   .WithOne(x => x.Item)
                   .HasForeignKey(x => x.ItemId);

            builder.HasMany(x => x.Production_QC)
                   .WithOne(x => x.Item)
                   .HasForeignKey(x => x.ItemId);
        }
    }
}
