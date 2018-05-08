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
    public partial class EmailPreviewUsage : System.Web.UI.UserControl, IReportSettingsControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void LoadCustomers()
        {
            int baseChannelID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID;
            KMPlatform.Entity.User user = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
            List<ECN_Framework_Entities.Accounts.Customer> lstCust = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID).Where(x => x.ActiveFlag == "Y").ToList();
            lstboxCustomer.DataSource = lstCust;
            lstboxCustomer.DataTextField = "CustomerName";
            lstboxCustomer.DataValueField = "CustomerID";
            lstboxCustomer.DataBind();       

            if (!(KM.Platform.User.IsSystemAdministrator(user) || KM.Platform.User.IsChannelAdministrator(user)))
            {
                lstboxCustomer.Visible = false;
                lblHeader.Visible = false;
            }
            else
            {
                lstboxCustomer.Visible = true;
                lblHeader.Visible = true;                      
            }
        }

        #region IReportSettingsControl
        public void SetParameters(int ReportScheduleID)
        {
            LoadCustomers();
            if (ReportScheduleID > 0)
            {
                ECN_Framework_Entities.Communicator.ReportSchedule ReportSchedule = ECN_Framework_BusinessLayer.Communicator.ReportSchedule.GetByReportScheduleID(ReportScheduleID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                try
                {
                    if (ReportSchedule.ReportParameters != null && ReportSchedule.ReportParameters != string.Empty)
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(ReportSchedule.ReportParameters);
                        XmlNodeList nodeList = xmlDoc.GetElementsByTagName("CustomerID");
                        foreach (XmlNode node in nodeList)
                        {
                            ListItem item = lstboxCustomer.Items.FindByValue(node.InnerText);
                            if (item != null)
                                item.Selected = true;
                        }
                    }
                }
                catch { }
            }
        }

        public string GetParameters()
        {
            bool selected= false;
            KMPlatform.Entity.User user = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            if (!(KM.Platform.User.IsSystemAdministrator(user) || KM.Platform.User.IsChannelAdministrator(user)))
            {
                selected = true;
                sb.Append("<CustomerID>");
                sb.Append(user.CustomerID.ToString());
                sb.Append("</CustomerID>");
            }
            else
            {
                foreach (ListItem li in lstboxCustomer.Items)
                {
                    if (li.Selected)
                    {
                        selected = true;
                        sb.Append("<CustomerID>");
                        sb.Append(li.Value);
                        sb.Append("</CustomerID>");
                    }
                }
            }

            sb.Append("</xml>");

            if (selected == false)
            {

                ECNError ecnError = new ECNError(Enums.Entity.ReportSchedule, Enums.Method.Save, "Please select a Customer account");
                List<ECNError> errorList = new List<ECNError>();
                errorList.Add(ecnError);
                throw new ECNException(errorList, Enums.ExceptionLayer.WebSite);
            }
            return sb.ToString();

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
    }
}