using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DM.DomainModels;

namespace DA.ModelRelations
{
    public class ItemInspectionCardConfig : IEntityTypeConfiguration<Item_Inspection_Card>
    {
        public void Configure(EntityTypeBuilder<Item_Inspection_Card> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IntCode)
                   .ValueGeneratedOnAdd().HasAnnotation("DatabaseGenerated", "Identity");

            builder.HasOne(x => x.Inspection_Card)
                   .WithMany(x => x.Item_Inspection_Cards)
                   .HasForeignKey(x => x.InspectionCardId);

            builder.HasOne(x => x.Item)
                   .WithMany(x => x.Item_Inspection_Cards)
                   .HasForeignKey(x => x.ItemId);
        }
    }
}
