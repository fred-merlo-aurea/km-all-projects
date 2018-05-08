using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.MarketingAutomation.Models.PostModels.ECN_Objects
{
    public class MessageSelect
    {
        public MessageSelect()
        {
            MessageID = -1;
            MessageName = "";
            IsArchived = false;
            CustomerID = -1;
            FolderID = 0;
            EmailSubject = "";
            FromName = "";
            ReplyTo = "";
            FromEmail = "";
            CreatedDate = null;
            CreatedUserID = -1;
            UpdatedDate = null;
            UpdatedUserID = -1;
            CustomerName = "";
        }
        public int MessageID { get; set; }
        public string MessageName { get; set; }

        public bool? IsArchived { get; set; }
        public int CustomerID { get; set; }
        public int FolderID { get; set; }
        public string EmailSubject { get; set; }
        public string FromName { get; set; }
        public string ReplyTo { get; set; }
        public string FromEmail { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedUserID { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdatedUserID { get; set; }
        public string CustomerName { get; set; }
    }
}