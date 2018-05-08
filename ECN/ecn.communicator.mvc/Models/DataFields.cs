using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.mvc.Models
{
    public class DataFields
    {
        public DataFields(List<DataFieldGridList> groupDataFieldsList, int groupid, string groupName = "", bool readPermissions = false, bool writePermissions = false) 
        {
            GroupDataFieldsList = groupDataFieldsList;
            canRead = readPermissions;
            canWrite = writePermissions;
            GroupID = groupid;
            GroupName = groupName;
        }
        public List<DataFieldGridList> GroupDataFieldsList { get; set; }
        public bool canRead { get; set; }
        public bool canWrite { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
    }
}