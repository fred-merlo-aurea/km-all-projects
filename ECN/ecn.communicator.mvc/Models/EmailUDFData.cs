using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.mvc.Models
{
    public class EmailUDFData
    {
        public EmailUDFData()
        {
            eudfdv = new List<EmailUDFDataValue>();
            datafieldSets = new List<ECN_Framework_Entities.Communicator.DataFieldSets>();
        }
        public int EmailID { get; set; }

        public string Email { get; set; }

        public int GroupID { get; set; }

        public List<EmailUDFDataValue> eudfdv { get; set; }

        public List<ECN_Framework_Entities.Communicator.DataFieldSets> datafieldSets { get; set; }
    }
}