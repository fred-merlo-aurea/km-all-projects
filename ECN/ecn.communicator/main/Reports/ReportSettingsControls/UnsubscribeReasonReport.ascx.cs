using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.Reports.ReportSettingsControls
{
    public partial class UnsubscribeReasonReport : System.Web.UI.UserControl, IReportSettingsControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SetParameters(int reportScheduleId)
        {
            if (reportScheduleId > 0)
            {
                try
                {
                    ECN_Framework_Entities.Communicator.ReportSchedule ReportSchedule = ECN_Framework_BusinessLayer.Communicator.ReportSchedule.GetByReportScheduleID(reportScheduleId, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    if (!string.IsNullOrEmpty(ReportSchedule.ReportParameters))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(ReportSchedule.ReportParameters);

                        XmlNode  nodeSearchBy = xmlDoc.DocumentElement.SelectSingleNode("SearchBy");
                 
                        XmlNode nodeSearchCriteria = xmlDoc.DocumentElement.SelectSingleNode("SearchText");
                   
                        XmlNode nodeFTPURL = xmlDoc.DocumentElement.SelectSingleNode("FTPURL");
                        XmlNode nodeFTPUserName = xmlDoc.DocumentElement.SelectSingleNode("FTPUsername");
                        XmlNode nodeFTPPassword = xmlDoc.DocumentElement.SelectSingleNode("FTPPassword");
                        setFTP(nodeSearchBy.InnerText, nodeSearchCriteria.InnerText, nodeFTPURL.InnerText, nodeFTPUserName.InnerText, nodeFTPPassword.InnerText);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void setFTP(string SearchBy, string SearchCriteria, string URL, string Username, string Password)
        {
            ddlSearchBy.SelectedValue = SearchBy;
            txtSearchCriteria.Text = SearchCriteria;

            txtFTPURL.Text = URL;
            txtFTPUsername.Text = Username;
            txtFTPPassword.Text = Password;
        }

        public string GetParameters()
        {
            if (txtFTPURL.Text.Length > 0)
            {
                if (txtFTPUsername.Text.Length > 0 && txtFTPPassword.Text.Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<xml>");
                    sb.Append("<SearchBy>");
                    sb.Append(ddlSearchBy.Text.Trim());
                    sb.Append("</SearchBy>");
                    sb.Append("<SearchText>");
                    sb.Append(txtSearchCriteria.Text.Trim());
                    sb.Append("</SearchText>");
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

        public bool IsValid()
        {
            if ((txtFTPURL.Text == "") || (txtFTPUsername.Text == "") || (txtFTPPassword.Text==""))
                return false;

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
    }
}