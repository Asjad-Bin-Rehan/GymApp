using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Item : Base<string>
    {
        // Columns
        public int IntCode { get; set; }
        public string? ItemCode { get; set; }
        public string? DocNum { get; set; }
        public string? LineNum { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? GroupCode { get; set; }
        public string? U_QACard { get; set; }
        public string? UoMGroupEntry { get; set; }
        public bool? IsEnabledForQA { get; set; }
        public string? GroupName { get; set; }
        public string? ManageBatchNumbers { get; set; }
        public bool? IsBatch { get; set; }
        public double? PackSize { get; set; }

        // Navigation
        public Item_Sample? Item_Sample { get; set; }
        public ICollection<Item_Inspection_Card> Item_Inspection_Cards { get; set; } = [];
        public ICollection<Purchase_QC> Purchase_QC { get; set; } = [];
        public ICollection<Production_QC> Production_QC { get; set; } = [];
        public ICollection<Production_QA> Production_QA { get; set; } = [];
    }
}
