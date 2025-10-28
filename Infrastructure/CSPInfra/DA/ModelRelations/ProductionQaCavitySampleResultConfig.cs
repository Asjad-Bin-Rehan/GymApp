using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.ModelRelations
{
    public class ProductionQaCavitySampleResultConfig : IEntityTypeConfiguration<Production_QA_Cavity_Sample_Result>
    {
        public void Configure(EntityTypeBuilder<Production_QA_Cavity_Sample_Result> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Production_QA_Cavity_Sample)
                   .WithMany(x => x.Production_QA_Cavity_Sample_Results)
                   .HasForeignKey(x => x.QaCavitySampleId);

            builder.HasOne(x => x.Qualitative_Inspection_Mapping)
                   .WithMany(x => x.Production_QA_Cavity_Sample_Results)
                   .HasForeignKey(x => x.QualitativeInspectionMappingId);

            builder.HasOne(x => x.Quantitative_Inspection_Mapping)
                   .WithMany(x => x.Production_QA_Cavity_Sample_Results)
                   .HasForeignKey(x => x.QuantitativeInspectionMappingId);

            builder.HasMany(x => x.Log_Production_QA_Cavity_Sample_Results)
                   .WithOne(x => x.Production_QA_Cavity_Sample_Result)
                   .HasForeignKey(x => x.QaCavitySampleResultId);

            builder.HasOne(x => x.Qualitative_Result)
                   .WithMany(x => x.Production_QA_Cavity_Sample_Results)
                   .HasForeignKey(x => x.QualitativeResultId);
        }
    }
}
