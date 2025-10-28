using DM.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.ModelRelations
{
    public class ProductionQCSampleConfig : IEntityTypeConfiguration<Production_QC_Sample>
    {
        public void Configure(EntityTypeBuilder<Production_QC_Sample> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IntCode)
                   .ValueGeneratedOnAdd()
                   .HasAnnotation("DatabaseGenerated", "Identity");

            builder.HasOne(x => x.Production_QC)
                   .WithMany(x => x.Production_QC_Samples)
                   .HasForeignKey(x => x.QcId);

            builder.HasMany(x => x.Production_QC_Sample_Results)
                   .WithOne(x => x.Production_QC_Sample)
                   .HasForeignKey(x => x.QcSampleId);
        }
    }
}