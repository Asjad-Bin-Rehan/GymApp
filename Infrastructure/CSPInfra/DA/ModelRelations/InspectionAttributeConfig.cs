using DM.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DA.ModelRelations
{
    internal class InspectionAttributeConfig : IEntityTypeConfiguration<Inspection_Attribute>
    {
        public void Configure(EntityTypeBuilder<Inspection_Attribute> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IntCode)
                   .ValueGeneratedOnAdd().HasAnnotation("DatabaseGenerated", "Identity");

            builder.HasMany(x => x.Inspection_Characteristics)
                   .WithOne(x => x.Inspection_Attribute)
                   .HasForeignKey(x => x.AttributeId);
        }
    }
}
