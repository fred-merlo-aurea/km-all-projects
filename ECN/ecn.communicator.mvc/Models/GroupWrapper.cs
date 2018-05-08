using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ecn.communicator.mvc.Infrastructure;

namespace ecn.communicator.mvc.Models
{
    public class GroupWrapper : BaseWrapper
    {
        public GroupWrapper()
        {
            group = new Group();
            FolderList = new List<ECN_Framework_Entities.Communicator.Folder>();
            SeedListAllowed = false;
            SubscribeTypeCodes = new List<Tuple<string, string>>();
            KendoFolders = null;
            Comparator = "like";
            SearchValue = "";
        }
        public GroupWrapper(Group g)
        {
            group = g;
            FolderList = new List<ECN_Framework_Entities.Communicator.Folder>();
            SeedListAllowed = false;
            SubscribeTypeCodes = new List<Tuple<string, string>>();
            KendoFolders = null;
            Comparator = "like";
            SearchValue = "";
        }
        public ecn.communicator.mvc.Models.Group group { get; set; }
        public List<ECN_Framework_Entities.Communicator.Folder> FolderList { get; set; }
        public bool SeedListAllowed { get; set; }
        public List<Tuple<string, string>> SubscribeTypeCodes { get; set; }

        public IEnumerable<Kendo.Mvc.UI.TreeViewItemModel> KendoFolders { get; set; }

        public string Comparator { get; set; }

        public string SearchValue { get; set; }
    }
}