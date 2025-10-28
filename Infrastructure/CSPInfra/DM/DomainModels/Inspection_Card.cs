using GenericRepository.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Inspection_Card : Base<string>
    {
        // Columns
        public int IntCode { get; set; }
        public string? Description { get; set; }

        // Navigation
        public ICollection<Inspection_Characteristic_Mapping> Inspection_Characteristic_Mappings { get; set; } = [];
        public ICollection<Item_Inspection_Card> Item_Inspection_Cards { get; set; } = [];
    }
}
