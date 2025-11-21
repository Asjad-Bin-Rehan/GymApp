using GenericRepository.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Item_Inspection_Card : Base<string>
    {
        // Columns
        public int IntCode { get; set; }
        public string? CardDescription { get; set; }
        public string? ItemDescription { get; set; }
        public string? Type { get; set; }

        // FKs
        public string? ItemId { get; set; }
        public string? InspectionCardId { get; set; }

        // Navigation
        public Inspection_Card? Inspection_Card { get; set; }
        public Item? Item { get; set; }
        public Qualitative_Inspection? Qualitative_Inspection { get; set; }
        public Quantitative_Inspection? Quantitative_Inspection { get; set; }
    }
}
