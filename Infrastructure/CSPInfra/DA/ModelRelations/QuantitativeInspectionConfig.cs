using DM.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.ModelRelations
{
    public class QuantitativeInspectionConfig : IEntityTypeConfiguration<Quantitative_Inspection>
    {
        public void Configure(EntityTypeBuilder<Quantitative_Inspection> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IntCode)
                   .ValueGeneratedOnAdd().HasAnnotation("DatabaseGenerated", "Identity");

            builder.HasOne(x => x.Item_Inspection_Card)
                   .WithOne(x => x.Quantitative_Inspection)
                   .HasForeignKey<Quantitative_Inspection>(x => x.ItemInspectionCardId);

            builder.HasMany(x => x.Quantitative_Inspection_Mappings)
                   .WithOne(x => x.Quantitative_Inspection)
                   .HasForeignKey(x => x.QuantitativeInspectionId);
        }
    }
}
