namespace BS.Services.ItemInspectionCardService.DTOs
{
    public class UpdateItemInspectionCardDTO
    {
        public string Id { get; set; }
        public bool IsActive { get; set; } = true;
        public List<QualitativeInspectionUpdateObject> QualitativeInspectionResults { get; set; } = [];
        public List<QuantitativeInspectionUpdateObject> QuantitativeInspectionResults { get; set; } = [];
    }

    public class QualitativeInspectionUpdateObject
    {
        public string? Id { get; set; }             // CharacteristicId
        public string? QualitativeInspectionId { get; set; }
        public bool IsActive { get; set; } = true; // IF RECEIVED FALSE THEN USE IT TO SOFT DELETE QUALITATIVE INSPECTION OBJECT
        public bool IsMandatory { get; set; } = true;
        public bool? IsQcCritical { get; set; }
        public bool? IsQcFloor { get; set; }
        public bool? IsQcLab { get; set; }
        public bool? IsDispatch { get; set; }
        public bool? IsIncoming { get; set; }
        public bool? IsTrial { get; set; }
        public List<QualitativeResultPassStatusUpdateContract> QualitativeResultPassStatusResults { get; set; } = [];
        public List<QualitativeResultFailStatusUpdateContract> QualitativeResultFailStatusResults { get; set; } = [];
        public string? QualitativeSpec { get; set; }
    }

    public class QualitativeResultPassStatusUpdateContract
    {
        public string? QualitativeResultId { get; set; }
        public bool IsPassed { get; set; } = true;
        public bool IsActive { get; set; } = true;  // IF RECEIVED FALSE THEN USE IT TO SOFT DELETE PASS STATUS OBJECT
    }

    public class QualitativeResultFailStatusUpdateContract
    {
        public string? QualitativeResultId { get; set; }
        public bool IsPassed { get; set; } = true;
    }

    public class QuantitativeInspectionUpdateObject
    {
        public string? Id { get; set; }             // CharacteristicId
        public string? QuantitiveInspectionId { get; set; }
        public bool IsActive { get; set; } = true;  // IF RECEIVED FALSE THEN USE IT TO SOFT DELETE QUANTITATIVE INSPECTION OBJECT
        public bool IsMandatory { get; set; } = true;
        public bool? IsQcCritical { get; set; }
        public bool? IsQcFloor { get; set; }
        public bool? IsQcLab { get; set; }
        public bool? IsDispatch { get; set; }
        public bool? IsIncoming { get; set; }
        public bool? IsTrial { get; set; }
        public string? UoMId { get; set; }
        public double? Target { get; set; }
        public double? Max { get; set; }
        public double? Min { get; set; }
        public string? QuantitativeSpec { get; set; }
        public double? UpperLimit { get; set; }
        public double? LowerLimit { get; set; }
    }
}