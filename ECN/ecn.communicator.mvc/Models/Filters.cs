using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.mvc.Models
{
    public class Filters
    {
        public Filters(List<ECN_Framework_Entities.Communicator.Filter> filterList, int groupid, string groupName = "", bool readPermissions = false, bool writePermissions = false) 
        {
            FilterList = filterList;
            canRead = readPermissions;
            canWrite = writePermissions;
            GroupID = groupid;
            GroupName = groupName;
            ArchiveFilterTypes = new List<string> { "Active", "Archived", "All" };
            ArchiveFilter = "Active";
        }
        public List<ECN_Framework_Entities.Communicator.Filter> FilterList { get; set; }
        public bool canRead { get; set; }
        public bool canWrite { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public List<string> ArchiveFilterTypes { get; set; }
        public string ArchiveFilter { get; set; }
    }
}