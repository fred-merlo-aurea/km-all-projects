using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.mvc.Models
{
    public class SearchParams
    {
        public string searchType { get; set; }
        public string searchCriterion { get; set; }
        public string profileName { get; set; }
        public string archiveFilter { get; set; }
        public bool allFolders { get; set; }
        public int folderID { get; set; }
    }
}