using DM.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DA.ModelRelations
{
    public class LogSamplingRangeConfig : IEntityTypeConfiguration<Log_Sampling_Range>
    {
        public void Configure(EntityTypeBuilder<Log_Sampling_Range> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.SamplingRange)
                   .WithMany(x => x.Log_Sampling_Ranges)
                   .HasForeignKey(x => x.SamplingRangeId);
        }
    }
}
