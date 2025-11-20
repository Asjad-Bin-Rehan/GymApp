using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.PartnerGymService.DTOs
{
    public class ActiveGymDTO
    {
        public int gym_id { get; set; }
        public string gym_name { get; set; }
        public int? location_id { get; set; }
        public string contact_person { get; set; }
        public string phone { get; set; }
        public DateTime partnership_date { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
    }

}
