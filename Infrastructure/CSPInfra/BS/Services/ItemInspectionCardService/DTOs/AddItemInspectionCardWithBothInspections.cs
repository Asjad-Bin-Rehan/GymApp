using NATS.Client.JetStream;

namespace BS.Services.ItemInspectionCardService.DTOs
{
    public class AddItemInspectionCardWithBothInspectionsDTO
    {
        // Common Item properties
        public string? ItemName { get; set; }
        public string? ItemType { get; set; }
        public string? ItemGroupCode { get; set; }
        public string? ItemU_QACard { get; set; }
        public string? ItemUoMGroupEntry { get; set; }
        public string? ItemCode { get; set; }
        public string? GroupName { get; set; }
        public string? ManageBatchNumbers { get; set; }
        public bool? IsBatch { get; set; }
        public bool? IsEnabledForQA { get; set; }
        public double? PackSize { get; set; }

        // Inspection Card properties
        public string? InspectionCardId { get; set; }
        public string? ItemDescription { get; set; }
        public string? CardDescription { get; set; }

        // Qualitative Inspection Objects (reuse structure of Add Item Inspection with Quantitative)
        public List<QualitativeInspectionContract> QualitativeInspectionObjects { get; set; } = new();

        // Quantitative Inspection Objects (new structure)
        public List<QuantitativeInspectionContract> QuantitativeInspectionObjects { get; set; } = new();
    }

    public class QualitativeInspectionContract
    {
        public string? InspectionCharacteristicId { get; set; }
        public bool IsMandatory { get; set; } = false;
        public bool IsQcCritical { get; set; } = false;
        public bool IsQcFloor { get; set; } = false;
        public bool IsQcLab { get; set; } = false;
        public bool IsDispatch { get; set; } = false;
        public bool IsIncoming { get; set; } = false;
        public bool IsTrial { get; set; } = false;
        public List<QualitativeResultPassStatusContract> QualitativeResultPassStatusObjects { get; set; } = new();
        public string? QualitativeSpec { get; set; }
    }

    public class QualitativeResultPassStatusContract
    {
        public string? QualitativeResultId { get; set; }
        public bool IsPassed { get; set; } = true;
    }

    public class QuantitativeInspectionContract
    {
        public string? InspectionCharacteristicId { get; set; }
        public bool IsMandatory { get; set; } = false;
        public bool IsQcCritical { get; set; } = false;
        public bool IsQcFloor { get; set; } = false;
        public bool IsQcLab { get; set; } = false;
        public bool IsDispatch { get; set; } = false;
        public bool IsIncoming { get; set; } = false;
        public bool IsTrial { get; set; } = false;
        public string? UoMId { get; set; }
        public double? Target { get; set; }
        public double? Max { get; set; }
        public double? Min { get; set; }
        public string? QuantitativeSpec { get; set; }
        public double? UpperLimit { get; set; }
        public double? LowerLimit { get; set; }
    }
}

