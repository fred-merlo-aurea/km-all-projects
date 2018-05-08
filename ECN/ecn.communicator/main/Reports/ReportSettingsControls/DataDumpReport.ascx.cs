using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ECN_Framework_Common.Objects;
using System.Xml;

namespace ecn.communicator.main.Reports.ReportSettingsControls
{
    public partial class DataDumpReport : System.Web.UI.UserControl, IReportSettingsControl
    {
        private static List<ECN_Framework_Entities.Communicator.Group> groupList;

        public void SetParameters(int reportScheduleID)
        {
            groupList = new List<ECN_Framework_Entities.Communicator.Group>();
            if (reportScheduleID > 0)
            {
                try
                {
                    ECN_Framework_Entities.Communicator.ReportSchedule ReportSchedule = ECN_Framework_BusinessLayer.Communicator.ReportSchedule.GetByReportScheduleID(reportScheduleID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    if (ReportSchedule.ReportParameters != null && ReportSchedule.ReportParameters != string.Empty)
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(ReportSchedule.ReportParameters);
                        XmlNode nodeGroupIDs = xmlDoc.DocumentElement.SelectSingleNode("GroupIDs");
                        if (nodeGroupIDs != null)
                        {
                            string[] groupIDs = nodeGroupIDs.InnerText.Split(',');

                           

                            foreach (string s in groupIDs)
                            {
                                ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(Convert.ToInt32(s), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                                groupList.Add(g);
                            }
                            if (groupList.Count > 0)
                            {
                                gvSelectedGroups.DataSource = groupList;
                                gvSelectedGroups.DataBind();

                                lblNoGroups.Visible = false;
                                gvSelectedGroups.Visible = true;
                            }
                            else
                            {
                                lblNoGroups.Visible = true;
                                gvSelectedGroups.Visible = false;
                            }
                        }
                        XmlNode nodeFTPURL = xmlDoc.DocumentElement.SelectSingleNode("FTPURL");
                        XmlNode nodeFTPUserName = xmlDoc.DocumentElement.SelectSingleNode("FTPUsername");
                        XmlNode nodeFTPPassword = xmlDoc.DocumentElement.SelectSingleNode("FTPPassword");
                        setFTP(nodeFTPURL.InnerText, nodeFTPUserName.InnerText, nodeFTPPassword.InnerText);


                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        public string GetParameters()
        {
            if (groupList.Count > 0)
            {
                if (txtFTPURL.Text.Length > 0)
                {
                    if (txtFTPUsername.Text.Length > 0 && txtFTPPassword.Text.Length > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<xml>");
                        sb.Append("<GroupIDs>");
                        sb.Append(GetGroupIDs());
                        sb.Append("</GroupIDs>");
                        sb.Append("<CustomerID>");
                        sb.Append(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID.ToString());
                        sb.Append("</CustomerID>");
                        sb.Append("<FTPURL>");
                        sb.Append(txtFTPURL.Text.Trim());
                        sb.Append("</FTPURL>");
                        sb.Append("<FTPUsername>");
                        sb.Append(txtFTPUsername.Text.Trim());
                        sb.Append("</FTPUsername>");
                        sb.Append("<FTPPassword>");
                        sb.Append(txtFTPPassword.Text.Trim());
                        sb.Append("</FTPPassword>");
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
                ECNError ecnError = new ECNError(Enums.Entity.ReportSchedule, Enums.Method.Save, "Please select at least one group");
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

        private void setFTP(string URL, string Username, string Password)
        {
            txtFTPURL.Text = URL;
            txtFTPUsername.Text = Username;
            txtFTPPassword.Text = Password;
        }

        private string GetGroupIDs()
        {
            StringBuilder sbGroups = new StringBuilder();
            foreach (ECN_Framework_Entities.Communicator.Group g in groupList)
            {
                sbGroups.Append(g.GroupID + ",");
            }
            return sbGroups.ToString().TrimEnd(',');
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            groupExplorer1.enableSelectMode();
           
            if (!Page.IsPostBack)
            {
                if (groupList == null || groupList.Count == 0)
                {
                    lblNoGroups.Visible = true;
                    gvSelectedGroups.Visible = false;
                }
            }
        }

        protected void gvSelectedGroups_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Equals("deletegroup"))
            {
                groupList.RemoveAll(x => x.GroupID == Convert.ToInt32(e.CommandArgument.ToString()));
                gvSelectedGroups.DataSource = groupList;
                gvSelectedGroups.DataBind();
                if(groupList.Count == 0)
                {
                    lblNoGroups.Visible = true;
                    gvSelectedGroups.Visible = false;
                }
            }
        }

        protected void gvSelectedGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ECN_Framework_Entities.Communicator.Group currentgroup = (ECN_Framework_Entities.Communicator.Group)e.Row.DataItem;
                ImageButton imgbtnDelete = (ImageButton)e.Row.FindControl("imgbtnDeleteGroup");
                imgbtnDelete.CommandArgument = currentgroup.GroupID.ToString();

            }
        }

        protected void imgSelectGroup_Click(object sender, ImageClickEventArgs e)
        {
            hfGroupSelectionMode.Value = "SelectGroup";
            groupExplorer1.reset();
            this.modalPopupGroupExplorer.Show();
        }
        protected override bool OnBubbleEvent(object sender, EventArgs e)
        {
            try
            {
                string source = sender.ToString();
                if (source.Equals("GroupSelected"))
                {
                    int groupID = groupExplorer1.selectedGroupID;
                    ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(groupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    if (hfGroupSelectionMode.Value.Equals("SelectGroup"))
                    {
                        if (groupList == null)
                            groupList = new List<ECN_Framework_Entities.Communicator.Group>();
                        if (groupList.Count(x => x.GroupID == group.GroupID) == 0)
                        {
                            groupList.Add(group);
                        }
                        gvSelectedGroups.Visible = true;
                        lblNoGroups.Visible = false;
                        gvSelectedGroups.DataSource = groupList;
                        gvSelectedGroups.DataBind();

                    }
                    else
                    {

                    }
                    
                    this.modalPopupGroupExplorer.Hide();
                }
            }
            catch { }
            return true;
        }
        protected void groupExplorer_Hide(object sender, EventArgs e)
        {
            groupExplorer1.reset();
            this.modalPopupGroupExplorer.Hide();
        }
    }
}