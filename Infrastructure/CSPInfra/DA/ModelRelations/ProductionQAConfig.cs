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
    public class ProductionQAConfig : IEntityTypeConfiguration<Production_QA>
    {
        public void Configure(EntityTypeBuilder<Production_QA> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IntCode)
                   .ValueGeneratedOnAdd()
                   .HasAnnotation("DatabaseGenerated", "Identity");

            builder.HasOne(x => x.Item)
                   .WithMany(x => x.Production_QA)
                   .HasForeignKey(x => x.ItemId);

            builder.HasMany(x => x.Production_QA_Cavities)
                   .WithOne(x => x.Production_QA)
                   .HasForeignKey(x => x.QaId);

            builder.HasMany(x => x.Log_Post_Saps)
                   .WithOne(x => x.Production_QA)
                   .HasForeignKey(x => x.QId);
        }
    }
}
