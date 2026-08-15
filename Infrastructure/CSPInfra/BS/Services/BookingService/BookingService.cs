using System.Linq.Expressions;
using BS.Services.BookingService.DTOs;
using DA;
using DM.DomainModels;
using Helpers.CommonModels;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.BookingService;

public class BookingService(IUnitOfWork uow) : IBookingService
{
    IUnitOfWork _uow { get; set; } = uow;

    public async Task<UpsertBookingResponse> UpsertBooking(UpsertBookingRequest req, string userId, CancellationToken ct)
    {
        var updateIds = req.Bookings.Where(x => !string.IsNullOrEmpty(x.Id)).Select(x => x.Id!).ToList();
        var existingBookings = await _uow.booking.GetQueryable().Data.Where(x => updateIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, ct);

        List<string> ids = [];
        foreach (var obj in req.Bookings)
        {
            if (string.IsNullOrEmpty(obj.Id))
            {
                var booking = obj.ToInsert(userId);
                await _uow.booking.AddAsync(booking, userId, ct);
                ids.Add(booking.Id);
            }
            else if (existingBookings.TryGetValue(obj.Id, out var existingBooking))
            {
                var booking = existingBooking.ToUpdate(obj);
                await _uow.booking.UpdateAsync(booking, userId, ct);
                ids.Add(booking.Id);
            }
        }

        await _uow.CommitAsync();
        return new UpsertBookingResponse { Ids = ids };
    }

    public async Task<PagedResponse<GetBookingResponse>> GetBooking(string? bookingId, string? slotId, string? customerProfileId, string? status, string? paymentStatus, string? name, int lastCount, int skipRecords, CancellationToken ct)
    {
        var queryable = _uow.booking.GetQueryable();
        Expression<Func<Booking, bool>> filter = x =>
            (bookingId == null || x.Id == bookingId)
            && (slotId == null || x.SlotId == slotId)
            && (customerProfileId == null || x.CustomerProfileId == customerProfileId)
            && (status == null || x.Status == status)
            && (paymentStatus == null || x.PaymentStatus == paymentStatus)
            && (name == null || x.Name != null && x.Name.Contains(name))
            && x.IsActive;

        var totalCount = await queryable.Data.AsNoTracking().CountAsync(filter, ct);

        var bookings = await queryable.Data.AsNoTracking()
            .Where(filter)
            .OrderByDescending(x => x.CreatedDate)
            .Skip(skipRecords)
            .Take(lastCount)
            .ToListAsync(ct);

        ArgumentFalseException.ThrowIfFalse(bookings.Any(), "No bookings found.");

        var data = bookings.Select(x => x.ToResponse()).ToList();
        return new PagedResponse<GetBookingResponse> { TotalCount = totalCount, Data = data };
    }
}
