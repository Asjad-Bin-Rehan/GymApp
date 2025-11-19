using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.AdminService.DTOs
{
    public class LoginAdminDTO
    {
        public string username { get; set; } = null!;
        public string password { get; set; } = null!;
    }
}
