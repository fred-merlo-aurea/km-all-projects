using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;
using Microsoft.Reporting.WebForms;

namespace ecn.communicator.blasts.reports
{
    public partial class LinkReport : ECN_Framework.WebPageHelper
    {
        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
            Master.SubMenu = "link report";
            Master.Heading = "";
            Master.HelpContent = "";
            Master.HelpTitle = "";

            if (!IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.LinkReport, KMPlatform.Enums.Access.View))
                {
                    List<ECN_Framework_Entities.Communicator.LinkOwnerIndex> linkOwnerIndexList =
                    ECN_Framework_BusinessLayer.Communicator.LinkOwnerIndex.GetByCustomerID_NoAccessCheck(Master.UserSession.CurrentUser.CustomerID);
                    var linkOwnerIndexList_result = from src in linkOwnerIndexList
                                                    orderby src.LinkOwnerName
                                                    select src;

                    lstlinkowner.DataSource = linkOwnerIndexList_result;
                    lstlinkowner.DataBind();

                    List<ECN_Framework_Entities.Communicator.Code> codeList =
                    ECN_Framework_BusinessLayer.Communicator.Code.GetByCustomerAndCategory(ECN_Framework_Common.Objects.Communicator.Enums.CodeType.LINKTYPE, Master.UserSession.CurrentUser);
                    var codeList_result = from src in codeList
                                          orderby src.CodeDisplay
                                          select src;
                    lstlinktype.DataSource = codeList_result;
                    lstlinktype.DataBind();

                    txtTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    txtFrom.Text = DateTime.Now.AddDays(-1 * (DateTime.Now.Day - 1)).ToString("MM/dd/yyyy");

                    List<ECN_Framework_Entities.Communicator.Campaign> clist = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCustomerID_NoAccessCheck(Master.UserSession.CurrentUser.CustomerID,  false);
                    var result = (from src in clist
                                  orderby src.CampaignName
                                  select src).ToList();
                    lstCampaign.DataSource = result;
                    lstCampaign.DataTextField = "CampaignName";
                    lstCampaign.DataValueField = "CampaignID";
                    lstCampaign.DataBind();
                }
                else
                {
                    throw new ECN_Framework_Common.Objects.SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
                }

            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            DateTime.TryParse(txtFrom.Text, out startDate);
            DateTime.TryParse(txtTo.Text, out endDate);
            List<ECN_Framework_Entities.Activity.Report.LinkReportList> lrlList = ECN_Framework_BusinessLayer.Activity.Report.LinkReportList.Get(Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID), getListboxValues(lstlinkowner), getListboxValues(lstlinktype),startDate, endDate, getListboxValues(lstCampaign));
            foreach(var l in lrlList)
            {
                l.emailsubject = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(l.emailsubject);
            }
            Microsoft.Reporting.WebForms.ReportDataSource rds = new Microsoft.Reporting.WebForms.ReportDataSource("DS_LinkReport", lrlList);
            ReportViewer1.Visible = false;
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("rpt_LinkReportList.rdlc");
            ReportViewer1.LocalReport.Refresh();

            Warning[] warnings = null;
            string[] streamids = null;
            String mimeType = null;
            String encoding = null;
            String extension = null;
            Byte[] bytes = null;

            switch (drpExport.SelectedItem.Value.ToUpper())
            {
                case "PDF":
                    bytes = ReportViewer1.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                    Response.ContentType = "application/pdf";
                    break;
                case "XLS":
                    bytes = ReportViewer1.LocalReport.Render("EXCEL", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                    Response.ContentType = "application/vnd.ms-excel";
                    break;
                case "DOC":
                    Response.ContentType = "application/ms-word";
                    bytes = ReportViewer1.LocalReport.Render("WORD", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                    break;
            }

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=LinkReportList." + drpExport.SelectedItem.Value);
            Response.BinaryWrite(bytes);
            Response.End();
        }

        protected void btndownloaddetails_Click(object sender, EventArgs e)
        {

            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            DateTime.TryParse(txtFrom.Text, out startDate);
            DateTime.TryParse(txtTo.Text, out endDate);
            List<ECN_Framework_Entities.Activity.Report.LinkDetails> linkdetails = ECN_Framework_BusinessLayer.Activity.Report.LinkDetails.Get(Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID), getListboxValues(lstlinkowner), getListboxValues(lstlinktype), startDate, endDate, getListboxValues(lstCampaign));

            if (linkdetails != null)
            {
                string newline = "";
                string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");
                if (!Directory.Exists(txtoutFilePath))
                    Directory.CreateDirectory(txtoutFilePath);

                String tfile = Master.UserSession.CurrentUser.CustomerID + "-linkdetailsreport" + ".xls";
                string outfileName = txtoutFilePath + tfile;
                List<string> fakeList = null;



                string report = ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.GetTabDelimited<ECN_Framework_Entities.Activity.Report.LinkDetails, string>(linkdetails, fakeList);

                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment; filename=" + tfile);
                Response.Write(report);
                Response.Flush();
                Response.End();

                //TextWriter txtfile = File.AppendText(outfileName);

                //Type type = typeof(ECN_Framework_Entities.Activity.Report.LinkDetails);

                //foreach (System.Reflection.PropertyInfo info in type.GetProperties())
                //{
                //    newline += info.Name + "\t";
                //}

                //txtfile.WriteLine(newline);

                //foreach (ECN_Framework_Entities.Activity.Report.LinkDetails l in linkdetails)
                //{
                //    newline = "";
                //    foreach (System.Reflection.PropertyInfo info in type.GetProperties())
                //    {

                //        newline += info.GetValue(l, null) + "\t";
                //    }

                //    txtfile.WriteLine(newline);
                //}

                //txtfile.Close();
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("content-disposition", "attachment; filename=" + tfile);
                //Response.WriteFile(outfileName);
                //Response.Flush();
                //Response.End();
            }
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Link, Enums.Method.Get, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            ReportGridPager.CurrentPage = 1;
            ReportGridPager.CurrentIndex = 0;
            ReportGrid.CurrentPageIndex = 0;
            LoadReport();
        }

        private void LoadReport()
        {
            string lstCampaign_values = getListboxValues(lstCampaign);
            string lstlinkowner_values = getListboxValues(lstlinkowner);
            string lstlinktype_values = getListboxValues(lstlinktype);

            if (lstCampaign_values == string.Empty)
            {
                throwECNException("Please Select Campaign");
                return;
            }
            if (lstlinkowner_values == string.Empty)
            {
                throwECNException("Please Select Advertiser");
                return;
            }
            //if (lstlinktype_values == string.Empty)
            //{
            //    throwECNException("Please Select LinkType");
            //    return;
            //}

            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            DateTime.TryParse(txtFrom.Text, out startDate);
            DateTime.TryParse(txtTo.Text, out endDate);

            List<ECN_Framework_Entities.Activity.Report.LinkReportList> linkreportlist = ECN_Framework_BusinessLayer.Activity.Report.LinkReportList.Get(Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID), getListboxValues(lstlinkowner), getListboxValues(lstlinktype), startDate, endDate, getListboxValues(lstCampaign));
            foreach(ECN_Framework_Entities.Activity.Report.LinkReportList lrl in linkreportlist)
            {
                lrl.emailsubject = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(lrl.emailsubject);
            }
            ReportGrid.DataSource = linkreportlist;
            ReportGrid.DataBind();
            ReportGridPager.RecordCount = linkreportlist.Count;
            pnlReport.Visible = true;

            btnDownload.Visible = KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.LinkReport, KMPlatform.Enums.Access.Download);
            btndownloaddetails.Visible = KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.LinkReport, KMPlatform.Enums.Access.DownloadDetails);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ReportGridPager.CurrentPage = 1;
            ReportGridPager.CurrentIndex = 0;
            ReportGrid.CurrentPageIndex = 0;

            lstlinkowner.ClearSelection();
            lstlinktype.ClearSelection();
            lstCampaign.ClearSelection();
            txtTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
            txtFrom.Text = DateTime.Now.AddDays(-1 * (DateTime.Now.Day - 1)).ToString("MM/dd/yyyy");
            pnlReport.Visible = false;
        }

        private string getListboxValues(ListBox lst)
        {
            string selectedvalues = string.Empty;
            foreach (ListItem item in lst.Items)
            {
                if (item.Selected)
                {
                    selectedvalues += selectedvalues == string.Empty ? item.Value : "," + item.Value;
                }
            }
            return selectedvalues;
        }

        protected void ReportGridPager_IndexChanged(object sender, EventArgs e)
        {
            LoadReport();
        }
    }
}
