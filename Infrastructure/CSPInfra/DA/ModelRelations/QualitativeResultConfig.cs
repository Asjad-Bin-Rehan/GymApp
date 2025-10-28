using DM.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.ModelRelations
{
    public class QualitativeResultConfig : IEntityTypeConfiguration<Qualitative_Result>
    {
        public void Configure(EntityTypeBuilder<Qualitative_Result> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IntCode)
                   .ValueGeneratedOnAdd().HasAnnotation("DatabaseGenerated", "Identity");

            builder.HasMany(x => x.Qualitative_Result_Pass_Statuses)
                   .WithOne(x => x.Qualitative_Result)
                   .HasForeignKey(x => x.QualitativeResultId);

            builder.HasMany(x => x.Purchase_QC_Sample_Results)
                   .WithOne(x => x.Qualitative_Result)
                   .HasForeignKey(x => x.QualitativeResultId);

            builder.HasMany(x => x.Production_QC_Sample_Results)
                   .WithOne(x => x.Qualitative_Result)
                   .HasForeignKey(x => x.QualitativeResultId);

            builder.HasMany(x => x.Production_QA_Cavity_Sample_Results)
                   .WithOne(x => x.Qualitative_Result)
                   .HasForeignKey(x => x.QualitativeResultId);
        }
    }
}
