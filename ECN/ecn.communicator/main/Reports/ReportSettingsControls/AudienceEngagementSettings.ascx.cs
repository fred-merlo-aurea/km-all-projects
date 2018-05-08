using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.Reports.ReportSettingsControls
{
    public partial class AudienceEngagementSettings : System.Web.UI.UserControl, IReportSettingsControl
    {
        #region IReportSettingsControl
        public void SetParameters(int ReportScheduleID)
        {
            if (ReportScheduleID > 0)
            {
                ECN_Framework_Entities.Communicator.ReportSchedule ReportSchedule = ECN_Framework_BusinessLayer.Communicator.ReportSchedule.GetByReportScheduleID(ReportScheduleID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                try
                {
                    if (ReportSchedule.ReportParameters != null && ReportSchedule.ReportParameters != string.Empty)
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(ReportSchedule.ReportParameters);
                        var node = xmlDoc.GetElementsByTagName("GroupID")[0];
                        setGroup(Convert.ToInt32(node.InnerText));

                        node = xmlDoc.GetElementsByTagName("ClickPercentage")[0];
                        txtClickPercentage.Text = node.InnerText;
                    }
                }
                catch { }
            }
        }

        public string GetParameters()
        {
            if (!hfSelectGroupID.Value.Equals("0") && txtClickPercentage.Text != string.Empty)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<xml>");
                sb.Append("<GroupID>");
                sb.Append(hfSelectGroupID.Value);
                sb.Append("</GroupID>");
                sb.Append("<ClickPercentage>");
                sb.Append(txtClickPercentage.Text);
                sb.Append("</ClickPercentage>");
                sb.Append("</xml>");
                return sb.ToString();
            }
            else
            {

                ECNError ecnError = new ECNError(Enums.Entity.ReportSchedule, Enums.Method.Save, "Invalid input. Please check the Group and Click Percentage entered.");
                List<ECNError> errorList = new List<ECNError>();
                errorList.Add(ecnError);
                throw new ECNException(errorList, Enums.ExceptionLayer.WebSite);
            }
        }

        public bool IsValid()
        {
            if (Page.IsValid)
            {
                return true;
            }
            else
                return false;
        }
        #endregion

        private void setGroup(int GroupID)
        {
            if (GroupID > 0)
            {
                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(GroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                lblSelectGroupName.Text = group.GroupName;
                hfSelectGroupID.Value = GroupID.ToString();
            }
        }

        delegate void HidePopup();

        protected void Page_Load(object sender, EventArgs e)
        {
            HidePopup delGroupsLookupPopup = new HidePopup(GroupsLookupPopupHide);
            this.ctrlgroupsLookup1.hideGroupsLookupPopup = delGroupsLookupPopup;
            ctrlgroupsLookup1.ShowArchiveFilter = false;
        }

        private void GroupsLookupPopupHide()
        {
            ctrlgroupsLookup1.Visible = false;
        }

        protected void imgSelectGroup_Click(object sender, ImageClickEventArgs e)
        {
            ctrlgroupsLookup1.LoadControl();
            ctrlgroupsLookup1.Visible = true;
        }

        protected override bool OnBubbleEvent(object sender, EventArgs e)
        {
            try
            {
                string source = sender.ToString();
                if (source.Equals("GroupSelected"))
                {
                    setGroup(ctrlgroupsLookup1.selectedGroupID);
                    ctrlgroupsLookup1.Visible = false;
                }
            }
            catch { }
            return true;
        }


    }
}