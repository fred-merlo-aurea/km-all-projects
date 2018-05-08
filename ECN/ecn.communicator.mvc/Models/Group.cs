using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.mvc.Models
{
    public class Group : ECN_Framework_Entities.Communicator.Group
    {
        public Group() : base()
        {
            Subscribers = -1;
            Emails = new List<Email>();
        }

        public Group(ECN_Framework_Entities.Communicator.Group group)
        {
            TotalCount = 0;
            AllowUDFHistory = group.AllowUDFHistory;
            Archived = group.Archived.HasValue ? group.Archived.Value : false;
            CreatedDate = group.CreatedDate;
            CreatedUserID = group.CreatedUserID;
            CustomerID = group.CustomerID;
            FolderID = group.FolderID;
            GroupDescription = group.GroupDescription;
            GroupID = group.GroupID;
            GroupName = group.GroupName;
            IsSeedList = group.IsSeedList;
            MasterSuppression = group.MasterSupression;
            OptinFields = group.OptinFields;
            //OptinHTML = group.OptinHTML;
            OwnerTypeCode = group.OwnerTypeCode;
            PublicFolder = group.PublicFolder;
            UpdatedDate = group.UpdatedDate;
            UpdatedUserID = group.UpdatedUserID;
            Subscribers = -1;
            Emails = new List<Email>();

            if(group.FolderID.HasValue && group.FolderID.Value > 0)
            {
                ECN_Framework_Entities.Communicator.Folder f = ECN_Framework_BusinessLayer.Communicator.Folder.GetByFolderID_NoAccessCheck(group.FolderID.Value);
                if(f != null)
                {
                    FolderName = f.FolderName;
                }
            }
            else
            {
                FolderName = "Root";
            }
        }

        public Group(ECN_Framework_Entities.Communicator.Group group, List<ECN_Framework_Entities.Communicator.Email> emails) : this(group)
        {
            foreach(ECN_Framework_Entities.Communicator.Email email in emails)
            {
                Emails.Add(new ecn.communicator.mvc.Models.Email(email, GroupID));
            }
        }
        public int TotalCount { get; set; }
        public new DateTime? UpdatedDate { get; private set; }
        public int Subscribers { get; set; }
        public int? MasterSuppression { get; set; }
        public List<Email> Emails { get; set; }
        public string FolderName { get; set; }

    }
}