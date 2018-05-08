using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.mvc.Models
{
    public class EditUDFData
    {

        public List<ECN_Framework_Entities.Communicator.GroupDataFields> GroupDataFields { get; set; }

        public int EmailID { get; set; }

        public int GroupID { get; set; }

    }
}