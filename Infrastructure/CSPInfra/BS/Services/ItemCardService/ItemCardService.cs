using BS.Services.ItemCardService.DTOs;
using DA;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.ItemCardService
{
    public class ItemCardService : IItemCardService
    {
        private IUnitOfWork _uow;
        public ItemCardService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<bool> AddItem(AddItemCardDTO request, string userId, CancellationToken ct)
        {
            var entity = request.ToDomain(userId);
            await _uow.item.AddAsync(entity, userId, ct);
            await _uow.CommitAsync();
            return true;
        }

        public async Task<List<ResponseItemCard>> ListAllItems(CancellationToken ct, int lastCount, int skipRecords)
        {
            var query = _uow.item.GetQueryable();
            var result = await query.Data
                                    .OrderByDescending(x => x.CreatedDate)
                                    .Skip(skipRecords)
                                    .Take(lastCount)
                                    .ToListAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
            var data = result.ToResponseList().ToList();
            return data;
        }

        public async Task<List<ResponseItemCard>> GetItemById(CancellationToken ct, string itemId)
        {
            var query = _uow.item.GetQueryable();
            var result = await query.Data
                                    .OrderByDescending(x => x.CreatedDate)
                                    .Where(x => x.Id == itemId)
                                    .ToListAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
            var data = result.ToResponseList().ToList();
            return data;
        }

        public async Task<List<ResponseItemCard>> GetItemByCode(CancellationToken ct, string itemCode, string? docNum, string? lineNum)
        {
            var query = _uow.item.GetQueryable();
            var result = await query.Data
                            .Where(x => x.ItemCode == itemCode &&
                                       (string.IsNullOrEmpty(docNum) || x.DocNum == docNum) &&
                                       (string.IsNullOrEmpty(lineNum) || x.LineNum == lineNum))
                            .OrderByDescending(x => x.CreatedDate)
                            .ToListAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
            var data = result.ToResponseList().ToList();
            return data;
        }

        public async Task<ResponseItemTypes> GetItemTypesById(string itemId, CancellationToken ct, int lastCount, int skipRecords)
        {
            var query = _uow.item.GetQueryable();

            var result = await query.Data
                .Where(x => x.Id == itemId && x.IsActive)
                .Include(x => x.Item_Inspection_Cards)
                    .ThenInclude(card => card.Qualitative_Inspection)
                        .ThenInclude(qi => qi.Qualitative_Inspection_Mappings)
                .Include(x => x.Item_Inspection_Cards)
                    .ThenInclude(card => card.Quantitative_Inspection)
                        .ThenInclude(qi => qi.Quantitative_Inspection_Mappings)
                .OrderByDescending(x => x.CreatedDate)
                .Skip(skipRecords)
                .Take(lastCount)
                .FirstOrDefaultAsync(ct);  // Single item

            ArgumentFalseException.ThrowIfFalse(result != null, "No records found");

            bool isItemDispatch = result.Item_Inspection_Cards.Any(card =>
                (card.Qualitative_Inspection?.Qualitative_Inspection_Mappings?.Any(m => m.IsDispatch == true) ?? false) ||
                (card.Quantitative_Inspection?.Quantitative_Inspection_Mappings?.Any(m => m.IsDispatch == true) ?? false)
            );

            bool isItemIncoming = result.Item_Inspection_Cards.Any(card =>
                (card.Qualitative_Inspection?.Qualitative_Inspection_Mappings?.Any(m => m.IsIncoming == true) ?? false) ||
                (card.Quantitative_Inspection?.Quantitative_Inspection_Mappings?.Any(m => m.IsIncoming == true) ?? false)
            );

            return new ResponseItemTypes
            {
                IsDispatch = isItemDispatch,
                IsIncoming = isItemIncoming
            };
        }



    }
}