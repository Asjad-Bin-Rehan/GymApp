using GenericRepository.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Quantitative_Inspection : Base<string>
    {
        // Columns
        public int IntCode { get; set; }

        // FKs
        public string? ItemInspectionCardId { get; set; }

        // Navigation
        public Item_Inspection_Card? Item_Inspection_Card { get; set; }
        public ICollection<Quantitative_Inspection_Mapping> Quantitative_Inspection_Mappings { get; set; } = [];
    }
}
