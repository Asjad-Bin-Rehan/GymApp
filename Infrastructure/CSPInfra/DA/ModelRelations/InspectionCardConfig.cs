using DM.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ModelRelations
{
    public class InspectionCardConfig : IEntityTypeConfiguration<Inspection_Card>
    {
        public void Configure(EntityTypeBuilder<Inspection_Card> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IntCode)
                   .ValueGeneratedOnAdd().HasAnnotation("DatabaseGenerated", "Identity");

            builder.HasMany(x => x.Inspection_Characteristic_Mappings)
                   .WithOne(x => x.Inspection_Card)
                   .HasForeignKey(x => x.InspectionCardId);

            builder.HasMany(x => x.Item_Inspection_Cards)
                   .WithOne(x => x.Inspection_Card)
                   .HasForeignKey(x => x.InspectionCardId);
        }
    }
}