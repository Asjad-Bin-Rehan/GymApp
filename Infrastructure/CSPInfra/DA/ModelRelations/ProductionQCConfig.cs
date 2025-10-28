using DM.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.ModelRelations
{
    public class ProductionQCConfig : IEntityTypeConfiguration<Production_QC>
    {
        public void Configure(EntityTypeBuilder<Production_QC> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IntCode)
                   .ValueGeneratedOnAdd()
                   .HasAnnotation("DatabaseGenerated", "Identity");

            builder.HasOne(x => x.Item)
                    .WithMany(x => x.Production_QC)
                    .HasForeignKey(x => x.ItemId);

            builder.HasMany(x => x.Production_QC_Samples)
                   .WithOne(x => x.Production_QC)
                   .HasForeignKey(x => x.QcId);

            builder.HasMany(x => x.Log_Post_Saps)
                   .WithOne(x => x.Production_QC)
                   .HasForeignKey(x => x.QId);
        }
    }
}
