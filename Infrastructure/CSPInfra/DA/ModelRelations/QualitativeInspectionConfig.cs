using DM.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.ModelRelations
{
    public class QualitativeInspectionConfig : IEntityTypeConfiguration<Qualitative_Inspection>
    {
        public void Configure(EntityTypeBuilder<Qualitative_Inspection> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IntCode)
                   .ValueGeneratedOnAdd().HasAnnotation("DatabaseGenerated", "Identity");

            builder.HasOne(x => x.Item_Inspection_Card)
                   .WithOne(x => x.Qualitative_Inspection)
                   .HasForeignKey<Qualitative_Inspection>(x => x.ItemInspectionCardId);

            builder.HasMany(x => x.Qualitative_Inspection_Mappings)
                   .WithOne(x => x.Qualitative_Inspection)
                   .HasForeignKey(x => x.QualitativeInspectionId);
        }
    }
}
