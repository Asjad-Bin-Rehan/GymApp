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
    public class ItemSampleConfig : IEntityTypeConfiguration<Item_Sample>
    {
        public void Configure(EntityTypeBuilder<Item_Sample> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IntCode)
                   .ValueGeneratedOnAdd().HasAnnotation("DatabaseGenerated", "Identity");

            builder.HasOne(x => x.Item)
                   .WithOne(x => x.Item_Sample)
                   .HasForeignKey<Item_Sample>(x => x.ItemId);

            builder.HasMany(x => x.Sampling_Ranges)
                   .WithOne(x => x.Item_Sample)
                   .HasForeignKey(x => x.ItemSampleId);
        }
    }
}
