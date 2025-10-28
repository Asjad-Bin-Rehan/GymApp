using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Qualitative_Result_Pass_Status : Base<string>
    {
        // FKs
        public int IntCode { get; set; }
        public string? QualitativeInspectionMappingId { get; set; }
        public string? QualitativeResultId { get; set; }
        public bool? IsPassed { get; set; }

        // Navigation
        public Qualitative_Inspection_Mapping? Qualitative_Inspection_Mapping { get; set; }
        public Qualitative_Result? Qualitative_Result { get; set; }
    }
}
