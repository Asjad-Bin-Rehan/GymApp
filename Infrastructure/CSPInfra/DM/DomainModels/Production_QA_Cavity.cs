using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Production_QA_Cavity : Base<string>
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

        // Navigation Propertities
        public Production_QA? Production_QA { get; set; }
        public ICollection<Log_Production_QA_Cavity> Log_Production_QA_Cavities { get; set; } = [];
        public ICollection<Production_QA_Cavity_Sample> Production_QA_Cavity_Samples { get; set; } = [];
    }
}
