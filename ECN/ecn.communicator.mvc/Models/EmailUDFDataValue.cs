using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.mvc.Models
{
    public class EmailUDFDataValue
    {
        public int GroupDataFieldsID { get; set; }

        public string UDFName { get; set; }

        public string Data { get; set; }

        public int GroupID { get; set; }

        public int EmailID { get; set; }
    }
}