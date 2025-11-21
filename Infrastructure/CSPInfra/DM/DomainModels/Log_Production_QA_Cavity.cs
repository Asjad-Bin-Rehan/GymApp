using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Log_Production_QA_Cavity : Base<string>
    {
        // Columns
        public int IntCode { get; set; }
        public string? Name { get; set; }
        public string? InspectionBy { get; set; }
        public DateTime? InspectionDateTime { get; set; }
        public string? MouldNo { get; set; }
        public bool? IsToggledOn { get; set; }
        public bool? IsCavityPassed { get; set; }

        // FK
        public string? QaId { get; set; }

        // Log-FK
        public string? CavityId { get; set; }

        // Navigation
        public Production_QA_Cavity? Production_QA_Cavity { get; set; }
    }
}
