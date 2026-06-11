using DM.DomainModels;
using GenericRepository;

namespace DA
{
    public interface IUnitOfWork
    {
        IGenericRepository<Encrypted_Credentials, string> encrypted_credentials { get; }
        IGenericRepository<Location, string> location { get; }
        IGenericRepository<Location_From_Location, string> location_from_location { get; }
        IGenericRepository<Booking, string> booking { get; }
        IGenericRepository<Court, string> court { get; }
        IGenericRepository<CustomerProfile, string> customer_profile { get; }
        IGenericRepository<Organization, string> organization { get; }
        IGenericRepository<Pricing, string> pricing { get; }
        IGenericRepository<Slot, string> slot { get; }
        IGenericRepository<Tenant, string> tenant { get; }
        IGenericRepository<User, string> user { get; }
        IGenericRepository<Venue, string> venue { get; }

        void Commit();
        Task CommitAsync(CancellationToken cancellationToken);
        Task CommitAsync();
    }
}
