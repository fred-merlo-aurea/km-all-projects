using System;
using System.Collections.Generic;
using System.Web;

namespace ecn.communicator.classes
{
    public class ECN_Group
    {
        public ECN_Group()
        {
            GroupID = -1;
            GroupName = string.Empty;
        }

        public int GroupID { get; set; }
        public string GroupName { get; set; }
    }
}
