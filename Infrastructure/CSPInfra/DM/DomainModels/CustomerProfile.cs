using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class CustomerProfile : Base<string>
    {   
        // FKs
        public string? UserId { get; set; }

        // Nav
        public User? User { get; set; }
        public ICollection<Booking> Bookings { get; set; } = [];
    }
}