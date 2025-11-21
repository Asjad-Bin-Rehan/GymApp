using GenericRepository.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Unit_Of_Measure : Base<string>
    {
        // Columns
        public int IntCode { get; set; }
        public string? UoMcode { get; set; }
        public string? Description { get; set; }

        // Navigation
        public ICollection<Quantitative_Inspection_Mapping> Quantitative_Inspection_Mappings { get; set; } = [];
    }
}
