using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.lists
{
    public partial class channelMasterSuppression : ECN_Framework.WebPageHelper
    {
        private const string XmlVersionEncodingIsoXml = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>";
        private const string ClosingXmlTag = "</XML>";
        private const string EaOpeningTag = "<ea>";
        private const string EaClosingTag = "</ea>";
        private const string CarriageReturnNewLine = "\r\n";
        private const string ActionColumnName = "Action";
        private const string SortOrderColumnName = "sortOrder";
        private const string ActionNew = "New";
        private const string ActionChanged = "Changed";
        private const string ActionSkipped = "Skipped";
        private const string ActionDuplicates = "Duplicate(s)";
        private const string Nbsp = "&nbsp;";
        private const string TotalsColumnName = "Totals";
        private const string ActionTimeToImport = "Time to Import";
        private const string ActionTotalRecordsInTheFile = "Total Records in the File";
        private const string KeyTotalRecords = "T";
        private const string KeyNew = "I";
        private const string KeyChanged = "U";
        private const string KeyDuplicates = "D";
        private const string KeySkipped = "S";
        private const int SortOrderTotal = 1;
        private const int SortOrderNew = 2;
        private const int SortOrderChanged = 3;
        private const int SortOrderDuplicates = 4;
        private const int SortOrderSkipped = 5;
        private const int SortOrderNbsp = 8;
        private const int SortOrderTimeToImport = 9;
        private const string SortOrderAsc = "sortorder asc";
        private const string CountsColumnName = "Counts";
        private const string MessageLabelFormatText = "<font face=verdana size=2 color=#000000>&nbsp;{0} rows updated/inserted </font>";

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
            Master.Heading = "Channel Master Suppressed Emails";
            Master.HelpContent = "";
            Master.HelpTitle = "Groups Manager";	

            //if (!KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            if (KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
                Response.Redirect("../default.aspx");
            }
        }

        protected void Page_PreRender(object sender, System.EventArgs e) 
        {
            loadEmailsGrid();
        }

        public void addEmailBTN_Click(object sender, EventArgs eventArgs)
        {
            // TODO: update emailsAdded counter
            const int emailsAdded = 0;
            var emailAddressToAdd = emailAddresses.Text;
           
            var startDateTime = DateTime.Now;
            var emailRecordsDt = new DataTable();

            var hashTableUpdateRecords = new Hashtable();
            if (emailAddressToAdd.Length > 0) 
            {
                var xmlInsert = new StringBuilder();
                xmlInsert.Append(XmlVersionEncodingIsoXml);

                emailAddressToAdd = emailAddressToAdd.Replace(CarriageReturnNewLine, ",");
                var stringTokenizer = new StringTokenizer(emailAddressToAdd, ',');

                while (stringTokenizer.HasMoreTokens())
                {
                    xmlInsert.Append($"{EaOpeningTag}{stringTokenizer.NextToken().Trim()}{EaClosingTag}");
                }

                xmlInsert.Append(ClosingXmlTag);

                try
                {
                    emailRecordsDt = EmailGroup.ImportEmailsToCS(Master.UserSession.CurrentUser, Master.UserSession.CurrentBaseChannel.BaseChannelID, xmlInsert.ToString());
                }
                catch (ECNException ex)
                {
                    setECNError(ex);
                }

                if (emailRecordsDt.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in emailRecordsDt.Rows)
                    {
                        if (!hashTableUpdateRecords.Contains(dataRow[ActionColumnName].ToString()))
                        {
                            hashTableUpdateRecords.Add(dataRow[ActionColumnName].ToString().ToUpper(), Convert.ToInt32(dataRow[CountsColumnName]));
                        }
                        else 
                        {
                            var total = Convert.ToInt32(hashTableUpdateRecords[dataRow[ActionColumnName].ToString().ToUpper()]);
                            hashTableUpdateRecords[dataRow[ActionColumnName].ToString().ToUpper()] = total + Convert.ToInt32(dataRow[CountsColumnName]);
                        }
                    }
                }

                if (hashTableUpdateRecords.Count > 0)
                {
                    var updateRecords = GetUpdateRecords(hashTableUpdateRecords, startDateTime);
                    SetResultsGrid(updateRecords);
                }
            } 
            else 
            {
                ResultsGrid.Visible = false;
                MessageLabel.Visible = true;
                MessageLabel.Text = string.Format(MessageLabelFormatText, emailsAdded);
            }
        }

        private void SetResultsGrid(DataTable updateRecords)
        {
            var defaultView = updateRecords.DefaultView;
            defaultView.Sort = SortOrderAsc;

            ResultsGrid.DataSource = defaultView;
            ResultsGrid.DataBind();
            ResultsGrid.Visible = true;
            importResultsPNL.Visible = true;
            MessageLabel.Visible = false;
        }

        private static DataTable GetUpdateRecords(IEnumerable updatedRecords, DateTime startDateTime)
        {
            var records = new DataTable();
            records.Columns.Add(ActionColumnName);
            records.Columns.Add(TotalsColumnName);
            records.Columns.Add(SortOrderColumnName);

            DataRow row;

            foreach (DictionaryEntry de in updatedRecords)
            {
                row = records.NewRow();
                switch (de.Key.ToString())
                {
                    case KeyTotalRecords:
                        row[ActionColumnName] = ActionTotalRecordsInTheFile;
                        row[SortOrderColumnName] = SortOrderTotal;
                        break;
                    case KeyNew:
                        row[ActionColumnName] = ActionNew;
                        row[SortOrderColumnName] = SortOrderNew;
                        break;
                    case KeyChanged:
                        row[ActionColumnName] = ActionChanged;
                        row[SortOrderColumnName] = SortOrderChanged;
                        break;
                    case KeyDuplicates:
                        row[ActionColumnName] = ActionDuplicates;
                        row[SortOrderColumnName] = SortOrderDuplicates;
                        break;
                    case KeySkipped:
                        row[ActionColumnName] = ActionSkipped;
                        row[SortOrderColumnName] = SortOrderSkipped;
                        break;
                }

                row[TotalsColumnName] = de.Value;
                records.Rows.Add(row);
            }

            row = records.NewRow();
            row[ActionColumnName] = Nbsp;
            row[TotalsColumnName] = " ";
            row[SortOrderColumnName] = SortOrderNbsp;
            records.Rows.Add(row);

            var duration = DateTime.Now - startDateTime;

            row = records.NewRow();
            row[ActionColumnName] = ActionTimeToImport;
            row[TotalsColumnName] = $"{duration.Hours}:{duration.Minutes}:{duration.Seconds}";
            row[SortOrderColumnName] = SortOrderTimeToImport;
            records.Rows.Add(row);

            return records;
        }

        protected void exportEmailsBTN_Click(object sender, EventArgs e) 
        {
            string newline = "";
            string clientID = Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString();
            string fileName = clientID + "_MasterSuppressed_Emails.CSV";

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

            List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> channelMasterSuppressionList = new List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList>();

            if (EmailsTXT.Text.Length > 0)
            {
                channelMasterSuppressionList = ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.GetByEmailAddress(Master.UserSession.CurrentBaseChannel.BaseChannelID, EmailsTXT.Text.Replace("'", "''"), Master.UserSession.CurrentUser);
            }
            else
            {
                channelMasterSuppressionList = ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.GetByBaseChannelID(Master.UserSession.CurrentBaseChannel.BaseChannelID, Master.UserSession.CurrentUser);
            }
            var result = (from src in channelMasterSuppressionList
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
            string sql = string.Empty;
            List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> channelMasterSuppressionList = new List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList>();
            if (EmailsTXT.Text.Length > 0)
            {
                channelMasterSuppressionList = ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.GetByEmailAddress(Master.UserSession.CurrentBaseChannel.BaseChannelID, EmailsTXT.Text.Replace("'", "''"), Master.UserSession.CurrentUser);
            }
            else
            {
                channelMasterSuppressionList = ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.GetByBaseChannelID(Master.UserSession.CurrentBaseChannel.BaseChannelID, Master.UserSession.CurrentUser);
            }
            var result = (from src in channelMasterSuppressionList
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
                deleteBtn.Attributes.Add("onclick", "return confirm('Email Address \"" + e.Item.Cells[0].Text.ToString().Replace("'", "") + "\" will be removed from the Master Suppression List!" + "\\n" + "This process will enable \"" + e.Item.Cells[0].Text.ToString().Replace("'", "") + "\" to start receiving the campaigns that you have scheduled / will be sending in the future." + "\\n" + "\\n" + "Are you sure you want to continue? This process CANNOT be undone.');");
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
                        ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.Delete(Master.UserSession.CurrentBaseChannel.BaseChannelID, e.CommandArgument.ToString().Replace("'", "''"), Master.UserSession.CurrentUser);
                    }
                    catch (ECNException ex)
                    {
                        setECNError(ex);
                    }
                    break;
            }
        }

        protected void searchEmailsBTN_Click(object sender, EventArgs e)
        {
            loadEmailsGrid();
        }
    }

}
