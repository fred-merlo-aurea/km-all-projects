using System;
using System.Text;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Data;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.lists
{
    public partial class channelNoThresholdList : ECN_Framework.WebPageHelper
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
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS; 
            Master.SubMenu = "";
            Master.Heading = "Channel No Threshold Emails";
            Master.HelpContent = "";
            Master.HelpTitle = "Groups Manager";	

            if (!(KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser)))
            {
                Response.Redirect("../default.aspx");
            }
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            loadEmailsGrid();
        }

        public void addEmailBTN_Click(object sender, System.EventArgs e)
        {

            int emailsAdded = 0;
            string emailAddressToAdd = emailAddresses.Text;
            StringBuilder xmlInsert = new StringBuilder();
            xmlInsert.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>");
            DateTime startDateTime = DateTime.Now;

            Hashtable hUpdatedRecords = new Hashtable();

            if (emailAddressToAdd.Length > 0)
            {
                emailAddressToAdd = emailAddressToAdd.Replace("\r\n", ",");
                StringTokenizer st = new StringTokenizer(emailAddressToAdd, ',');

                while (st.HasMoreTokens())
                {
                    xmlInsert.Append("<ea>" + st.NextToken().Trim() + "</ea>");
                }

                xmlInsert.Append("</XML>");

                DataTable emailRecordsDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmailsToNoThreshold(Master.UserSession.CurrentUser, Master.UserSession.CurrentBaseChannel.BaseChannelID, xmlInsert.ToString());
                if (emailRecordsDT.Rows.Count > 0)
                {
                    foreach (DataRow dr in emailRecordsDT.Rows)
                    {
                        if (!hUpdatedRecords.Contains(dr["Action"].ToString()))
                            hUpdatedRecords.Add(dr["Action"].ToString().ToUpper(), Convert.ToInt32(dr["Counts"]));
                        else
                        {
                            int eTotal = Convert.ToInt32(hUpdatedRecords[dr["Action"].ToString().ToUpper()]);
                            hUpdatedRecords[dr["Action"].ToString().ToUpper()] = eTotal + Convert.ToInt32(dr["Counts"]);
                        }
                    }
                }

                if (hUpdatedRecords.Count > 0)
                {
                    DataTable dtRecords = new DataTable();

                    dtRecords.Columns.Add("Action");
                    dtRecords.Columns.Add("Totals");
                    dtRecords.Columns.Add("sortOrder");

                    DataRow row;

                    foreach (DictionaryEntry de in hUpdatedRecords)
                    {
                        row = dtRecords.NewRow();

                        if (de.Key.ToString() == "T")
                        {
                            row["Action"] = "Total Records in the File";
                            row["sortOrder"] = 1;
                        }
                        else if (de.Key.ToString() == "I")
                        {
                            row["Action"] = "New";
                            row["sortOrder"] = 2;
                        }
                        else if (de.Key.ToString() == "U")
                        {
                            row["Action"] = "Changed";
                            row["sortOrder"] = 3;
                        }
                        else if (de.Key.ToString() == "D")
                        {
                            row["Action"] = "Duplicate(s)";
                            row["sortOrder"] = 4;
                        }
                        else if (de.Key.ToString() == "S")
                        {
                            row["Action"] = "Skipped";
                            row["sortOrder"] = 5;
                        }
                        row["Totals"] = de.Value;
                        dtRecords.Rows.Add(row);
                    }

                    row = dtRecords.NewRow();
                    row["Action"] = "&nbsp;";
                    row["Totals"] = " ";
                    row["sortOrder"] = 8;
                    dtRecords.Rows.Add(row);

                    TimeSpan duration = DateTime.Now - startDateTime;

                    row = dtRecords.NewRow();
                    row["Action"] = "Time to Import";
                    row["Totals"] = duration.Hours + ":" + duration.Minutes + ":" + duration.Seconds;
                    row["sortOrder"] = 9;
                    dtRecords.Rows.Add(row);

                    DataView dv = dtRecords.DefaultView;
                    dv.Sort = "sortorder asc";

                    ResultsGrid.DataSource = dv;
                    ResultsGrid.DataBind();
                    ResultsGrid.Visible = true;
                    importResultsPNL.Visible = true;
                    MessageLabel.Visible = false;
                }

            }
            else
            {
                ResultsGrid.Visible = false;
                MessageLabel.Visible = true;
                MessageLabel.Text = "<font face=verdana size=2 color=#000000>&nbsp;" + emailsAdded.ToString() + " rows updated/inserted </font>";
            }
        }

        protected void exportEmailsBTN_Click(object sender, EventArgs e)
        {
            string newline = "";
            string clientID = Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString();
            string fileName = clientID + "_NoThreshold_Emails.CSV";

            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + clientID + "/downloads/");
            if (!Directory.Exists(txtoutFilePath))
                Directory.CreateDirectory(txtoutFilePath);

            DateTime date = DateTime.Now;
            string outfileName = txtoutFilePath + fileName;

            if (File.Exists(outfileName))
            {
                File.Delete(outfileName);
            }

            TextWriter txtfile = File.AppendText(outfileName);
            txtfile.WriteLine("EmailAddress, DateAdded");

            List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> channelNoThresholdList = ECN_Framework_BusinessLayer.Communicator.ChannelNoThresholdList.GetByBaseChannelID(Master.UserSession.CurrentBaseChannel.BaseChannelID, Master.UserSession.CurrentUser);
            var result = (from src in channelNoThresholdList
                          orderby src.EmailAddress
                          group src by src.EmailAddress into g
                          select new
                          {
                              EmailAddress = g.Key,
                              DateAdded = g.Max(t => t.CreatedDate)
                          }).ToList();

            if (result.Count > 0)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    newline = "";
                    newline += result[i].EmailAddress + ", " + result[i].DateAdded.ToString();
                    txtfile.WriteLine(newline);
                }
            }
            txtfile.Close();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.WriteFile(outfileName);
            Response.Flush();
            Response.End();
        }

        private void loadEmailsGrid()
        {
            List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> channelNoThresholdList = ECN_Framework_BusinessLayer.Communicator.ChannelNoThresholdList.GetByBaseChannelID(Master.UserSession.CurrentBaseChannel.BaseChannelID, Master.UserSession.CurrentUser);
            var result = (from src in channelNoThresholdList
                          orderby src.EmailAddress
                          group src by src.EmailAddress into g
                          select new
                          {
                              EmailAddress = g.Key,
                              DateAdded = g.Max(t => t.CreatedDate)
                          }).ToList();

            if (result.Count > 0)
            {
                exportEmailsBTN.Visible = true;
                emailsGrid.DataSource = result;
                emailsGrid.DataBind();
                emailsPager.RecordCount = result.Count;
            }
            else
            {
                exportEmailsBTN.Visible = false;
            }
        }

        protected void emailsGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                LinkButton deleteBtn = e.Item.FindControl("deleteEmailBTN") as LinkButton;
                deleteBtn.Attributes.Add("onclick", "return confirm('Email Address \"" + e.Item.Cells[0].Text.ToString().Replace("'", "") + "\" will be removed from the No Threshold List!!" + "\\n" + "This process will enable \"" + e.Item.Cells[0].Text.ToString().Replace("'", "") + "\" to be suppressed by Threshold Suppression for campaigns you have scheduled / will be sending in the future." + "\\n" + "\\n" + "Are you sure you want to contine?This process CANNOT be undone.');");
            }
            return;
        }

        protected void emailsGrid_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            switch (e.CommandName.ToUpper())
            {
                case "DELETEEMAIL":
                     try
                    {
                        ECN_Framework_BusinessLayer.Communicator.ChannelNoThresholdList.Delete(Master.UserSession.CurrentBaseChannel.BaseChannelID, e.CommandArgument.ToString().Replace("'", "''"), Master.UserSession.CurrentUser);
                    }
                    catch (ECNException ex)
                    {
                        setECNError(ex);
                    }
                    break;
            }
        }
        
    }
}
