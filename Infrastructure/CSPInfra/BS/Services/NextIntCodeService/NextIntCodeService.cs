using BS.EnumsAndConstants.Constant;
using BS.Services.NextIntCodeService.DTOs;
using DA;
using DM.DomainModels;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.NextIntCodeService
{
    public class NextIntCodeService : INextIntCodeService
    {
        IUnitOfWork _uow;
        public NextIntCodeService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ResponseGetQualityStatus> GetQualityStatus(string entityName, string ItemCode, string? docNumber, int? lineNo, string? stageType, CancellationToken ct)
        {
            var item = await _uow.item.GetQueryable().Data.FirstOrDefaultAsync(x => x.ItemCode == ItemCode);

            if (entityName == KConstantEntity.purchase_qc)
            {
                var entity = await _uow.purchase_qc.GetQueryable().Data
                    .OrderByDescending(x => x.CreatedDate)
                    .FirstOrDefaultAsync(x => item != null && x.ItemId == item.Id && (docNumber == null || x.DocNo == docNumber) && (lineNo == null || x.LineNo == lineNo) && (stageType==null || x.StageType==stageType));

                return new ResponseGetQualityStatus()
                {
                    IsClosed = entity?.IsClosed,
                    IsPerformed = entity?.IsPerformed,
                    IsPostedToSap = entity?.IsPostedToSap,
                    OverallStatus = entity?.OverallStatus,
                };
            }

            else if (entityName == KConstantEntity.production_qc)
            {
                var entity = await _uow.production_qc.GetQueryable().Data
                    .OrderByDescending(x => x.CreatedDate)
                    .FirstOrDefaultAsync(x => item != null && x.ItemId == item.Id && (docNumber == null || x.DocNo == docNumber) && (stageType == null || x.StageType == stageType));

                return new ResponseGetQualityStatus()
                {
                    IsClosed = entity?.IsClosed,
                    IsPerformed = entity?.IsPerformed,
                    IsPostedToSap = entity?.IsPostedToSap,
                    OverallStatus = entity?.OverallStatus,
                };
            }

            else if (entityName == KConstantEntity.production_qa)
            {
                var entity = await _uow.production_qa.GetQueryable().Data
                    .OrderByDescending(x => x.CreatedDate)
                    .FirstOrDefaultAsync(x => item != null && x.ItemId == item.Id && (docNumber == null || x.DocNo == docNumber) && (stageType == null || x.StageType == stageType));

                return new ResponseGetQualityStatus()
                {
                    IsClosed = entity?.IsClosed,
                    IsPerformed = entity?.IsPerformed,
                    IsPostedToSap = entity?.IsPostedToSap,
                    OverallStatus = entity?.OverallStatus,
                };
            }

            return new ResponseGetQualityStatus();
        }

        public async Task<bool> AddLogPostSap(AddLogPostSapDTO request, string userId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request.QType)) throw new ArgumentFalseException("Entity type (QType) must be provided");
            var qType = request.QType.ToLower();

            var entityCheckers = new Dictionary<string, Func<Task<bool>>>
            {
                { KConstantEntity.purchase_qc, () => _uow.purchase_qc.GetQueryable().Data.AnyAsync(x => x.Id == request.QId, ct) },
                { KConstantEntity.production_qc, () => _uow.production_qc.GetQueryable().Data.AnyAsync(x => x.Id == request.QId, ct) },
                { KConstantEntity.production_qa, () => _uow.production_qa.GetQueryable().Data.AnyAsync(x => x.Id == request.QId, ct) },
            };

            if (!entityCheckers.TryGetValue(qType, out var checkExists)) throw new ArgumentFalseException("Invalid entity name provided");
            bool exists = await checkExists();
            ArgumentFalseException.ThrowIfFalse(exists, "No records found");

            var logPostSap = new Log_Post_Sap
            {
                QType = request.QType,
                QCode = request.QCode,
                QId = request.QId,
                IsClosed = request.IsClosed,
                IsPostedToSap = request.IsPostedToSap,
                IsPerformed = request.IsPerformed,
                OverallStatus = request.OverallStatus,
                AnalyzedBy = request.AnalyzedBy,
                BMR = request.BMR,
                Remarks = request.Remarks,
                ReportRemarks = request.ReportRemarks,
                ReportReviewDate = request.ReportReviewDate,
                ReportNextReviewDate = request.ReportNextReviewDate,
                SamplesPassedCount = request.SamplesPassedCount,
                InspectionDateTime = request.InspectionDateTime,
                InspectionQuantity = request.InspectionQuantity,
                SampleQuantity = request.SampleQuantity,

                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true,
                IsArchived = false
            };
            await _uow.log_post_sap.AddAsync(logPostSap, userId, ct);
            await _uow.CommitAsync();
            return true;
        }

        public async Task<List<string>> ListAllEntityNamesAsync()
        {
            var entityNames = new List<string>
            {
                KConstantEntity.inspection_attribute,
                KConstantEntity.inspection_characteristic,
                KConstantEntity.inspection_characteristic_mapping,
                KConstantEntity.inspection_card,
                KConstantEntity.item,
                KConstantEntity.item_inspection_card,
                KConstantEntity.item_sample,
                KConstantEntity.qualitative_inspection,
                KConstantEntity.qualitative_inspection_mapping,
                KConstantEntity.qualitative_result,
                KConstantEntity.qualitative_result_pass_status,
                KConstantEntity.quantitative_inspection,
                KConstantEntity.quantitative_inspection_mapping,
                KConstantEntity.sampling_range,
                KConstantEntity.purchase_qc,
                KConstantEntity.purchase_qc_sample,
                KConstantEntity.production_qc,
                KConstantEntity.production_qc_sample,
                KConstantEntity.production_qa,
                KConstantEntity.production_qa_cavity,
                KConstantEntity.production_qa_cavity_sample
            };
            return await Task.FromResult(entityNames);
        }

        public async Task<int> GetNextIntCount(string entityName, CancellationToken ct)
        {
            int lastRecordIntCode;
            switch (entityName.ToLower())
            {
                case KConstantEntity.inspection_attribute:
                    {
                        var query = _uow.inspection_attribute.GetQueryable();
                        var lastRecord = await query.Data.OrderByDescending(x => x.IntCode).FirstOrDefaultAsync(ct);
                        lastRecordIntCode = lastRecord != null ? lastRecord.IntCode : 0;
                        break;
                    }

                case KConstantEntity.inspection_characteristic:
                    {
                        var query = _uow.inspection_characteristic.GetQueryable();
                        var lastRecord = await query.Data.OrderByDescending(x => x.IntCode).FirstOrDefaultAsync(ct);
                        lastRecordIntCode = lastRecord != null ? lastRecord.IntCode : 0;
                        break;
                    }

                case KConstantEntity.inspection_card:
                    {
                        var query = _uow.inspection_card.GetQueryable();
                        var lastRecord = await query.Data.OrderByDescending(x => x.IntCode).FirstOrDefaultAsync(ct);
                        lastRecordIntCode = lastRecord != null ? lastRecord.IntCode : 0;
                        break;
                    }

                case KConstantEntity.item:
                    {
                        var query = _uow.item.GetQueryable();
                        var lastRecord = await query.Data.OrderByDescending(x => x.IntCode).FirstOrDefaultAsync(ct);
                        lastRecordIntCode = lastRecord != null ? lastRecord.IntCode : 0;
                        break;
                    }

                case KConstantEntity.item_inspection_card:
                    {
                        var query = _uow.item_inspection_card.GetQueryable();
                        var lastRecord = await query.Data.OrderByDescending(x => x.IntCode).FirstOrDefaultAsync(ct);
                        lastRecordIntCode = lastRecord != null ? lastRecord.IntCode : 0;
                        break;
                    }

                case KConstantEntity.item_sample:
                    {
                        var query = _uow.item_sample.GetQueryable();
                        var lastRecord = await query.Data.OrderByDescending(x => x.IntCode).FirstOrDefaultAsync(ct);
                        lastRecordIntCode = lastRecord != null ? lastRecord.IntCode : 0;
                        break;
                    }

                case KConstantEntity.qualitative_inspection:
                    {
                        var query = _uow.qualitative_inspection.GetQueryable();
                        var lastRecord = await query.Data.OrderByDescending(x => x.IntCode).FirstOrDefaultAsync(ct);
                        lastRecordIntCode = lastRecord != null ? lastRecord.IntCode : 0;
                        break;
                    }

                case KConstantEntity.qualitative_result:
                    {
                        var query = _uow.qualitative_result.GetQueryable();
                        var lastRecord = await query.Data.OrderByDescending(x => x.IntCode).FirstOrDefaultAsync(ct);
                        lastRecordIntCode = lastRecord != null ? lastRecord.IntCode : 0;
                        break;
                    }

                case KConstantEntity.qualitative_result_pass_status:
                    {
                        var query = _uow.qualitative_result_pass_status.GetQueryable();
                        var lastRecord = await query.Data.OrderByDescending(x => x.IntCode).FirstOrDefaultAsync(ct);
                        lastRecordIntCode = lastRecord != null ? lastRecord.IntCode : 0;
                        break;
                    }

                case KConstantEntity.quantitative_inspection:
                    {
                        var query = _uow.quantitative_inspection.GetQueryable();
                        var lastRecord = await query.Data.OrderByDescending(x => x.IntCode).FirstOrDefaultAsync(ct);
                        lastRecordIntCode = lastRecord != null ? lastRecord.IntCode : 0;
                        break;
                    }

                case KConstantEntity.sampling_range:
                    {
                        var query = _uow.sampling_range.GetQueryable();
                        var lastRecord = await query.Data.OrderByDescending(x => x.IntCode).FirstOrDefaultAsync(ct);
                        lastRecordIntCode = lastRecord != null ? lastRecord.IntCode : 0;
                        break;
                    }

                case KConstantEntity.unit_of_measure:
                    {
                        var query = _uow.unit_of_measure.GetQueryable();
                        var lastRecord = await query.Data.OrderByDescending(x => x.IntCode).FirstOrDefaultAsync(ct);
                        lastRecordIntCode = lastRecord != null ? lastRecord.IntCode : 0;
                        break;
                    }

                case KConstantEntity.purchase_qc:
                    {
                        var query = _uow.purchase_qc.GetQueryable();
                        var lastRecord = await query.Data.OrderByDescending(x => x.IntCode).FirstOrDefaultAsync(ct);
                        lastRecordIntCode = lastRecord != null ? lastRecord.IntCode : 0;
                        break;
                    }

                case KConstantEntity.purchase_qc_sample:
                    {
                        var query = _uow.purchase_qc_sample.GetQueryable();
                        var lastRecord = await query.Data.OrderByDescending(x => x.IntCode).FirstOrDefaultAsync(ct);
                        lastRecordIntCode = lastRecord != null ? lastRecord.IntCode : 0;
                        break;
                    }

                case KConstantEntity.production_qc:
                    {
                        var query = _uow.production_qc.GetQueryable();
                        var lastRecord = await query.Data.OrderByDescending(x => x.IntCode).FirstOrDefaultAsync(ct);
                        lastRecordIntCode = lastRecord != null ? lastRecord.IntCode : 0;
                        break;
                    }

                case KConstantEntity.production_qc_sample:
                    {
                        var query = _uow.production_qc_sample.GetQueryable();
                        var lastRecord = await query.Data.OrderByDescending(x => x.IntCode).FirstOrDefaultAsync(ct);
                        lastRecordIntCode = lastRecord != null ? lastRecord.IntCode : 0;
                        break;
                    }

                case KConstantEntity.production_qa:
                    {
                        var query = _uow.production_qa.GetQueryable();
                        var lastRecord = await query.Data.OrderByDescending(x => x.IntCode).FirstOrDefaultAsync(ct);
                        lastRecordIntCode = lastRecord != null ? lastRecord.IntCode : 0;
                        break;
                    }

                case KConstantEntity.production_qa_cavity:
                    {
                        var query = _uow.production_qa_cavity.GetQueryable();
                        var lastRecord = await query.Data.OrderByDescending(x => x.IntCode).FirstOrDefaultAsync(ct);
                        lastRecordIntCode = lastRecord != null ? lastRecord.IntCode : 0;
                        break;
                    }

                case KConstantEntity.production_qa_cavity_sample:
                    {
                        var query = _uow.production_qa_cavity_sample.GetQueryable();
                        var lastRecord = await query.Data.OrderByDescending(x => x.IntCode).FirstOrDefaultAsync(ct);
                        lastRecordIntCode = lastRecord != null ? lastRecord.IntCode : 0;
                        break;
                    }

                default:
                    throw new ArgumentFalseException("Invalid entity name provided.");
            }
            return lastRecordIntCode + 1;
        }

        public async Task<int> GetTotalRecordsCount(string entityName, string? filterParameter, CancellationToken ct)
        {
            int count;
            switch (entityName.ToLower())
            {
                case KConstantEntity.qualitative_result:
                    var qualitativeResultQuery = _uow.qualitative_result.GetQueryable();
                    if (filterParameter == "true") count = await qualitativeResultQuery.Data.CountAsync(x => x.IsActive);
                    else  count = await qualitativeResultQuery.Data.CountAsync();
                    break;

                case KConstantEntity.unit_of_measure:
                    var unitOfMeasureQuery = _uow.unit_of_measure.GetQueryable();
                    if (filterParameter == "true")  count = await unitOfMeasureQuery.Data.CountAsync(x => x.IsActive);
                    else count = await unitOfMeasureQuery.Data.CountAsync();
                    break;

                case KConstantEntity.inspection_attribute:
                    var inspectionAttributeQuery = _uow.inspection_attribute.GetQueryable();
                    if (filterParameter == "true") count = await inspectionAttributeQuery.Data.CountAsync(x => x.IsActive);
                    else count = await inspectionAttributeQuery.Data.CountAsync();
                    break;

                case KConstantEntity.inspection_characteristic:
                    var inspectionCharacteristicQuery = _uow.inspection_characteristic.GetQueryable();
                    if (filterParameter == null) count = await inspectionCharacteristicQuery.Data.CountAsync();
                    else if (filterParameter == "true") count = await inspectionCharacteristicQuery.Data.CountAsync(x => x.IsActive);
                    else count = await inspectionCharacteristicQuery.Data.Include(x => x.Inspection_Characteristic_Mappings.Where(mapping => mapping.IsActive)).ThenInclude(x => x.Inspection_Characteristic).Where(x => x.Id == filterParameter && x.Inspection_Characteristic_Mappings.Any(mapping => mapping.IsActive)).CountAsync(ct);
                    break;

                case KConstantEntity.inspection_card:
                    var inspectionCardQuery = _uow.inspection_card.GetQueryable();
                    if (filterParameter == "true") count = await inspectionCardQuery.Data.CountAsync(x => x.IsActive);
                    else count = await inspectionCardQuery.Data.CountAsync();
                    break;

                case KConstantEntity.item:
                    var itemQuery = _uow.item.GetQueryable();
                    count = await itemQuery.Data.CountAsync(x => x.IsActive);
                    break;

                case KConstantEntity.item_inspection_card:
                    var itemInspectionCardQuery = _uow.item_inspection_card.GetQueryable();
                    count = await itemInspectionCardQuery.Data.CountAsync();
                    break;

                case KConstantEntity.item_sample:
                    var itemSampleQuery = _uow.item_sample.GetQueryable();
                    count = await itemSampleQuery.Data.CountAsync(x => x.IsActive);
                    break;

                case KConstantEntity.sampling_range:
                    var samplingRangeQuery = _uow.sampling_range.GetQueryable();
                    if (filterParameter == null) count = await samplingRangeQuery.Data.CountAsync(x => x.IsActive);
                    else if (filterParameter == "false") count = await samplingRangeQuery.Data.CountAsync(x => !x.IsActive);
                    else count = await samplingRangeQuery.Data.CountAsync(x => x.IsActive && x.ItemSampleId == filterParameter);
                    break;

                case KConstantEntity.purchase_qc:
                    var purchaseQCQuery = _uow.purchase_qc.GetQueryable();
                    count = await purchaseQCQuery.Data.CountAsync(x => x.IsActive);
                    break;

                case KConstantEntity.purchase_qc_sample:
                    var purchaseQCSampleQuery = _uow.purchase_qc_sample.GetQueryable();
                    if (filterParameter == null) count = await purchaseQCSampleQuery.Data.CountAsync(x => x.IsActive);
                    else if (filterParameter == "false") count = await purchaseQCSampleQuery.Data.CountAsync(x => !x.IsActive);
                    else count = await purchaseQCSampleQuery.Data.CountAsync(x => x.IsActive && x.QcId == filterParameter);
                    break;

                case KConstantEntity.production_qc:
                    var productionQCQuery = _uow.production_qc.GetQueryable();
                    count = await productionQCQuery.Data.CountAsync(x => x.IsActive);
                    break;

                case KConstantEntity.production_qc_sample:
                    var productionQCSampleQuery = _uow.production_qc_sample.GetQueryable(); 
                    if (filterParameter == null) count = await productionQCSampleQuery.Data.CountAsync(x => x.IsActive);
                    else if (filterParameter == "false") count = await productionQCSampleQuery.Data.CountAsync(x => !x.IsActive);
                    else count = await productionQCSampleQuery.Data.CountAsync(x => x.IsActive && x.QcId == filterParameter);
                    break;

                case KConstantEntity.production_qa:
                    var productionQAQuery = _uow.production_qa.GetQueryable();
                    count = await productionQAQuery.Data.CountAsync(x => x.IsActive);
                    break;

                case KConstantEntity.production_qa_cavity:
                    var productionQACavityQuery = _uow.production_qa_cavity.GetQueryable();
                    if (filterParameter == null) count = await productionQACavityQuery.Data.CountAsync(x => x.IsActive);
                    else if (filterParameter == "false") count = await productionQACavityQuery.Data.CountAsync(x => !x.IsActive);
                    else count = await productionQACavityQuery.Data.CountAsync(x => x.IsActive && x.QaId == filterParameter);
                    break;

                case KConstantEntity.production_qa_cavity_sample:
                    var productionQACavitySampleQuery = _uow.production_qa_cavity_sample.GetQueryable();
                    if (filterParameter == null) count = await productionQACavitySampleQuery.Data.CountAsync(x => x.IsActive);
                    else if (filterParameter == "false") count = await productionQACavitySampleQuery.Data.CountAsync(x => !x.IsActive);
                    else count = await productionQACavitySampleQuery.Data.CountAsync(x => x.IsActive && x.CavityId == filterParameter);
                    break;

                default:
                    throw new ArgumentFalseException("Invalid entity name provided.");
            }
            return count;
        }

        public async Task<bool> AddTestRecords(AddTestRecordsDTO request, string userId, CancellationToken ct)
        {
            return true;
        }
    }
}