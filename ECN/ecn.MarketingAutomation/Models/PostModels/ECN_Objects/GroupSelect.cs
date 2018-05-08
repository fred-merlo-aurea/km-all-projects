using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.MarketingAutomation.Models.PostModels.ECN_Objects
{
    public class GroupSelect
    {
        public int CustomerID { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public int FolderID { get; set; }
        public string GroupDescription { get; set; }
    }
}