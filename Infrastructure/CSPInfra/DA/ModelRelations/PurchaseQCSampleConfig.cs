using DM.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.ModelRelations
{
    public class PurchaseQCSampleConfig : IEntityTypeConfiguration<Purchase_QC_Sample>
    {
        public void Configure(EntityTypeBuilder<Purchase_QC_Sample> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IntCode)
                   .ValueGeneratedOnAdd()
                   .HasAnnotation("DatabaseGenerated", "Identity");

            builder.HasOne(x => x.Purchase_QC)
                   .WithMany(x => x.Purchase_QC_Samples)
                   .HasForeignKey(x => x.QcId);

            builder.HasMany(x => x.Purchase_QC_Sample_Results)
                   .WithOne(x => x.Purchase_QC_Sample)
                   .HasForeignKey(x => x.QcSampleId);
        }
    }
}