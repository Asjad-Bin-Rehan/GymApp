using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.CustomExceptions.Common
{
    public class BootstrapDataMissingException:Exception
    {
        public BootstrapDataMissingException(string message):base(message) 
        {
            
        }
    }
}
