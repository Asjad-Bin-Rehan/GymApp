using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Item_Sample : Base<string>
    {
        // Columns
        public int IntCode { get; set; }
        public string? ItemDescription { get; set; }
        public bool? Flexibility { get; set; }

        // FKs
        public string? ItemId { get; set; }

        // Navigation
        public Item? Item { get; set; }
        public ICollection<Sampling_Range> Sampling_Ranges { get; set; } = [];
    }
}
