using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Log_Purchase_QC_Sample_Result : Base<string>
    {
        // FKs
        public string? QcSampleId { get; set; }
        public string? QualitativeInspectionMappingId { get; set; }
        public string? QuantitativeInspectionMappingId { get; set; }

        // Log-FK
        public string? QcSampleResultId { get; set; }

        // Columns
        public string? QualitativeResultId { get; set; }
        public bool? IsQualitativeResultPassed { get; set; }
        public double? QuantitativeResult { get; set; }
        public bool? IsQuantitativeResultPassed { get; set; }
        public string? Remarks { get; set; }
        public bool? IsSamplePassed { get; set; }

        // Navigation
        public Purchase_QC_Sample_Result? Purchase_QC_Sample_Result { get; set; }
    }
}
