using BS.Services.InspectionAttributeService.DTOs;
using DA;
using DA.AppDbContexts;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BS.Services.InspectionAttributeService
{
    public class InspectionAttributeService : IInspectionAttributeService
    {
        private readonly IUnitOfWork _uow;
        private AppDbContext _dbContext;
        public InspectionAttributeService(IUnitOfWork uow, AppDbContext dbContext)
        {
            _uow = uow;
            _dbContext = dbContext;
        }

        public async Task<List<ResponseInspectionAttribute>> ListAllInspectionAttributesRaw(CancellationToken ct)
        {
            var sqlQuery = @"
                            SELECT ""Id"", ""IntCode"", ""Name"", ""Description"", ""Tag"", ""CreatedBy"", ""UpdatedBy"", ""CreatedDate"", ""UpdatedDate"", ""IsActive"", ""IsArchived""
                            FROM public.""Inspection_Attribute""
                            WHERE ""IsActive"" = true
                            ORDER BY ""Name"";
                            ";

            var result = await _dbContext
                                 .Database
                                 .SqlQueryRaw<ResponseInspectionAttribute>(sqlQuery)
                                 .ToListAsync(ct);

            return result;
        }

        public async Task<bool> AddInspectionAttributeRaw(AddInspectionAttributeDTO request, string userId, CancellationToken ct)
        {
            var sqlQuery = @"
                            INSERT INTO public.""Inspection_Attribute"" 
                            (""Id"", ""Name"", ""Description"", ""Tag"", ""CreatedBy"", ""CreatedDate"", ""UpdatedBy"", ""UpdatedDate"", ""IsActive"", ""IsArchived"")
                            VALUES 
                            (@Id, @Name, @Description, @Tag, @CreatedBy, @CreatedDate, @UpdatedBy, @UpdatedDate, @IsActive, @IsArchived)
                            ";

            var parameters = new[]
            {
                new NpgsqlParameter("@Id", Guid.NewGuid().ToString() ?? (object)DBNull.Value),
                new NpgsqlParameter("@Name", request.Name ?? (object)DBNull.Value),
                new NpgsqlParameter("@Description", request.Description ?? (object)DBNull.Value),
                new NpgsqlParameter("@Tag", request.Tag ?? (object)DBNull.Value),
                new NpgsqlParameter("@CreatedBy", userId),
                new NpgsqlParameter("@CreatedDate", DateTime.UtcNow),
                new NpgsqlParameter("@UpdatedBy", userId),
                new NpgsqlParameter("@UpdatedDate", DateTime.UtcNow),
                new NpgsqlParameter("@IsActive", true),
                new NpgsqlParameter("@IsArchived", false)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery, parameters, ct);
            return true;
        }


        public async Task<bool> AddInspectionAttribute(AddInspectionAttributeDTO request, string userId, CancellationToken ct)
        {
            var entity = request.ToDomain(userId);
            await _uow.inspection_attribute.AddAsync(entity, userId, ct);
            await _uow.CommitAsync(ct);
            return true;
        }

        public async Task<List<ResponseInspectionAttribute>> ListAllInspectionAttributes(int lastCount, int skipRecords, CancellationToken ct)
        {
            var query = _uow.inspection_attribute.GetQueryable();
            var result = await query.Data
                                    .OrderByDescending(x => x.CreatedDate)
                                    .Skip(skipRecords)
                                    .Take(lastCount)
                                    .ToListAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result != null, "No records found");
            var data = result?.ToResponseList().ToList();
            return data ?? [];
        }

        public async Task<ResponseInspectionAttribute> GetInspectionAttributeById(string id, CancellationToken ct)
        {
            var query = _uow.inspection_attribute.GetQueryable();
            var result = await query.Data.FirstOrDefaultAsync(x => x.Id==id);
            ArgumentFalseException.ThrowIfFalse(result!=null, "No records found");
            var dto = result?.ToResponse();
            return dto ?? new ResponseInspectionAttribute();
        }

        public async Task<bool> UpdateInspectionAttribute(UpdateInspectionAttributeDTO request, string userId, CancellationToken ct)
        {
            var query = _uow.inspection_attribute.GetQueryable();

            var alreadyExists = await query.Data.AnyAsync(x => x.Id!=request.Id && x.Name!=null && request.Name!=null && x.Name.ToLower()==request.Name.ToLower());
            //ArgumentFalseException.ThrowIfFalse(!alreadyExists, "Details already exist for another record");

            var result = await query.Data.Where(x => x.Id == request.Id).SingleOrDefaultAsync(ct) ?? throw new ArgumentFalseException("No active record found to update");
            result.Name = request.Name;
            result.Description = request.Description;
            result.Tag = request.Tag;
            result.IsActive = request.IsActive;
            await _uow.inspection_attribute.UpdateAsync(result, userId, ct);
            await _uow.CommitAsync();
            return true;
        }

        #region Custom FluentValidations
        public async Task<bool> IsInspectionAttributeNameExists(string? name, CancellationToken ct)
        {
            return await _uow.inspection_attribute.GetQueryable().Data.Where(x => x.Name != null && name != null && x.Name.ToLower() == name.ToLower()).AnyAsync();
        }
        #endregion Custom FluentValidations
    }
}
