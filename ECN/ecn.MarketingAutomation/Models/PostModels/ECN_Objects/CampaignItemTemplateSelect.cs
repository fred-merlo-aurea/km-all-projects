using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.MarketingAutomation.Models.PostModels.ECN_Objects
{
    public class CampaignItemTemplateSelect
    {
        public CampaignItemTemplateSelect()
        {
            CITemplateID = -1;
            TemplateName = "";
            Archived = false;
            CustomerID = -1;
            MessageID = -1;
            MessageName = "";
            GroupID = -1;
            BlastField1 = "";
            BlastField2 = "";
            BlastField3 = "";
            BlastField4 = "";
            BlastField5 = "";
            CreatedDate = null;
            UpdatedDate = null;
            FromEmail = "";
            FromName = "";
            ReplyTo= "";
            Subject= "";
            SuppressionGroupList = null;
            SuppressionGroupFilterList = null;
            SelectedGroupList = null;
            SelectedGroupFilterList = null;
            OptoutGroupList = null;
            CampaignID = -1;
            CampaignName = "";
        }
        public int CITemplateID { get; set; }
        public string TemplateName { get; set; }
        public int MessageID { get; set; }
        public string MessageName { get; set; }
        public bool? Archived { get; set; }
        public int CustomerID { get; set; }
        public int GroupID { get; set; }

        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string ReplyTo { get; set; }
        public string Subject { get; set; }
        public string BlastField1 { get; set; }
        public string BlastField2 { get; set; }
        public string BlastField3 { get; set; }
        public string BlastField4 { get; set; }
        public string BlastField5 { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedUserID { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdatedUserID { get; set; }
        public string CustomerName { get; set; }
        public IEnumerable<GroupSelect> SuppressionGroupList { get; set; }
        public IEnumerable<FilterSelect> SuppressionGroupFilterList { get; set; }
        public IEnumerable<FilterSelect> SelectedGroupFilterList { get; set; }

        public IEnumerable<GroupSelect> SelectedGroupList { get; set; }

        public List<ECN_Framework_Entities.Communicator.CampaignItemTemplateOptoutGroup> OptoutGroupList { get; set; }
        public string CampaignName { get; set; }
        public int CampaignID { get; set; }
    }
}