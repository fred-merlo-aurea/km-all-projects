using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace KMPS_Tools
{
    public partial class InterimDataExport : Form
    {
        string InterimDBConnString = System.Configuration.ConfigurationManager.ConnectionStrings["KMPSInterim"].ConnectionString;
        string ecnCommunicatorConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ECNCommunicator"].ConnectionString;

        int SelectedGroupID = 0;
        string SelectedGroupName = string.Empty;

        public InterimDataExport()
        {
            InitializeComponent();
            LoadGroups();
        }

        private void LoadGroups()
        {

            List<int> lGroupID = new List< int>(); ;
            Dictionary<int, string> dGroups = new Dictionary<int, string>(); ;

            SqlConnection conn = new SqlConnection(InterimDBConnString);
            SqlCommand cmd = new SqlCommand("select distinct groupID from PublicationSubscriptionData group by GROUPID, PUBLICATIONCODE order by groupID", conn);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;

            cmd.Connection.Open();
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (rdr.Read())
            {
                lGroupID.Add(Convert.ToInt32(rdr["groupID"]));
            }
            
            conn.Close();
            rdr.Dispose();

            string groupIDs = string.Join(",", lGroupID.ToArray());

            conn = new SqlConnection(ecnCommunicatorConnString);
            cmd = new SqlCommand("select groupID, groupname from groups where groupID in (" + groupIDs  + ") order by groupname asc", conn);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Connection.Open();
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (rdr.Read())
            {
                dGroups.Add(Convert.ToInt32(rdr["groupID"]), rdr["groupname"].ToString());
            }

            conn.Close();

            cbGroup.DataSource = new BindingSource(dGroups, null);
            cbGroup.DisplayMember = "Value";
            cbGroup.ValueMember = "Key";
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ExportData();

            backgroundWorker1.ReportProgress(100, "End Time : " + System.DateTime.Now);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lstMessage.Items.Add(e.UserState);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnExport.Enabled = true;
            btnCancel.Enabled = false;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            lstMessage.Items.Clear();

            if (cbGroup.SelectedIndex > -1)
            {
                btnExport.Enabled = false;
                btnCancel.Enabled = true;

                lstMessage.Items.Add("Start Time : " + System.DateTime.Now);
                lstMessage.Items.Add(" ");

                KeyValuePair<int, string> selectedPair = (KeyValuePair<int, string>)cbGroup.SelectedItem;

                SelectedGroupID = selectedPair.Key;
                SelectedGroupName = selectedPair.Value;

                backgroundWorker1.RunWorkerAsync();

            }
            else
            {
                MessageBox.Show("Select Pubcode to Export;");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InterimDataExport_FormClosing(object sender, FormClosingEventArgs e)
        {
            Main frm = (Main)this.MdiParent;
            frm.ToggleMenus(true);
        }

        private void ExportData()
        {
            StringBuilder strInsertXML = new StringBuilder();
            int count = 1;
            string newline = string.Empty;

            try
            {
                if (backgroundWorker1.CancellationPending)
                {
                    
                }

                SqlConnection conn = new SqlConnection(InterimDBConnString);
                SqlCommand cmd = new SqlCommand("SELECT * FROM PublicationSubscriptionData WHERE GroupID = " + SelectedGroupID + "  AND APPDATE IS NULL ", conn); 
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                
                conn.Open();
                cmd.Connection = conn;

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dr.Read())
                {
                    string FileName = String.Format("{0}\\{1}.txt", folderBrowserDialog1.SelectedPath, SelectedGroupName.Replace(" ", "_"));

                    if (File.Exists(FileName))
                        File.Delete(FileName);

                    StreamWriter sw = new StreamWriter(FileName);

                    var columns = new List<string>();

                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        columns.Add(dr.GetName(i));
                        newline += dr.GetName(i) + "\t";
                    }

                    sw.WriteLine(newline);

                    while (dr.Read())
                    {

                        if (count % 1000 == 0)
                        {
                            backgroundWorker1.ReportProgress(count, "* Writing " + count.ToString() + " records");
                        }
                        count++;

                        newline = string.Empty;

                        foreach (string s in columns)
                        {
                            newline += dr[s].ToString() + "\t";
                        }

                        sw.WriteLine(newline);
                    }

                    conn.Close();

                    backgroundWorker1.ReportProgress(count, "* Writing " + count.ToString() + " records");
                    sw.Close();

                    backgroundWorker1.ReportProgress(count, " ");

                    backgroundWorker1.ReportProgress(count, "* Updating Database......");
                    cmd = new SqlCommand("UPDATE PublicationSubscriptionData set APPDATE=getdate() WHERE GroupID = @GroupID AND APPDATE IS NULL ", conn);
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@GroupID", SelectedGroupID);

                    conn.Open();
                    cmd.Connection = conn;

                    cmd.ExecuteNonQuery();

                    ;
                    backgroundWorker1.ReportProgress(count, "* Export Completed");

                }
                conn.Close();

                MessageBox.Show("Export Complete.");
            }
            catch (Exception ex)
            {
                string exx = ex.ToString();
                backgroundWorker1.ReportProgress(count++, "[ERROR]: " + exx);
                MessageBox.Show("[ERROR]: " + exx);
            }
        }

        private void btnFileLocation_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                txtFolderLocation.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void InterimDataExport_Load(object sender, EventArgs e)
        {

        }
    }
}
