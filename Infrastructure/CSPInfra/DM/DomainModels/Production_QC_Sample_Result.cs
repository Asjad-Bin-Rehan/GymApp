using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Production_QC_Sample_Result : Base<string>
    {
        // FKs
        public string? QcSampleId { get; set; }
        public string? QualitativeInspectionMappingId { get; set; }
        public string? QuantitativeInspectionMappingId { get; set; }

        // Columns
        public string? QualitativeResultId { get; set; }
        public bool? IsQualitativeResultPassed { get; set; }
        public double? QuantitativeResult { get; set; }
        public bool? IsQuantitativeResultPassed { get; set; }
        public string? Remarks { get; set; }

        // Navigation
        public Production_QC_Sample? Production_QC_Sample { get; set; }
        public Qualitative_Result? Qualitative_Result { get; set; }
        public ICollection<Log_Production_QC_Sample_Result> Log_Production_QC_Sample_Results { get; set; } = [];
        public Qualitative_Inspection_Mapping? Qualitative_Inspection_Mapping { get; set; }
        public Quantitative_Inspection_Mapping? Quantitative_Inspection_Mapping { get; set; }
    }
}
