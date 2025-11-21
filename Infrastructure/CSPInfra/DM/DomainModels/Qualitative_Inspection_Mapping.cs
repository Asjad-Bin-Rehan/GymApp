using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Qualitative_Inspection_Mapping : Base<string>
    {
        // FKs
        public string? QualitativeInspectionId { get; set; }
        public string? CharacteristicId { get; set; }

        // Columns
        public bool? IsMandatory { get; set; }
        public bool? IsQcCritical { get; set; }
        public bool? IsQcFloor { get; set; }
        public bool? IsQcLab { get; set; }
        public bool? IsDispatch { get; set; }
        public bool? IsIncoming { get; set; }
        public bool? IsTrial { get; set; }
        public string? QualitativeSpec { get; set; }

        // Navigation
        public Qualitative_Inspection? Qualitative_Inspection { get; set; }
        public Inspection_Characteristic? Inspection_Characteristic { get; set; }
        public ICollection<Qualitative_Result_Pass_Status> Qualitative_Result_Pass_Statuses { get; set; } = [];
        public ICollection<Production_QC_Sample_Result> Production_QC_Sample_Results { get; set; } = [];
        public ICollection<Purchase_QC_Sample_Result> Purchase_QC_Sample_Results { get; set; } = [];
        public ICollection<Production_QA_Cavity_Sample_Result> Production_QA_Cavity_Sample_Results { get; set; } = [];
    }
}
