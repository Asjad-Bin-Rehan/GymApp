using DM.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.ModelRelations
{
    public class QualitativeResultPassStatusConfig : IEntityTypeConfiguration<Qualitative_Result_Pass_Status>
    {
        public void Configure(EntityTypeBuilder<Qualitative_Result_Pass_Status> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IntCode)
                   .ValueGeneratedOnAdd().HasAnnotation("DatabaseGenerated", "Identity");

            builder.HasOne(x => x.Qualitative_Inspection_Mapping)
                   .WithMany(x => x.Qualitative_Result_Pass_Statuses)
                   .HasForeignKey(x => x.QualitativeInspectionMappingId);

            builder.HasOne(x => x.Qualitative_Result)
                   .WithMany(x => x.Qualitative_Result_Pass_Statuses)
                   .HasForeignKey(x => x.QualitativeResultId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
