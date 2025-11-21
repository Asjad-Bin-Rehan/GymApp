using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DA.AppDbContexts
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions) { }

        public override int SaveChanges()
        {
            //ConvertDateTimesToUtc();
            return base.SaveChanges();
        }

       /* private void ConvertDateTimesToUtc()
        {
            var entities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity);

            foreach (var entity in entities)
            {
                var properties = entity.GetType().GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));

                foreach (var property in properties)
                {
                    if (property.GetValue(entity) is DateTime dateTime)
                    {
                        property.SetValue(entity, dateTime.ToUniversalTime());
                    }

                }
            }
        }
*/
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
          //  ConvertDateTimesToUtc();
            return await base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            SeedSuperAdmin(builder);
            
        }
        private void SeedSuperAdmin(ModelBuilder builder)
        {
            

            
         


        }
    }
}




