using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.domaintracking.Models
{
    public class Folder 
    {
        public int FolderID { get; set; }
        public string FolderName { get; set; }
        public int ParentID { get; set; }
        public string FolderDescription { get; set; }
        public string FolderType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedUserID { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdatedUserID { get; set; }
        public string CustomerName { get; set; }
        public bool hasChildren { get { return true; } }

        public int CustomerID { get; set; }
    }
}