using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers.CommonModels;

namespace BS.Services.ItemInspectionCardService.DTOs
{
    public class ResponseItemInspectionCard : ActivityTrackersInResponse
    {
        public string Id { get; set; } = string.Empty;
        public int IntCode { get; set; }
        public string? CardDescription { get; set; }
        public string? ItemDescription { get; set; }
        public string? ItemId { get; set; }
        public string? InspectionCardId { get; set; }
    }
}
