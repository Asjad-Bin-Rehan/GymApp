using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.AdminService.DTOs
{
    public class AdminDTO
    {
        public int admin_id { get; set; }
        public string username { get; set; } = null!;
        public string role { get; set; } = null!;
    }
}
