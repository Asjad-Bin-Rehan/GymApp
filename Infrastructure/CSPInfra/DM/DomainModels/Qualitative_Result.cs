using GenericRepository.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Qualitative_Result : Base<string>
    {
        // Columns
        public int IntCode { get; set; }
        public string? ResultDescription { get; set; }

        // Navigation
        public ICollection<Qualitative_Inspection> Qualitative_Inspections { get; set; } = [];
        public ICollection<Qualitative_Result_Pass_Status> Qualitative_Result_Pass_Statuses { get; set; } = [];
        public ICollection<Purchase_QC_Sample_Result> Purchase_QC_Sample_Results { get; set; } = [];
        public ICollection<Production_QC_Sample_Result> Production_QC_Sample_Results { get; set; } = [];
        public ICollection<Production_QA_Cavity_Sample_Result> Production_QA_Cavity_Sample_Results { get; set; } = [];
    }
}
