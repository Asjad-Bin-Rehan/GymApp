using System.Linq.Expressions;
using BS.Services.UserService.DTOs;
using DA;
using DM.DomainModels;
using Helpers.CommonModels;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.UserService;

public class UserService(IUnitOfWork uow) : IUserService
{
    IUnitOfWork _uow { get; set; } = uow;

    public async Task<UpsertUserResponse> UpsertUser(UpsertUserRequest req, string userId, CancellationToken ct)
    {
        var updateIds = req.Users.Where(x => !string.IsNullOrEmpty(x.Id)).Select(x => x.Id!).ToList();
        var existingUsers = await _uow.user.GetQueryable().Data.Where(x => updateIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, ct);

        List<string> ids = [];
        foreach (var obj in req.Users)
        {
            if (string.IsNullOrEmpty(obj.Id))
            {
                var user = obj.ToInsert(userId);
                await _uow.user.AddAsync(user, userId, ct);
                ids.Add(user.Id);
            }
            else if (existingUsers.TryGetValue(obj.Id, out var existingUser))
            {
                var user = existingUser.ToUpdate(obj);
                await _uow.user.UpdateAsync(user, userId, ct);
                ids.Add(user.Id);
            }
        }

        await _uow.CommitAsync();
        return new UpsertUserResponse { Ids = ids };
    }

    public async Task<PagedResponse<GetUserResponse>> GetUser(string? userId, string? organizationId, string? email, string? name, string? userType, int lastCount, int skipRecords, CancellationToken ct)
    {
        var queryable = _uow.user.GetQueryable();
        Expression<Func<User, bool>> filter = x =>
            (userId == null || x.Id == userId)
            && (organizationId == null || x.OrganizationId == organizationId)
            && (email == null || x.Email == email)
            && (name == null || x.Name != null && x.Name.Contains(name))
            && (userType == null || x.UserType == userType)
            && x.IsActive;

        var totalCount = await queryable.Data.AsNoTracking().CountAsync(filter, ct);

        var users = await queryable.Data.AsNoTracking()
            .Where(filter)
            .OrderByDescending(x => x.CreatedDate)
            .Skip(skipRecords)
            .Take(lastCount)
            .ToListAsync(ct);

        ArgumentFalseException.ThrowIfFalse(users.Any(), "No users found.");

        var data = users.Select(x => x.ToResponse()).ToList();
        return new PagedResponse<GetUserResponse> { TotalCount = totalCount, Data = data };
    }
}
