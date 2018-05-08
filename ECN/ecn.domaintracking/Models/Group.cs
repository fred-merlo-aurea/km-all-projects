using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.domaintracking.Models
{
    public class Group 
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public int FolderID { get; set; }
        public string GroupDescription { get; set; }

        public int CustomerID { get; set; }
    }
}