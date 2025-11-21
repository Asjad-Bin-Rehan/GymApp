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
    public class ProductionQaCavitySampleConfig : IEntityTypeConfiguration<Production_QA_Cavity_Sample>
    {
        public void Configure(EntityTypeBuilder<Production_QA_Cavity_Sample> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Production_QA_Cavity)
                   .WithMany(x => x.Production_QA_Cavity_Samples)
                   .HasForeignKey(x => x.CavityId);

            builder.HasMany(x => x.Production_QA_Cavity_Sample_Results)
                   .WithOne(x => x.Production_QA_Cavity_Sample)
                   .HasForeignKey(x => x.QaCavitySampleId);
        }
    }
}
