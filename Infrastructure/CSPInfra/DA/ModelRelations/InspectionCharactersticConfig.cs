using DM.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.ModelRelations
{
    public class InspectionCharacteristicConfig : IEntityTypeConfiguration<Inspection_Characteristic>
    {
        public void Configure(EntityTypeBuilder<Inspection_Characteristic> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IntCode)
                   .ValueGeneratedOnAdd().HasAnnotation("DatabaseGenerated", "Identity");

            builder.HasMany(x => x.Inspection_Characteristic_Mappings)
                   .WithOne(x => x.Inspection_Characteristic)
                   .HasForeignKey(x => x.CharacteristicId);

            builder.HasMany(x => x.Quantitative_Inspection_Mappings)
                   .WithOne(x => x.Inspection_Characteristic)
                   .HasForeignKey(x => x.CharacteristicId);

            builder.HasMany(x => x.Qualitative_Inspection_Mappings)
                   .WithOne(x => x.Inspection_Characteristic)
                   .HasForeignKey(x => x.CharacteristicId);

            builder.HasOne(x => x.Inspection_Attribute)
                   .WithMany(x => x.Inspection_Characteristics)
                   .HasForeignKey(x => x.AttributeId);
        }
    }
}
