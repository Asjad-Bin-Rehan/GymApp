using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.CustomExceptions.Common
{
    public class RecordAlreadyExistException:Exception
    {
        public RecordAlreadyExistException(string message):base(message)
        {
                
        }
    }
}
