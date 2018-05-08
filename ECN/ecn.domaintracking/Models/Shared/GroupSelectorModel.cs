

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using ECN_Framework_Entities.Communicator;

namespace ecn.domaintracking.Models.Shared
{
    public class GroupSelectorModel
    {
        #region Properties
        public int DomainTrackId { get; set; }
        
        public int FolderIdNewGroup { get; set; }
        public int FolderId { get; set; }
        public List<SelectListItem> FolderList { get; set; }
        public int GroupId { get; set; }
        public List<SelectListItem> GroupList { get; set; }

        public string toDate { get; set; }
        public string fromDate { get; set; }
        public string pageUrl { get; set; }

        public string FilterEmail { get; set; }
        public string ExportType { get; set; }
        public string pathname { get; set; }
        
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }

        public string TypeFilter { get; set; }
        #endregion

        #region Constructor
        public GroupSelectorModel()
        {
            var session = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            int customerId = session.CurrentCustomer.CustomerID;
            var currentUser = session.CurrentUser;

            
            FolderList = new[] { new SelectListItem { Text = "Root", Value = "0", Selected = true } }.Concat(
            from f in ECN_Framework_BusinessLayer.Communicator.Folder.GetByType(customerId, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.GRP.ToString(), currentUser)
            select new SelectListItem
            {
                Text = f.FolderName,
                Value = f.FolderID.ToString()
            }).ToList();


            DataTable ECNCurrentGroups = ECN_Framework_BusinessLayer.Communicator.Group.GetSubscribers(0, currentUser,1,10000000);
            GroupList = new List<SelectListItem>();
            for (int j = 0; j < ECNCurrentGroups.Rows.Count; j++)
            {
                var sl = new SelectListItem
                {
                    Value = ECNCurrentGroups.Rows[j]["GroupID"].ToString(),
                    Text = ECNCurrentGroups.Rows[j]["GroupName"].ToString()
                };
                GroupList.Add(sl);
            }

            TypeFilter = "known";
        }
        #endregion Constructor
    }
}