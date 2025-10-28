using DM.DomainModels;

namespace BS.Services.ItemCardService.DTOs
{
    public static class CRTItemCard
    {
        #region Add Item Card
        public static Item ToDomain(this AddItemCardDTO request, string userId)
        {
            return new Item()
            {
                ItemCode = request.ItemCode,
                Name = request.Name,
                Type = request.Type,
                GroupCode =  request.GroupCode,
                U_QACard = request.U_QACard,
                UoMGroupEntry = request.UoMGroupEntry,
                IsEnabledForQA = request.IsEnabledForQA,
                IsBatch = request.IsBatch,
                GroupName = request.GroupName,
                ManageBatchNumbers = request.ManageBatchNumbers,
                PackSize = request.PackSize,
                
                Id = Guid.NewGuid().ToString(),
                IsActive = request.IsActive,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false
            };
        }
        #endregion Add Item Card

        #region List All Item Cards
        public static ResponseItemCard ToResponse(this Item row)
        {
            return new ResponseItemCard()
            {
                ItemCode = row.ItemCode,
                IntCode = row.IntCode,
                Name = row.Name,
                Type = row.Type,
                GroupCode = row.GroupCode,
                U_QACard = row.U_QACard,
                UoMGroupEntry = row.UoMGroupEntry,
                IsEnabledForQA = row.IsEnabledForQA,
                IsBatch = row.IsBatch,
                GroupName = row.GroupName,
                ManageBatchNumbers = row.ManageBatchNumbers,
                PackSize = row.PackSize,

                Id = row.Id,
                IsActive = row.IsActive,
                CreatedDate = row.CreatedDate,
                CreatedBy = row.CreatedBy,
                UpdatedBy = row.UpdatedBy,
                UpdatedDate = row.UpdatedDate,
                IsArchived = row.IsArchived
            };
        }
        public static List<ResponseItemCard> ToResponseList(this IEnumerable<Item> rows)
        {
            return rows.Select(x => x.ToResponse()).ToList();
        }
        #endregion List All Item Cards
    }
}