using GenericRepository.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Inspection_Characteristic_Mapping : Base<string>
    {
        // FKs
        public string? InspectionCardId { get; set; }
        public string? CharacteristicId { get; set; }

        // Navigation
        public Inspection_Card? Inspection_Card { get; set; }
        public Inspection_Characteristic? Inspection_Characteristic { get; set; }
    }
}
