using GenericRepository.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Inspection_Characteristic : Base<string>
    {
        // Columns
        public int IntCode { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? SingleCriteria { get; set; }
        public string? QuantitativeCriteria { get; set; }

        // FKs
        public string? AttributeId { get; set; }

        // Navigation
        public Inspection_Attribute? Inspection_Attribute { get; set; }
        public ICollection<Inspection_Characteristic_Mapping> Inspection_Characteristic_Mappings { get; set; } = [];
        public ICollection<Quantitative_Inspection_Mapping> Quantitative_Inspection_Mappings { get; set; } = [];
        public ICollection<Qualitative_Inspection_Mapping> Qualitative_Inspection_Mappings { get; set; } = [];
    }
}