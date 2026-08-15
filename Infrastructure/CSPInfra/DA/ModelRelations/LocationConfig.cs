using DM.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DA.ModelRelations
{
    public class LocationConfig : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasIndex(x => x.Id);
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.CreatedDate);
            builder.HasMany(x => x.LocationChildren).WithOne(x => x.LocationParent).HasForeignKey(x => x.ParentId);
            builder.HasMany(x => x.Location_From_Location).WithOne(x => x.Location).HasForeignKey(x => x.LocationId);
            builder.HasMany(x => x.Other_Location_From_Location).WithOne(x => x.OtherLocation).HasForeignKey(x => x.OtherLocationId);
        }
    }

    public class LocationFromLocationConfig : IEntityTypeConfiguration<Location_From_Location>
    {
        public void Configure(EntityTypeBuilder<Location_From_Location> builder)
        {
            builder.HasIndex(x => x.Id);
            builder.HasIndex(x => x.LocationId);
            builder.HasIndex(x => x.Distance);
            // NearbyRank low cardinality avoid
        }
    }
}