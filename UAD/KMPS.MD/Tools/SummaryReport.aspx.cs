using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Data.SqlClient;
using System.IO;

namespace KMPS.MD.Tools
{
    public partial class SummaryReport : KMPS.MD.Main.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Tools";
            Master.SubMenu = "Summary Report";

            lblErrorMessage.Text = "";
            divError.Visible = false;

            if (!IsPostBack)
            {
                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.SummaryReport, KMPlatform.Enums.Access.View))
                {
                    Response.Redirect("../SecurityAccessError.aspx");
                }

                List<Brand> b = new List<Brand>();
                bool IsBrandAssignedUser = false;

                if (!KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    b = Brand.GetByUserID(Master.clientconnections, Master.LoggedInUser);

                    if (b.Count > 0)
                        IsBrandAssignedUser = true;
                }

                if (b.Count == 0)
                {
                    b = Brand.GetAll(Master.clientconnections);
                }

                if (b.Count > 0)
                {
                    pnlBrand.Visible = true;
                    if (b.Count > 1)
                    {
                        drpBrand.Visible = true;
                        drpBrand.DataSource = b;
                        drpBrand.DataBind();
                        if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser) || !IsBrandAssignedUser)
                        {
                            drpBrand.Items.Insert(0, new ListItem("All Products", "0"));
                            hfBrandID.Value = drpBrand.SelectedItem.Value;
                        }
                        else
                        {
                            drpBrand.Items.Insert(0, new ListItem("", "-1"));
                            hfBrandID.Value = drpBrand.SelectedItem.Value;
                        }
                    }
                    else if (b.Count == 1)
                    {
                        if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser) || !IsBrandAssignedUser)
                        {
                            drpBrand.Items.Insert(0, new ListItem("All Products", "0"));
                            hfBrandID.Value = drpBrand.SelectedItem.Value;
                        }
                        else
                        {
                            lblColon.Visible = true;
                            lblBrandName.Visible = true;
                            lblBrandName.Text = b.FirstOrDefault().BrandName;
                            hfBrandID.Value = b.FirstOrDefault().BrandID.ToString();
                        }
                    }
                }
            }
        }

        protected void drpBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            hfBrandID.Value = drpBrand.SelectedValue;
            GrdReports.DataSource = null;
            GrdReports.DataBind();
            GrdReports.Visible = false;
        }

        private DataTable SummaryReportData()
        {
            DataTable dt = null;
            try
            {
                if (Convert.ToInt32(hfBrandID.Value) >= 0)
                {
                    SqlCommand cmd = new SqlCommand("sp_SummaryReport");
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MasterGroupID", "");
                    cmd.Parameters.AddWithValue("@BrandID", Convert.ToInt32(hfBrandID.Value));
                    dt = DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(Master.clientconnections));
                }
            }
            catch (Exception ex)
            {
                divError.Visible = true;
                lblErrorMessage.Text = ex.Message;
            }

            return dt;
        }

        protected void ReportViewer_ReportError(object sender, ReportErrorEventArgs e)
        {
            Exception ex = e.Exception;
            divError.Visible = true;
            lblErrorMessage.Text = ex.Message;
        }

        protected void btnDownloadSummary_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.SummaryReport, KMPlatform.Enums.Access.View))
            {
                try
                {
                    bool reCreate = false;

                    DataTable dt = new DataTable();
                    DataColumn dc;
                    dc = new DataColumn("FileName");
                    dt.Columns.Add(dc);

                    string DatabaseName = DataFunctions.GetDBName(Master.clientconnections);

                    string path = string.Empty;

                    if (pnlBrand.Visible)
                        path = Server.MapPath("~/downloads/" + DatabaseName + "/" + hfBrandID.Value + "/");
                    else
                        path = Server.MapPath("~/downloads/" + DatabaseName + "/");

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    if (chkRefresh.Checked)
                    {
                        DirectoryInfo di = new DirectoryInfo(path);

                        FileInfo[] rgFiles = di.GetFiles("*.*");
                        foreach (FileInfo fi in rgFiles)
                        {
                            fi.Delete();
                        }

                        reCreate = true;
                    }
                    else
                    {
                        //use the file that is already in the folder
                        DirectoryInfo di = new DirectoryInfo(path);

                        FileInfo[] rgFiles = di.GetFiles("*.*");
                        if (rgFiles.Length == 0)
                        {
                            reCreate = true;
                        }
                    }

                    if (reCreate)
                    {
                        RenderReport();
                        Microsoft.Reporting.WebForms.Warning[] warnings = null;
                        string[] streamids = null;
                        String mimeType = null;
                        String encoding = null;
                        String extension = null;
                        Byte[] bytes = null;
                        bytes = ReportViewer1.LocalReport.Render("pdf", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                        FileStream fs = File.Create(path + "summary_" + DateTime.Now.ToString("MM-dd-yyyy hh-mm-ss") + ".pdf");
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Close();

                        bytes = ReportViewer1.LocalReport.Render("excel", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                        FileStream fsExcel = File.Create(path + "summary_" + DateTime.Now.ToString("MM-dd-yyyy hh-mm-ss") + ".xls");
                        fsExcel.Write(bytes, 0, bytes.Length);
                        fsExcel.Close();
                    }

                    var filename = Directory.GetFiles(path)
                         .Select(x => new FileInfo(x))
                         .OrderByDescending(x => x.LastWriteTime)
                         .Take(2)
                         .ToArray();

                    foreach (FileInfo fi in filename)
                    {
                        if (pnlBrand.Visible)
                            dt.Rows.Add("<a href='../downloads/" + DatabaseName + "/" + hfBrandID.Value + "/" + fi.Name + "' runat='server' id='lnk" + fi.Name + "' target='_blank'>" + fi.Name + "</a>");
                        else
                            dt.Rows.Add("<a href='../downloads/" + DatabaseName + "/" + fi.Name + "' runat='server' id='lnk" + fi.Name + "' target='_blank'>" + fi.Name + "</a>");
                    }

                    GrdReports.DataSource = dt;
                    GrdReports.DataBind();
                    GrdReports.Visible = true;
                }
                catch (Exception ex)
                {
                    divError.Visible = true;
                    lblErrorMessage.Text = ex.Message;
                }
            }
        }

        private void RenderReport()
        {
            ReportViewer1.ReportError += new ReportErrorEventHandler(ReportViewer_ReportError);
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_SummaryReport.rdlc");
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(
                new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], SummaryReportData()));
            ReportViewer1.LocalReport.Refresh();
        }
    }

}