
using DA.Common.CommonRoles;
using DM.DomainModels;
using Helpers.StringsExtension;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DA.AppDbContexts
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Encrypted_Credentials> Encrypted_Credentials { get; set; }
        public DbSet<Inspection_Attribute> Inspection_Attribute { get; set; }
        public DbSet<Inspection_Characteristic> Inspection_Characteristic { get; set; }
        public DbSet<Inspection_Characteristic_Mapping> Inspection_Characteristic_Mapping { get; set; }
        public DbSet<Inspection_Card> Inspection_Card { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Item_Inspection_Card> Item_Inspection_Card { get; set; }
        public DbSet<Item_Sample> Item_Sample { get; set; }
        public DbSet<Log_Post_Sap> Log_Post_Sap { get; set; }
        public DbSet<Log_Sampling_Range> Log_Sampling_Range { get; set; }
        public DbSet<Log_Production_QA_Cavity> Log_Production_QA_Cavity { get; set; }
        public DbSet<Log_Production_QA_Cavity_Sample_Result> Log_Production_QA_Cavity_Sample_Result { get; set; }
        public DbSet<Log_Production_QC_Sample_Result> Log_Production_QC_Sample_Result { get; set; }
        public DbSet<Log_Purchase_QC_Sample_Result> Log_Purchase_QC_Sample_Result { get; set; }
        public DbSet<Qualitative_Inspection> Qualitative_Inspection { get; set; }
        public DbSet<Qualitative_Inspection_Mapping> Qualitative_Inspection_Mapping { get; set; }
        public DbSet<Qualitative_Result> Qualitative_Result { get; set; }
        public DbSet<Qualitative_Result_Pass_Status> Qualitative_Result_Pass_Status { get; set; }
        public DbSet<Quantitative_Inspection> Quantitative_Inspection { get; set; }
        public DbSet<Quantitative_Inspection_Mapping> Quantitative_Inspection_Mapping { get; set; }
        public DbSet<Sampling_Range> Sampling_Range { get; set; }
        public DbSet<Unit_Of_Measure> Unit_Of_Measure { get; set; }
        public DbSet<Production_QC> Production_QC { get; set; }
        public DbSet<Production_QC_Sample> Production_QC_Sample { get; set; }
        public DbSet<Production_QC_Sample_Result> Production_QC_Sample_Result { get; set; }
        public DbSet<Purchase_QC> Purchase_QC { get; set; }
        public DbSet<Purchase_QC_Sample> Purchase_QC_Sample { get; set; }
        public DbSet<Purchase_QC_Sample_Result> Purchase_QC_Sample_Result { get; set; }
        public DbSet<Production_QA> Production_QA { get; set; }
        public DbSet<Production_QA_Cavity> Production_QA_Cavity { get; set; }
        public DbSet<Production_QA_Cavity_Sample> Production_QA_Cavity_Sample { get; set; }
        public DbSet<Production_QA_Cavity_Sample_Result> Production_QA_Cavity_Sample_Result { get; set; }

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




