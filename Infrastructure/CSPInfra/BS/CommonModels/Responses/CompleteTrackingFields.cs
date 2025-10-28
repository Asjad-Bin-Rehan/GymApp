using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.CommonModels.Responses
{
    public class PartialTrackingFields
    {
        


 
        public  string CreatedBy { get; set; }


      
        public  string UpdatedBy { get; set; }


   
        public  DateTime CreatedDate { get; set; }


       
        public  DateTime UpdatedDate { get; set; }
      
       

    }
    public  class CompleteTrackingFields:PartialTrackingFields
    {
        public bool IsActive { get; set; }

        public bool IsArchived { get; set; }
    }
}
