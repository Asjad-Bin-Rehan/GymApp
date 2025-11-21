using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.ProductionQAService.DTOs
{
    public class ListProductionQAResponse
    {
        public int TotalRecords { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public int PageNumber { get; set; } = 0;
        public List<ResponseProductionQAWithItem> Values { get; set; } = [];
    }
}
