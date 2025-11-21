using DM.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.ModelRelations
{
    public class UnitOfMeasureConfig : IEntityTypeConfiguration<Unit_Of_Measure>
    {
        public void Configure(EntityTypeBuilder<Unit_Of_Measure> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IntCode)
                   .ValueGeneratedOnAdd().HasAnnotation("DatabaseGenerated", "Identity");

            builder.HasMany(x => x.Quantitative_Inspection_Mappings)
                   .WithOne(x => x.Unit_Of_Measure)
                   .HasForeignKey(x => x.UoMId);
        }
    }
}
