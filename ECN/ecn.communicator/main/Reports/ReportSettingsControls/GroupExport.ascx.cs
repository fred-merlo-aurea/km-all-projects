using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Xml;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.Reports.ReportSettingsControls
{
    public partial class GroupExport : System.Web.UI.UserControl, IReportSettingsControl
    {
        #region IReportSettingsControl
        public void SetParameters(int ReportScheduleID)
        {
            if (ReportScheduleID > 0)
            {
                try
                {

                    ECN_Framework_Entities.Communicator.ReportSchedule ReportSchedule = ECN_Framework_BusinessLayer.Communicator.ReportSchedule.GetByReportScheduleID(ReportScheduleID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    if (ReportSchedule.ReportParameters != null && ReportSchedule.ReportParameters != string.Empty)
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(ReportSchedule.ReportParameters);
                        var node = xmlDoc.GetElementsByTagName("GroupID")[0];
                        setGroup(Convert.ToInt32(node.InnerText));
                        var node2 = xmlDoc.GetElementsByTagName("ExportFormat")[0];
                        setExportType(node2.InnerText);
                        var node3 = xmlDoc.GetElementsByTagName("ExportSettings")[0];
                        setSettings(node3.InnerText);
                        var node4 = xmlDoc.GetElementsByTagName("ExportSubscribeTypeCode")[0];
                        setSubscribeTypeCode(node4.InnerText);
                        var node5 = xmlDoc.GetElementsByTagName("FTPURL")[0];
                        var node6 = xmlDoc.GetElementsByTagName("FTPUsername")[0];
                        var node7 = xmlDoc.GetElementsByTagName("FTPPassword")[0];
                        setFTP(node5.InnerText, node6.InnerText, node7.InnerText);
                        var node8 = xmlDoc.GetElementsByTagName("FilterID")[0];
                        setFilter(Convert.ToInt32(node8.InnerText));
                    }
                }
                catch { }
            }
        }

        public string GetParameters()
        {
            if (!hfSelectGroupID.Value.Equals("0"))
            {
                if (txtFTPURL.Text.Length > 0)
                {
                    if (txtFTPUsername.Text.Length > 0 && txtFTPPassword.Text.Length > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<xml>");
                        sb.Append("<GroupID>");
                        sb.Append(hfSelectGroupID.Value);
                        sb.Append("</GroupID>");
                        sb.Append("<ExportFormat>");
                        sb.Append(ddlFormat.SelectedValue.ToString());
                        sb.Append("</ExportFormat>");
                        sb.Append("<ExportSettings>");
                        sb.Append(groupExportSettings.selected.ToString());
                        sb.Append("</ExportSettings>");
                        sb.Append("<ExportSubscribeTypeCode>");
                        sb.Append(ddlSubscribeTypeCode.SelectedValue.ToString());
                        sb.Append("</ExportSubscribeTypeCode>");
                        sb.Append("<FTPURL>");
                        sb.Append(txtFTPURL.Text.Trim());
                        sb.Append("</FTPURL>");
                        sb.Append("<FTPUsername>");
                        sb.Append(txtFTPUsername.Text.Trim());
                        sb.Append("</FTPUsername>");
                        sb.Append("<FTPPassword>");
                        sb.Append(txtFTPPassword.Text.Trim());
                        sb.Append("</FTPPassword>");
                        sb.Append("<FilterID>");
                        sb.Append(ddlFilters.SelectedValue);
                        sb.Append("</FilterID>");
                        sb.Append("</xml>");
                        return sb.ToString();
                    }
                    else
                    {
                        ECNError ecnError = new ECNError(Enums.Entity.ReportSchedule, Enums.Method.Save, "Please enter a Username and/or password for connecting to the FTP site");
                        List<ECNError> errorList = new List<ECNError>();
                        errorList.Add(ecnError);
                        throw new ECNException(errorList, Enums.ExceptionLayer.WebSite);

                    }

                }
                else
                {
                    ECNError ecnError = new ECNError(Enums.Entity.ReportSchedule, Enums.Method.Save, "Please enter an ftp URL");
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(ecnError);
                    throw new ECNException(errorList, Enums.ExceptionLayer.WebSite);
                }
            }
            else
            {

                ECNError ecnError = new ECNError(Enums.Entity.ReportSchedule, Enums.Method.Save, "Invalid Group");
                List<ECNError> errorList = new List<ECNError>();
                errorList.Add(ecnError);
                throw new ECNException(errorList, Enums.ExceptionLayer.WebSite);
            }
        }

        public bool IsValid()
        {
            revFTPURL.Validate();
            if (!revFTPURL.IsValid)
            {
                
                return false;
            }

            rfvURL.Validate();
            if (!rfvURL.IsValid)
            {
                return false;
            }

            rfvURLname.Validate();
            if (!rfvURLname.IsValid)
            {
                return false;
            }

            rfvURLpassword.Validate();
            if (!rfvURLpassword.IsValid)
            {
                return false;
            }

            //try to post to ftp
            ECN_Framework_Common.Functions.FtpFunctions ftp = new ECN_Framework_Common.Functions.FtpFunctions(txtFTPURL.Text, txtFTPUsername.Text, txtFTPPassword.Text);
            if (!ftp.ValidateCredentials(txtFTPUsername.Text, txtFTPPassword.Text, txtFTPURL.Text, "", "") || (!ftp.ValidateFtpUrl(txtFTPUsername.Text, txtFTPPassword.Text, txtFTPURL.Text, "", "")))
                return false;
            return true;
        }
        #endregion

        private void setGroup(int GroupID)
        {
            if (GroupID > 0)
            {
                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(GroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                lblSelectGroupName.Text = group.GroupName;
                hfSelectGroupID.Value = GroupID.ToString();

                loadFilters();
            }
        }

        private void setFilter(int FilterID)
        {
            if (FilterID > 0)
            {
                ddlFilters.SelectedValue = FilterID.ToString();
            }
        }

        private void setExportType(string format)
        {
            if (format.Length > 0)
            {
                ddlFormat.SelectedValue = format;
            }
        }

        private void setSettings(string settings)
        {
            if (settings.Length > 0)
            {
                groupExportSettings.selected = settings;
            }
        }

        private void setSubscribeTypeCode(string code)
        {
            if (code.Length > 0)
            {
                ddlSubscribeTypeCode.SelectedValue = code;
            }
        }

        private void setFTP(string URL, string Username, string Password)
        {
            txtFTPURL.Text = URL;
            txtFTPUsername.Text = Username;
            txtFTPPassword.Text = Password;
        }


        private void loadFilters()
        {
            ddlFilters.Items.Clear();
            ddlFilters.SelectedValue = null;
            ddlFilters.DataSource = ECN_Framework_BusinessLayer.Communicator.Filter.GetByGroupID(Convert.ToInt32(hfSelectGroupID.Value), true, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            ddlFilters.DataBind();
            ddlFilters.Items.Insert(0, new ListItem("-Select-", "0"));
            upMain.Update();
        }

         delegate void HidePopup();

        protected void Page_Load(object sender, EventArgs e)
        {
            HidePopup delGroupsLookupPopup = new HidePopup(GroupsLookupPopupHide);
            this.ctrlgroupsLookup1.hideGroupsLookupPopup = delGroupsLookupPopup;
            ctrlgroupsLookup1.ShowArchiveFilter = false;
            if (Request.QueryString["ReportSchedule"] == null)
            {
                groupExportSettings.selected = "ProfileOnly";
            }
        }

        private void GroupsLookupPopupHide()
	    {
		    ctrlgroupsLookup1.Visible = false;
        }

        protected void imgSelectGroup_Click(object sender, ImageClickEventArgs e)
        {
            hfGroupSelectionMode.Value = "SelectGroup";
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