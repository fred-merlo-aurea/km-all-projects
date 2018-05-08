using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Xml;

namespace ecn.communicator.main.Reports.ReportSettingsControls
{
    public partial class EmailPerformanceByDomain : System.Web.UI.UserControl, IReportSettingsControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

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
                        XmlNodeList nodeList = xmlDoc.GetElementsByTagName("DrillDownOther");
                        if(nodeList != null && nodeList.Count > 0)
                        {
                            bool drillDown = false;
                            bool.TryParse(nodeList[0].InnerText, out drillDown);
                            chkDrillDownOther.Checked = drillDown;
                        }
                    }
                }
                catch { }
            }
        }

        public string GetParameters()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<DrillDownOther>" + chkDrillDownOther.Checked.ToString() + "</DrillDownOther>");
            sb.Append("</xml>");

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
    }


}