using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DA.ModelRelations
{
    public class SamplingRangeConfig : IEntityTypeConfiguration<Sampling_Range>
    {
        public void Configure(EntityTypeBuilder<Sampling_Range> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IntCode)
                   .ValueGeneratedOnAdd().HasAnnotation("DatabaseGenerated", "Identity");

            builder.HasOne(x => x.Item_Sample)
                   .WithMany(x => x.Sampling_Ranges)
                   .HasForeignKey(x => x.ItemSampleId);
        }
    }
}
