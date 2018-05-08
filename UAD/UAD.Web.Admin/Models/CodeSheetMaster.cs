using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAD.Web.Admin.Models
{
    public class CodeSheetMaster
    {
        public int MasterID { get; set; }
        public string MasterValue { get; set; }
        public string MasterDesc { get; set; }
        public int MasterGroupID { get; set; }
        public string DisplayName { get; set; }
        public string Title { get; set; }
    }
}