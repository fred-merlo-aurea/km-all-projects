using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework.Accounts.Entity;
using ecn.common.classes;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.IO;

namespace ecn.accounts.main.reports
{
    public partial class BPAAuditReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpChannel.DataSource = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll().OrderBy(x => x.BaseChannelName);
                drpChannel.DataBind();
                drpChannel.Items.Insert(0, new ListItem("-- ALL --", "0"));
            }
        }

        protected void drpChannel_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            drpCustomer.DataSource = ECN_Framework_BusinessLayer.Accounts.Customer.GetCustomersByChannelID(Convert.ToInt32(drpChannel.SelectedItem.Value));
            drpCustomer.DataBind();
        }

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spBPArenewalusingClicks";
            cmd.Parameters.AddWithValue("@customerID", drpCustomer.SelectedItem.Value);
            cmd.Parameters.AddWithValue("@GroupID", txtGroupID.Text);
            cmd.Parameters.AddWithValue("@PubCode", txtPubCode.Text);
            cmd.Parameters.AddWithValue("@fromdt", txtstartDate.Text);
            cmd.Parameters.AddWithValue("@todt", txtendDate.Text);
            cmd.Parameters.AddWithValue("@HowManyClicks", txtClicks.Text);
            DataSet ds = DataFunctions.GetDataSet("activity", cmd);

            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + drpCustomer.SelectedItem.Value + "/downloads/");
            String dfileName = "";
            String afileName = "";

            DataTable dtAudit = ds.Tables[0];
            afileName = drpCustomer.SelectedItem.Value + "-" + txtPubCode.Text.Trim() + "-Audit-" + DateTime.Now.ToString("M-d-yyyy") + ".xls";
            ExportToFile(txtoutFilePath, Path.Combine(txtoutFilePath, afileName), dtAudit, afileName);
            
            DataTable dtData = ds.Tables[1];
            dfileName = drpCustomer.SelectedItem.Value + "-" + txtPubCode.Text.Trim() + "-Data-" + DateTime.Now.ToString("M-d-yyyy") + ".xls";
            ExportToFile(txtoutFilePath, Path.Combine(txtoutFilePath, dfileName), dtData, dfileName);

            DirectoryInfo Info = new DirectoryInfo(txtoutFilePath);

            FileInfo[] rgInfo = Info.GetFiles("*.xls");

            foreach (FileInfo fi in rgInfo)
            {
                if (fi.Name.Contains("Audit"))
                {
                    hAuditReport.HRef = txtoutFilePath + fi.Name;
                    lblAuditReport.Text = fi.Name;
                }
                else
                {
                    hDataReport.HRef = txtoutFilePath + fi.Name;
                    lblDataReport.Text = fi.Name;
                }

                //dt.Rows.Add("<a href='" + txtoutFilePath + fi.Name + "' runat='server' id='lnk" + fi.Name + "' target='_blank'>" + (fi.Name.Split('.').GetValue(0).ToString()).Replace('_', ' ') + "</a>");
            }

        }


        protected void ExportToFile(string outFilePath, string outFileName, DataTable dt, string tfile)
        {
            ArrayList columnHeadings = new ArrayList();
            IEnumerator aListEnum = null;
            string newline = "";

            if (!Directory.Exists(outFilePath))
            {
                Directory.CreateDirectory(outFilePath);
            }

            if (File.Exists(outFileName))
            {
                File.Delete(outFileName);
            }

            TextWriter txtfile = File.AppendText(outFileName);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    columnHeadings.Add(dt.Columns[i].ColumnName.ToString());
                }

                aListEnum = columnHeadings.GetEnumerator();
                while (aListEnum.MoveNext())
                {
                    newline += aListEnum.Current.ToString() + "\t";
                }
                txtfile.WriteLine(newline);

                foreach (DataRow dr in dt.Rows)
                {
                    newline = "";
                    aListEnum.Reset();
                    string colData = "";
                    while (aListEnum.MoveNext())
                    {
                        colData = dr[aListEnum.Current.ToString()].ToString();
                        colData = colData.Replace("\r", "");
                        colData = colData.Replace("\n", "");
                        newline += colData + "\t";
                    }
                    txtfile.WriteLine(newline);
                }
                txtfile.Close();
           }
        }
    }
}