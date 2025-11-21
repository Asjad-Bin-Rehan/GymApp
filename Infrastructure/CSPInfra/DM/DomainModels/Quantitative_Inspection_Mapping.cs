using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Quantitative_Inspection_Mapping : Base<string>
    {
        // FKs
        public string? QuantitativeInspectionId { get; set; }
        public string? CharacteristicId { get; set; }
        public string? UoMId { get; set; }

        // Columns
        public bool? IsMandatory { get; set; }
        public double? Target { get; set; }
        public double? Max { get; set; }
        public double? Min { get; set; }
        public double? UpperLimit { get; set; }
        public double? LowerLimit { get; set; }
        public bool? IsQcCritical { get; set; }
        public bool? IsQcFloor { get; set; }
        public bool? IsQcLab { get; set; }
        public bool? IsDispatch { get; set; }
        public bool? IsIncoming { get; set; }
        public bool? IsTrial { get; set; }
        public string? QuantitativeSpec { get; set; }

        // Navigation
        public Quantitative_Inspection? Quantitative_Inspection { get; set; }
        public Inspection_Characteristic? Inspection_Characteristic { get; set; }
        public Unit_Of_Measure? Unit_Of_Measure { get; set; }
        public ICollection<Production_QC_Sample_Result> Production_QC_Sample_Results { get; set; } = [];
        public ICollection<Purchase_QC_Sample_Result> Purchase_QC_Sample_Results { get; set; } = [];
        public ICollection<Production_QA_Cavity_Sample_Result> Production_QA_Cavity_Sample_Results { get; set; } = [];
    }
}
