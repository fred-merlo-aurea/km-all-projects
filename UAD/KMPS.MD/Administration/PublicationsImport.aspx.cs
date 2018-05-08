using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using KMPS.MD.Objects;

namespace KMPS.MDAdmin
{
    public partial class PublicationsImport : KMPS.MD.Main.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Products";
            Master.SubMenu = "Product Import";
            divError.Visible = false;
            lblErrorMessage.Text = "";
        }

        protected void btnCheckFile_Click(object sender, EventArgs e)
        {
            CheckFile();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            if (!CheckFile())
            {
                SqlConnection conn = null;                
                try
                {
                    conn = DataFunctions.GetClientSqlConnection(Master.clientconnections);

                    string server = (string)Request.ServerVariables["SERVER_NAME"];
                    string path = ConfigurationManager.AppSettings[DataFunctions.GetSubDomain(server) + "_PubPath"].ToString();

                    string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Mode=ReadWrite;Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;'";

                    OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
                    DataSet myDataSet = new DataSet();
                    myCommand.Fill(myDataSet, "ExcelTable");
                    DataTable myDT = myDataSet.Tables["ExcelTable"];

                    using (conn)
                    {
                        conn.Open();
                        int recordsImported = 0;
                        foreach (DataRow row in myDT.Rows)
                        {
                            string PubName = row[0].ToString().Trim();
                            if (PubName.Length == 0)
                            {
                                break;
                            }
                            string PubCode = row[1].ToString().Trim();
                            string PubTypeID = row[2].ToString().Trim();
                            SqlCommand cmdInsert = new SqlCommand("INSERT INTO Pubs(PubName, PubCode, PubTypeID) VALUES ('" + PubName + "','" + PubCode + "','" + PubTypeID + "')", conn);
                            cmdInsert.ExecuteNonQuery();
                            recordsImported++;
                        }
                        lblMessage.Text = "Imported " + recordsImported.ToString() + " records";
                    }
                }
                catch (Exception Ex)
                {
                    divError.Visible = true;
                    lblErrorMessage.Text = Ex.Message;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private bool CheckFile()
        {
            bool badFile = false;
            lblMessage.Text = "";
            try
            {
                if (FileUpload1.HasFile)
                {
                    string server = (string)Request.ServerVariables["SERVER_NAME"];
                    string path = ConfigurationManager.AppSettings[DataFunctions.GetSubDomain(server) + "_PubPath"].ToString();

                    FileUpload1.SaveAs(path);

                    string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Mode=ReadWrite;Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;'";

                    OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
                    DataSet myDataSet = new DataSet();
                    myCommand.Fill(myDataSet, "ExcelTable");
                    DataTable myDT = myDataSet.Tables["ExcelTable"];

                    string dupes = "";
                    string missingPubID = "";
                    int recordsToImport = 0;
                    foreach (DataRow row in myDT.Rows)
                    {
                        string PubName = row[0].ToString().Trim();
                        if (PubName.Length == 0)
                        {
                            break;
                        }
                        string PubCode = row[1].ToString().Trim();
                        string PubTypeID = row[2].ToString().Trim();
                        int count = Convert.ToInt32(DataFunctions.executeScalar("SELECT count(PubID) from Pubs where PubName = '" + PubName + "'", DataFunctions.GetClientSqlConnection(Master.clientconnections)));
                        if (count > 0)
                        {
                            dupes += PubName + ",";
                        }
                        count = Convert.ToInt32(DataFunctions.executeScalar("SELECT count(PubTypeID) from PubTypes where PubTypeID = " + PubTypeID + "", DataFunctions.GetClientSqlConnection(Master.clientconnections)));
                        if (count <= 0)
                        {
                            missingPubID += PubTypeID + ",";
                        }
                        recordsToImport++;
                    }
                    if (dupes.Length > 0)
                    {
                        lblErrorMessage.Text = "Duplicates found: " + dupes;
                        badFile = true;
                    }
                    if (missingPubID.Length > 0)
                    {
                        lblErrorMessage.Text = "PubType IDs not found: " + missingPubID;
                        badFile = true;
                    }
                    if (!badFile)
                    {
                        lblMessage.Text = "Can import " + recordsToImport.ToString() + " records";
                    }
                    else
                    {
                        divError.Visible = true;
                    }
                }
                else
                {
                    lblErrorMessage.Text = "You must first select a file";
                    divError.Visible = true;
                    badFile = true;
                }
            }
            catch (Exception Ex)
            {
                lblErrorMessage.Text = Ex.Message;
                divError.Visible = true;
                badFile = true;
            }
            return badFile;
        }
    }
}