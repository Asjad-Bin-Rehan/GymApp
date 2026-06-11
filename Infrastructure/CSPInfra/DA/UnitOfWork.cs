using DA.AppDbContexts;
using DM.DomainModels;
using GenericRepository;

namespace DA
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly AppDbContext _db;
        public UnitOfWork(AppDbContext db) { _db = db; }

        public IGenericRepository<Encrypted_Credentials, string> encrypted_credentials => new GenericRepository<Encrypted_Credentials, string>(_db);
        public IGenericRepository<Location, string> location => new GenericRepository<Location, string>(_db);
        public IGenericRepository<Location_From_Location, string> location_from_location => new GenericRepository<Location_From_Location, string>(_db);
        public IGenericRepository<Booking, string> booking => new GenericRepository<Booking, string>(_db);
        public IGenericRepository<Court, string> court => new GenericRepository<Court, string>(_db);
        public IGenericRepository<CustomerProfile, string> customer_profile => new GenericRepository<CustomerProfile, string>(_db);
        public IGenericRepository<Organization, string> organization => new GenericRepository<Organization, string>(_db);
        public IGenericRepository<Pricing, string> pricing => new GenericRepository<Pricing, string>(_db);
        public IGenericRepository<Slot, string> slot => new GenericRepository<Slot, string>(_db);
        public IGenericRepository<Tenant, string> tenant => new GenericRepository<Tenant, string>(_db);
        public IGenericRepository<User, string> user => new GenericRepository<User, string>(_db);
        public IGenericRepository<Venue, string> venue => new GenericRepository<Venue, string>(_db);

        #region Commit
        public void Commit()
        {
            _db.SaveChanges();
        }
        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        public async Task CommitAsync()
        {
            await _db.SaveChangesAsync();
        }
        public virtual void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);

        }
        #endregion Commit
    }
}