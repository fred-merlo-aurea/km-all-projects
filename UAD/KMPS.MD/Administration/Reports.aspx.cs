using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using KMPS.MD.Objects;

namespace KMPS.MD.Administration
{
    public partial class Reports : KMPS.MD.Main.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Reports";
            //Trace.Write(DataFunctions.GetClientSqlConnection(Master.clientconnections));
        }

        protected void btnCreateReport_Click(object sender, EventArgs e)
        {
            if (ddlReports.SelectedItem.Value == "Products")
            {
                //ddlOrderBy.SelectedValue spGetAdminReport
                SqlConnection conn = DataFunctions.GetClientSqlConnection(Master.clientconnections);

                SqlCommand cmd = new SqlCommand("spGetAdminReport", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@OrderBy", ddlOrderBy.SelectedValue));
                cmd.CommandTimeout = 0;

                DataSet ds = new DataSet();
                DataTable dt = new DataTable("DataTable");
                ds.Tables.Add(dt);

                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                ds.Tables["DataTable"].Load(sdr);
                conn.Close();

                string outputFolder = Server.MapPath("temp/");
                DirectoryInfo targetDirectory = new DirectoryInfo(outputFolder);
                if (!targetDirectory.Exists)
                {
                    targetDirectory.Create();
                }

                string outfileName = outputFolder + System.Guid.NewGuid().ToString().Substring(0, 5) + ".tsv";
                TextWriter txtfile = File.AppendText(outfileName);
                string newline = "";

                foreach (DataColumn col in dt.Columns)
                {
                    newline += col.ColumnName + "\t";
                }
                txtfile.WriteLine(newline);

                foreach (DataRow dr in dt.Rows)
                {
                    newline = "";
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        newline += dr[i].ToString() + "\t";
                    }
                    txtfile.WriteLine(newline);
                }
                txtfile.Close();
                txtfile.Dispose();

                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment; filename=admin_report.tsv");
                Response.TransmitFile(outfileName);
                Response.Flush();
                Response.End();
            }

        }
    }
}