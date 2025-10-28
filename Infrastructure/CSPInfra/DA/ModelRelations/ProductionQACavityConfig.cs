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
    public class ProductionQACavityConfig : IEntityTypeConfiguration<Production_QA_Cavity>
    {
        public void Configure(EntityTypeBuilder<Production_QA_Cavity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IntCode)
                   .ValueGeneratedOnAdd()
                   .HasAnnotation("DatabaseGenerated", "Identity");

            builder.HasOne(x => x.Production_QA)
                   .WithMany(x => x.Production_QA_Cavities)
                   .HasForeignKey(x => x.QaId);

            builder.HasMany(x => x.Production_QA_Cavity_Samples)
                   .WithOne(x => x.Production_QA_Cavity)
                   .HasForeignKey(x => x.CavityId);

            builder.HasMany(x => x.Log_Production_QA_Cavities)
                   .WithOne(x => x.Production_QA_Cavity)
                   .HasForeignKey(x => x.CavityId);
        }
    }
}
