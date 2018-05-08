using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.mvc.Models
{
    public class FolderExplorer : BaseWrapper
    {
        public FolderExplorer(IEnumerable<Kendo.Mvc.UI.TreeViewItemModel> ft)
        {
            FolderTree = ft;
            GroupsGrid = new List<ecn.communicator.mvc.Models.Group>();
            
        }
        public IEnumerable<Kendo.Mvc.UI.TreeViewItemModel> FolderTree { get; set; }

        public List<ecn.communicator.mvc.Models.Group> GroupsGrid { get; set; }
    }
}