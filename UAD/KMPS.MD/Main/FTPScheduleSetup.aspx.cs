using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;

namespace KMPS.MD.Main
{
    public partial class FTPScheduleSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                LoadAvailableExportFields();
                ResetAll();
            }
        }

        private void LoadGrid()
        {
            gvScheduledExports.DataSource = KMPS.MD.Objects.ScheduledExports.GetAllForGrid();
            gvScheduledExports.DataBind();

        }

        private void LoadAvailableFilters()
        {
            lstSourceFields.DataSource = KMPS.MD.Objects.MDFilter.GetAll(Master.clientconnections);
            lstSourceFields.DataBind();
        }

        private void LoadAvailableExportFields()
        {

            lbExport.DataSource = KMPS.MD.Objects.ScheduledFields.GetAvailableFieldsForList();
            lbExport.DataBind();
        }

        protected void btnTestNow_Click(object sender, EventArgs e)
        {
            //test ftp connection
            try
            {

                string userName = txtUser.Text;
                string password = txtPass.Text;
                string server = txtHost.Text;

                StringBuilder result = new StringBuilder();
                FtpWebRequest reqFTP;

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(server));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(userName,
                                                            password);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response
                                                .GetResponseStream());

                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                // to remove the trailing '\n'
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                lblTestResult.Text = "Success!";
            }
            catch (Exception)
            {
                lblTestResult.Text = "Fail!";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //save
            ResetAll();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ResetAll();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstSourceFields.Items.Count; i++)
            {
                if (lstSourceFields.Items[i].Selected)
                {
                    lstDestFields.Items.Add(lstSourceFields.Items[i]);
                }
            }
        }

        protected void btnremove_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstDestFields.Items.Count; i++)
            {
                if (lstDestFields.Items[i].Selected)
                {
                    lstDestFields.Items.RemoveAt(i);
                    i--;
                }
            }
        }

        protected void rblScheduleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetSchedule();
        }

        private void ResetSchedule()
        {
            cbSun.Checked = false;
            cbMon.Checked = false;
            cbTue.Checked = false;
            cbWed.Checked = false;
            cbThurs.Checked = false;
            cbFri.Checked = false;
            cbSat.Checked = false;
            txtMonthlyDate.Text = string.Empty;
            if (rblScheduleType.Items[0].Selected)
            {
                pnlDaily.Visible = true;
                pnlMonthly.Visible = false;
            }
            else
            {
                pnlDaily.Visible = false;
                pnlMonthly.Visible = true;
            }
        }

        private void ResetAll()
        {
            lstDestFields.Items.Clear();
            lbExport.ClearSelection();

            txtSchedName.Text = string.Empty;
            txtHost.Text = string.Empty;
            txtUser.Text = string.Empty;
            txtPass.Text = string.Empty;

            ResetSchedule();
            LoadAvailableFilters();
            LoadGrid();
        }
    }
}