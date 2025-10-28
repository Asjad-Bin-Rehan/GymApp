using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.ProductionQAService.DTOs
{
    public class ResponseProductionQaReportCavityWise
    {
        public string? CavityName { get; set; } = string.Empty;
        public string? DocNo { get; set; } = string.Empty;
        public DateTime? InspectionDateTime { get; set; }
        public string? ItemCode { get; set; } = string.Empty;
        public bool? AreCavitiesPassed { get; set; }
        public bool? AreSamplesPassed { get; set; }
        public List<SampleObject> SampleObjects { get; set; } = [];
    }

    public class SampleObject
    {
        public int IntCode { get; set; }
        public string? Name { get; set; }
        public DateTime? InspectionDateTime { get; set; }
        public string? Type { get; set; }
        public bool? Result { get; set; }
        public string? Characteristic { get; set; }
        public double? QuantitativeResult { get; set; }
    }
}
