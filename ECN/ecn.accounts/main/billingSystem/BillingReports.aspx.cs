using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Entities.Accounts;
using Customer = ECN_Framework_BusinessLayer.Accounts.Customer;

namespace ecn.accounts.main.billingSystem
{
    public partial class BillingReports : System.Web.UI.Page
    {
        private const string ConfigImagesVirtualPath = "Images_VirtualPath";
        private const string ReportPathFormat = "{0}/customers/{1}/downloads/";
        private const string ReportFileFormat = "BillingReport_{0}_{1}.xls";
        private const string DateFormat = "MM-dd-yyyy";
        private const string ContentTypeForExcel = "application/cnd.ms-excel";
        private const string HeaderContentDisposition = "content-disposition";
        private const string AttachementFormat = "attachment; filename={0}";
        private const string False = "false";
        private const string True = "true";
        private const string All = "all";
        private const string ColumnId = "ID";
        private const string ColumnIsDeleted = "IsDeleted";
        private const string ColumnAmount = "Amount";
        private const string ColumnItemName = "ItemName";
        private const string ColumnSendTime = "sendtime";
        private const string ColumnBlastField1 = "blastfield1";
        private const string ColumnBlastField2 = "blastfield2";
        private const string ColumnBlastField3 = "blastfield3";
        private const string ColumnBlastField4 = "blastfield4";
        private const string ColumnBlastField5 = "blastfield5";
        private const string ColumnFromMail = "fromemail";
        private const string ColumnFromName = "fromname";
        private const string ColumnEmailSubject = "emailsubject";
        private const string ColumnGroupName = "groupname";
        private const string BillingReportPage = "BillingReports.aspx";

        public static DataTable dtFlatRateItems;
        private static int _BillingReportID;
        private int BillingReportID
        {
            get
            {
                try
                {
                    int.TryParse(Request.QueryString["BillingReportID"].ToString(), out _BillingReportID);
                    return _BillingReportID;
                }
                catch
                {
                    return -1;
                }
            }
            set
            {
                _BillingReportID = value;
            }
        }
        private static ECN_Framework_Entities.Accounts.BillingReport currentReport = new ECN_Framework_Entities.Accounts.BillingReport();
        protected void Page_Load(object sender, EventArgs e)
        {

            phError.Visible = false;
            if (!Page.IsPostBack)
            {
                dtFlatRateItems = new DataTable();
                dtFlatRateItems.Columns.Add("ID");
                dtFlatRateItems.Columns.Add("ItemName");
                dtFlatRateItems.Columns.Add("Amount");
                dtFlatRateItems.Columns.Add("IsDeleted");

                if (!KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                {
                    Response.Redirect("~/main/securityAccessError.aspx");
                }

                LoadLists();
                if (BillingReportID > 0)
                {
                    pnlReportList.Visible = false;
                    pnlEditReport.Visible = true;
                    LoadReportDetails(BillingReportID);
                }
                else
                {
                    pnlReportList.Visible = true;
                    pnlEditReport.Visible = false;
                    LoadSavedReports();
                }

            }

        }

        private void LoadSavedReports()
        {

            List<ECN_Framework_Entities.Accounts.BillingReport> listBR = ECN_Framework_BusinessLayer.Accounts.BillingReport.GetALL();
            if (listBR.Count > 0)
            {
                gvBillingReports.DataSource = listBR;
                gvBillingReports.DataBind();
                lblNoReports.Visible = false;
            }
            else
            {
                lblNoReports.Visible = true;
            }
        }

        private void LoadLists()
        {
            List<ECN_Framework_Entities.Accounts.BaseChannel> lstBaseChannel = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();
            var result = (from src in lstBaseChannel
                          orderby src.BaseChannelName
                          select new
                          {
                              BaseChannelID = src.BaseChannelID,
                              BaseChannelName = src.BaseChannelName
                          }).ToList();

            ddlBaseChannel.DataSource = result;
            ddlBaseChannel.DataTextField = "BaseChannelName";
            ddlBaseChannel.DataValueField = "BaseChannelID";
            ddlBaseChannel.DataBind();
            ddlBaseChannel.Items.FindByValue(Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString()).Selected = true;

            LoadCustomers();
        }

        private void LoadReportDetails(int billingReportID)
        {
            currentReport = ECN_Framework_BusinessLayer.Accounts.BillingReport.GetByBillingReportID(billingReportID);

            txtFromEmail.Text = currentReport.FromEmail;
            txtFromName.Text = currentReport.FromName;

            txtReportName.Text = currentReport.BillingReportName;
            txtSubject.Text = currentReport.Subject;
            txtToEmail.Text = currentReport.ToEmail;

            if (currentReport.EndDate.HasValue)
            {
                txtRunToDate.Text = currentReport.EndDate.Value.ToShortDateString();
            }
            if (currentReport.StartDate.HasValue)
            {
                txtRunFromDate.Text = currentReport.StartDate.Value.ToShortDateString();
            }

            List<ECN_Framework_Entities.Accounts.BillingReportItem> billItems = ECN_Framework_BusinessLayer.Accounts.BillingReportItem.GetItemsByBillingReportID(billingReportID).Where(x => x.IsFlatRateItem == true).ToList();

            if (billItems.Count > 0)
            {
                foreach (ECN_Framework_Entities.Accounts.BillingReportItem bri in billItems)
                {
                    if (bri.IsDeleted == false)
                    {
                        DataRow dr = dtFlatRateItems.NewRow();
                        dr["ID"] = bri.BillingItemID.ToString();
                        dr["ItemName"] = bri.InvoiceText.ToString();
                        dr["Amount"] = bri.Amount.Value.ToString();
                        dr["IsDeleted"] = "false";

                        dtFlatRateItems.Rows.Add(dr);
                    }
                }
                gvFlatRateItems.DataSource = dtFlatRateItems;
                gvFlatRateItems.DataBind();
            }

            ddlBaseChannel.SelectedValue = currentReport.BaseChannelID.Value.ToString();
            LoadCustomers();

            if (currentReport.AllCustomers.HasValue && currentReport.AllCustomers.Value)
            {
                rblCustomer.SelectedValue = "all";
                lstbxCustomers.Visible = false;
            }
            else if (!currentReport.AllCustomers.HasValue)
            {
                rblCustomer.SelectedValue = "all";
                lstbxCustomers.Visible = false;
            }
            else
            {
                rblCustomer.SelectedValue = "select";
                lstbxCustomers.Visible = true;
                string[] custIDs = currentReport.CustomerIDs.Split(',');
                foreach (ListItem li in lstbxCustomers.Items)
                {
                    if (custIDs.Contains(li.Value))
                    {
                        li.Selected = true;
                    }
                    else
                        li.Selected = false;
                }
            }

            string[] blastFields = currentReport.BlastFields.Split(',');
            foreach (ListItem li in lstbxBlastColumns.Items)
            {
                if (blastFields.Contains(li.Value))
                    li.Selected = true;
                else
                    li.Selected = false;
            }

            //if (currentReport.RecurrenceType != null && currentReport.RecurrenceType != string.Empty)
            //{
            //    pnlRecurrence.Visible = true;
            //    pnlRecurring.Visible = true;
            //    pnlOneTime.Visible = false;
            //    ddlScheduleType.SelectedValue = "Recurring";
            //    ddlRecurrence.SelectedValue = currentReport.RecurrenceType.ToString();
            //    txtRecurringStartDate.Text = currentReport.StartDate.ToShortDateString();
            //    //ddlRecurringStartTime.SelectedValue = currentReport.StartTime;
            //    txtRecurringEndDate.Text = currentReport.EndDate == null ? "" : currentReport.EndDate.ToShortDateString();

            //    switch (ddlRecurrence.SelectedValue)
            //    {
            //        case "Monthly":
            //            pnlMonth.Visible = true;
            //            break;
            //        case "Quarterly":
            //            pnlMonth.Visible = true;
            //            break;
            //        case "Yearly":

            //            break;
            //        default:
            //            break;
            //    }

            //}


        }

        private void LoadCustomers()
        {
            List<ECN_Framework_Entities.Accounts.Customer> lstCustomers = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(Convert.ToInt32(ddlBaseChannel.SelectedValue.ToString()));

            if (lstCustomers.Count > 0)
            {
                var cust = (from src in lstCustomers
                            orderby src.CustomerName
                            select new
                            {
                                CustomerID = src.CustomerID,
                                CustomerName = src.CustomerName
                            }).ToList();

                lstbxCustomers.DataSource = cust;
                lstbxCustomers.DataTextField = "CustomerName";
                lstbxCustomers.DataValueField = "CustomerID";
                lstbxCustomers.DataBind();
            }
            else
            {
                lstbxCustomers.Items.Clear();

            }
        }

        protected void rblCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblCustomer.SelectedValue.Equals("all"))
            {
                lstbxCustomers.Visible = false;
            }
            else
            {
                lstbxCustomers.Visible = true;
            }
        }

        protected void ddlScheduleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlScheduleType.SelectedValue)
            {
                case "One-Time":
                    pnlOneTime.Visible = true;
                    pnlRecurrence.Visible = false;
                    pnlRecurring.Visible = false;
                    pnlMonth.Visible = false;
                    pnlDays.Visible = false;
                    resetRecurringControls();
                    break;
                case "Recurring":
                    pnlOneTime.Visible = false;
                    pnlRecurrence.Visible = true;
                    pnlRecurring.Visible = true;
                    resetOneTimeControls();
                    break;
                default:
                    break;
            }
        }

        private void resetRecurringControls()
        {
            ddlRecurrence.ClearSelection();
            txtRecurringStartDate.Text = "";
            ddlRecurringStartTime.ClearSelection();
            txtRecurringEndDate.Text = "";
            cbDays.ClearSelection();
            txtMonth.Text = "";
            cbLastDay.Checked = false;
        }

        protected void ddlRecurrence_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlRecurrence.SelectedValue)
            {
                case "Daily":
                    pnlDays.Visible = false;
                    pnlMonth.Visible = false;
                    cbDays.ClearSelection();
                    txtMonth.Text = "";
                    cbLastDay.Checked = false;
                    break;
                case "Weekly":
                    pnlDays.Visible = true;
                    pnlMonth.Visible = false;
                    txtMonth.Text = "";
                    cbLastDay.Checked = false;
                    break;
                case "Monthly":
                    pnlDays.Visible = false;
                    pnlMonth.Visible = true;
                    cbDays.ClearSelection();
                    break;
                default:
                    break;
            }
        }

        private void resetOneTimeControls()
        {
            txtStartDate.Text = "";
            ddlStartTime.ClearSelection();
        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        protected void btnSaveFlatRateItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfIsEdit.Value.Trim()))
            {

                decimal amount = 0.0M;
                decimal.TryParse(txtItemRate.Text, out amount);


                DataRow dr = dtFlatRateItems.NewRow();
                dr["ID"] = Guid.NewGuid();
                dr["Amount"] = amount.ToString();
                dr["ItemName"] = txtItemName.Text.Trim();
                dr["IsDeleted"] = "false";

                dtFlatRateItems.Rows.Add(dr);
            }
            else if (!string.IsNullOrEmpty(hfIsEdit.Value.Trim()))
            {
                DataRow dr = dtFlatRateItems.AsEnumerable().First(x => x.Field<string>("ID") == hfIsEdit.Value.ToString());

                decimal amount = 0.0M;
                decimal.TryParse(txtItemRate.Text, out amount);

                dr["Amount"] = amount.ToString();
                dr["ItemName"] = txtItemName.Text.Trim();
            }
            gvFlatRateItems.DataSource = dtFlatRateItems.AsEnumerable().Where(x => x.Field<string>("IsDeleted") == "false").AsDataView().ToTable();
            gvFlatRateItems.DataBind();

            modalPopupFlatRate.Hide();
        }

        protected void btnRunReport_Click(object sender, EventArgs e)
        {
            try
            {
                int baseChannelId;
                DateTime startDate;
                DateTime endDate;

                int.TryParse(ddlBaseChannel.SelectedValue, out baseChannelId);
                DateTime.TryParse(txtRunFromDate.Text, out startDate);
                DateTime.TryParse(txtRunToDate.Text, out endDate);

                var listBillingItems = new List<BillingReportItem>();

                AddFlatRateItems(listBillingItems, baseChannelId);

                // Get selected customer Ids
                var customerIds = GetSelectedCustomerIds(baseChannelId);

                // Get selected columns for report
                string columnsString;
                var columns = GetSelectedColumns(out columnsString);

                // Get the report data
                if (customerIds.Length > 0)
                {
                    listBillingItems.AddRange(ECN_Framework_BusinessLayer.Accounts.BillingReportItem.GetEmailUsageByCustomer(customerIds, startDate, endDate, columnsString, BuildColumnSQL(columns)));
                }

                // Write to file
                var osFilePath = Server.MapPath(string.Format(ReportPathFormat, ConfigurationManager.AppSettings[ConfigImagesVirtualPath], Master.UserSession.CurrentCustomer.CustomerID));
                var tfile = string.Format(ReportFileFormat, startDate.ToString(DateFormat), endDate.ToString(DateFormat));
                var outfileName = osFilePath + tfile;

                WriteFile(listBillingItems, columnsString, osFilePath, outfileName);

                // Return attachment in response
                Response.ContentType = ContentTypeForExcel;
                Response.AddHeader(HeaderContentDisposition, string.Format(AttachementFormat, tfile));
                Response.WriteFile(outfileName);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
                phError.Visible = true;
            }
        }
        
        private void AddFlatRateItems(List<BillingReportItem> listBillingItems, int baseChannelId)
        {
            if (dtFlatRateItems != null)
            {
                // Get report flat rate data
                foreach (DataRow dr in dtFlatRateItems.Rows)
                {
                    if (dr[ColumnId].ToString().Contains("-") && dr[ColumnIsDeleted].ToString() == False)
                    {
                        decimal amount;
                        decimal.TryParse(dr[ColumnAmount].ToString(), out amount);

                        var reportItem = new BillingReportItem
                        {
                            IsFlatRateItem = true,
                            Amount = amount,
                            BaseChannelID = baseChannelId,
                            BaseChannelName = ddlBaseChannel.SelectedItem.Text,
                            InvoiceText = dr[ColumnItemName].ToString()
                        };

                        listBillingItems.Add(reportItem);
                    }
                }
            }
        }

        private string GetSelectedCustomerIds(int baseChannelId)
        {
            IEnumerable<string> customerIds;
            if (rblCustomer.SelectedValue.Equals(All, StringComparison.OrdinalIgnoreCase))
            {
                customerIds = Customer.GetByBaseChannelID(baseChannelId)
                    .Select(c => c.CustomerID.ToString());
            }
            else
            {
                customerIds = lstbxCustomers.Items
                    .Cast<ListItem>()
                    .Where(l => l.Selected)
                    .Select(l => l.Value);
            }

            return string.Join(",", customerIds);
        }

        private List<string> GetSelectedColumns(out string columnsString)
        {
            var columns = new List<string>();
            var columnsBuilder = new StringBuilder();
            foreach (ListItem listItem in lstbxBlastColumns.Items)
            {
                if (listItem.Selected)
                {
                    columns.Add(listItem.Value);
                    if (listItem.Value.Equals(ColumnSendTime))
                    {
                        columnsBuilder.AppendFormat("\tb.{0}", listItem.Value); 
                    }
                    else
                    {
                        columnsBuilder.AppendFormat("\t{0}", listItem.Value);
                    }
                }
            }

            columnsString = columnsBuilder.ToString();
            return columns;
        }

        private static void WriteFile(List<BillingReportItem> listBillingItems, string columnsString, string osFilePath, string outfileName)
        {
            if (!Directory.Exists(osFilePath))
            {
                Directory.CreateDirectory(osFilePath);
            }

            if (File.Exists(outfileName))
            {
                File.Delete(outfileName);
            }

            using (var txtfile = File.AppendText(outfileName))
            {
                if (listBillingItems.Any(x => x.IsFlatRateItem))
                {
                    txtfile.WriteLine("BaseChannel\tCustomerName" + columnsString + "\tSendTime\tFlatRateItem\tSendTotal");
                }
                else
                {
                    txtfile.WriteLine("BaseChannel\tCustomerName" + columnsString + "\tSendTime\tSendTotal");
                }

                foreach (var reportItem in listBillingItems)
                {
                    var newline = new StringBuilder();
                    newline.Append(reportItem.BaseChannelName + "\t");
                    newline.Append(reportItem.CustomerName + "\t");

                    foreach (var s in columnsString.Split('\t'))
                    {
                        switch (s.ToLower())
                        {
                            case ColumnBlastField1:
                                newline.Append(reportItem.BlastField1 + "\t");
                                break;
                            case ColumnBlastField2:
                                newline.Append(reportItem.BlastField2 + "\t");
                                break;
                            case ColumnBlastField3:
                                newline.Append(reportItem.BlastField3 + "\t");
                                break;
                            case ColumnBlastField4:
                                newline.Append(reportItem.BlastField4 + "\t");
                                break;
                            case ColumnBlastField5:
                                newline.Append(reportItem.BlastField5 + "\t");
                                break;
                            case ColumnFromMail:
                                newline.Append(reportItem.FromEmail + "\t");
                                break;
                            case ColumnFromName:
                                newline.Append(reportItem.FromName + "\t");
                                break;
                            case ColumnEmailSubject:
                                newline.Append(reportItem.EmailSubject + "\t");
                                break;
                            case ColumnGroupName:
                                newline.Append(reportItem.GroupName + "\t");
                                break;
                            default:
                                break;
                        }
                    }

                    newline.Append(reportItem.SendTime + "\t");
                    newline.Append(reportItem.IsFlatRateItem ? reportItem.InvoiceText + "\t" : "");
                    newline.Append(reportItem.AmountOfItems.HasValue ? reportItem.AmountOfItems.Value + "\t" : "\t");
                    newline.Append(reportItem.IsFlatRateItem ? reportItem.Amount.ToString() : "");

                    txtfile.WriteLine(newline.ToString().TrimEnd('\t'));
                }

                txtfile.Close();
            }
        }

        protected void btnSaveSchedule_Click(object sender, EventArgs e)
        {

        }

        protected void btnAddFlatItem_Click(object sender, EventArgs e)
        {
            hfIsEdit.Value = "";
            if (dtFlatRateItems == null)
            {
                dtFlatRateItems = new DataTable();
                dtFlatRateItems.Columns.Add("ID");
                dtFlatRateItems.Columns.Add("ItemName");
                dtFlatRateItems.Columns.Add("Amount");
                dtFlatRateItems.Columns.Add("IsDeleted");
            }
            txtItemName.Text = "";
            txtItemRate.Text = "";
            modalPopupFlatRate.Show();
        }

        protected void btnSaveReport_Click(object sender, EventArgs e)
        {
            //Save report
            int baseChannelId;
            int.TryParse(ddlBaseChannel.SelectedValue, out baseChannelId);

            if (currentReport.BillingReportID < 0)
            {
                currentReport = new BillingReport();
                currentReport.CreatedUserID = Master.UserSession.CurrentUser.UserID;
            }
            else
            {
                currentReport.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
            }

            currentReport.AllCustomers = rblCustomer.SelectedValue.Equals(All, StringComparison.OrdinalIgnoreCase);
            if (!currentReport.AllCustomers.Value)
            {
                currentReport.CustomerIDs = GetSelectedCustomerIds(baseChannelId);
            }
            else
            {
                currentReport.CustomerIDs = string.Empty;
            }

            var blastFields = lstbxBlastColumns.Items
                .Cast<ListItem>()
                .Where(l => l.Selected)
                .Select(l => l.Value);

            currentReport.BlastFields = string.Join(",", blastFields);
            
            currentReport.BaseChannelID = baseChannelId;
            currentReport.BillingReportName = txtReportName.Text.Trim();

            if (!string.IsNullOrWhiteSpace(txtRunToDate.Text))
            {
                DateTime endDate;
                DateTime.TryParse(txtRunToDate.Text, out endDate);
                currentReport.EndDate = endDate;
            }
            else
            {
                currentReport.EndDate = null;
            }

            if (!string.IsNullOrWhiteSpace(txtRunFromDate.Text))
            {
                DateTime startDate;
                DateTime.TryParse(txtRunFromDate.Text, out startDate);
                currentReport.StartDate = startDate;
            }
            else
            {
                currentReport.StartDate = null;
            }

            currentReport.FromEmail = txtFromEmail.Text;
            currentReport.FromName = txtFromName.Text;
            currentReport.IncludeFulfillment = false;
            currentReport.IncludeMasterFile = false;
            currentReport.IsDeleted = false;
            currentReport.IsRecurring = false;
            currentReport.Subject = txtSubject.Text;
            currentReport.ToEmail = txtToEmail.Text;

            var billingReportId = ECN_Framework_BusinessLayer.Accounts.BillingReport.SaveBillingReport(currentReport);
            
            AddFlatRateItemsForSave(billingReportId);

            Response.Redirect(BillingReportPage);
        }

        private void AddFlatRateItemsForSave(int billingReportId)
        {
            if (dtFlatRateItems != null && dtFlatRateItems.Rows.Count > 0)
            {
                foreach (DataRow dr in dtFlatRateItems.Rows)
                {
                    if (dr[ColumnId].ToString().Contains("-"))
                    {
                        if (dr[ColumnIsDeleted].ToString().Equals(False))
                        {
                            decimal amount;
                            decimal.TryParse(dr[ColumnAmount].ToString(), out amount);

                            var reportItem = new BillingReportItem
                            {
                                Amount = amount,
                                AmountOfItems = null,
                                BaseChannelID = Master.UserSession.CurrentBaseChannel.BaseChannelID,
                                BaseChannelName = Master.UserSession.CurrentBaseChannel.BaseChannelName,
                                BillingReportID = billingReportId,
                                CreatedUserID = Master.UserSession.CurrentUser.UserID,
                                CustomerID = Master.UserSession.CurrentCustomer.CustomerID,
                                CustomerName = Master.UserSession.CurrentCustomer.CustomerName,
                                IsDeleted = false,
                                IsFlatRateItem = true,
                                InvoiceText = dr[ColumnItemName].ToString()
                            };

                            ECN_Framework_BusinessLayer.Accounts.BillingReportItem.Save(reportItem);
                        }
                    }
                    else
                    {
                        if (dr[ColumnIsDeleted].ToString().Equals(True, StringComparison.OrdinalIgnoreCase))
                        {
                            var reportItem = ECN_Framework_BusinessLayer.Accounts.BillingReportItem.GetByBillingReportItemID(Convert.ToInt32(dr[ColumnId].ToString()));
                            reportItem.IsDeleted = true;
                            ECN_Framework_BusinessLayer.Accounts.BillingReportItem.Save(reportItem);
                        }
                        else
                        {
                            var reportItem = ECN_Framework_BusinessLayer.Accounts.BillingReportItem.GetByBillingReportItemID(Convert.ToInt32(dr[ColumnId].ToString()));
                            decimal amount;
                            decimal.TryParse(dr[ColumnAmount].ToString(), out amount);
                            reportItem.Amount = amount;
                            reportItem.InvoiceText = dr[ColumnItemName].ToString();
                            reportItem.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                            ECN_Framework_BusinessLayer.Accounts.BillingReportItem.Save(reportItem);
                        }
                    }
                }
            }
        }

        protected void btnCancelFlatRate_Click(object sender, EventArgs e)
        {
            modalPopupFlatRate.Hide();
        }

        protected void imgbtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton ib = (ImageButton)sender;

            dtFlatRateItems.AsEnumerable().First(x => x.Field<string>("ID") == ib.CommandArgument)["IsDeleted"] = "true";

            gvFlatRateItems.DataSource = dtFlatRateItems.AsEnumerable().Where(x => x.Field<string>("IsDeleted") == "false").AsDataView().ToTable();//.Select("IsDeleted = false");//.AsEnumerable().Where(x => x.Field<string>("IsDeleted") != "true");
            gvFlatRateItems.DataBind();
        }

        protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton ib = (ImageButton)sender;

            DataRow dr = dtFlatRateItems.AsEnumerable().First(x => x.Field<string>("ID") == ib.CommandArgument);

            txtItemName.Text = dr["ItemName"].ToString();
            txtItemRate.Text = dr["Amount"].ToString();

            modalPopupFlatRate.Show();
        }

        protected void gvFlatRateItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow dr = dtFlatRateItems.AsEnumerable().First(x => x.Field<string>("ID") == gvFlatRateItems.DataKeys[e.Row.RowIndex].Value.ToString());

                ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");
                ImageButton imgbtnDelete = (ImageButton)e.Row.FindControl("imgbtnDelete");

                imgbtnEdit.CommandArgument = dr["ID"].ToString();
                imgbtnDelete.CommandArgument = dr["ID"].ToString();
            }
        }

        private string BuildColumnSQL(List<string> columnList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(",");
            foreach (string s in columnList)
            {

                sb.Append(s + ",");

            }

            return sb.ToString().TrimEnd(',');
        }

        protected void ddlBaseChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        protected void btnAddNewReport_Click(object sender, EventArgs e)
        {
            pnlEditReport.Visible = true;
            pnlReportList.Visible = false;
            ClearControls();
        }

        private void ClearControls()
        {
            BillingReportID = -1;
            txtFromEmail.Text = string.Empty;
            txtFromName.Text = string.Empty;
            txtItemName.Text = string.Empty;
            txtItemRate.Text = string.Empty;
            txtMonth.Text = string.Empty;
            txtRecurringEndDate.Text = string.Empty;
            txtRecurringStartDate.Text = string.Empty;
            txtReportName.Text = string.Empty;
            txtRunFromDate.Text = string.Empty;
            txtRunToDate.Text = string.Empty;
            txtStartDate.Text = string.Empty;
            txtSubject.Text = string.Empty;
            txtToEmail.Text = string.Empty;

            gvFlatRateItems.DataSource = null;
            gvFlatRateItems.DataBind();



            currentReport = new ECN_Framework_Entities.Accounts.BillingReport();
            dtFlatRateItems = null;
            LoadLists();
        }

        protected void gvBillingReports_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString().ToLower().Equals("editreport"))
            {
                Response.Redirect(Request.Url.AbsoluteUri + "?BillingReportID=" + e.CommandArgument.ToString());
            }
            else if (e.CommandName.ToString().ToLower().Equals("deletereport"))
            {
                //delete report
                ECN_Framework_Entities.Accounts.BillingReport br = ECN_Framework_BusinessLayer.Accounts.BillingReport.GetByBillingReportID(Convert.ToInt32(e.CommandArgument.ToString()));
                br.IsDeleted = true;
                br.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                ECN_Framework_BusinessLayer.Accounts.BillingReport.SaveBillingReport(br);

                Response.Redirect(BillingReportPage);
            }
        }

        protected void gvBillingReports_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ECN_Framework_Entities.Accounts.BillingReport br = (ECN_Framework_Entities.Accounts.BillingReport)e.Row.DataItem;
                ImageButton imgbtnDeleteReport = (ImageButton)e.Row.FindControl("imgbtnDeleteReport");
                ImageButton imgbtnEditReport = (ImageButton)e.Row.FindControl("imgbtnEditReport");

                imgbtnDeleteReport.CommandArgument = br.BillingReportID.ToString();
                imgbtnEditReport.CommandArgument = br.BillingReportID.ToString();
            }
        }

        protected void gvFlatRateItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Equals("deleteflatrate"))
            {
                dtFlatRateItems.AsEnumerable().First(x => x.Field<string>("ID") == e.CommandArgument.ToString())["IsDeleted"] = "true";

                gvFlatRateItems.DataSource = dtFlatRateItems.AsEnumerable().Where(x => x.Field<string>("IsDeleted") == "false").AsDataView().ToTable();//.Select("IsDeleted = false");//.AsEnumerable().Where(x => x.Field<string>("IsDeleted") != "true");
                gvFlatRateItems.DataBind();
            }
            else if (e.CommandName.ToLower().Equals("editflatrate"))
            {
                hfIsEdit.Value = e.CommandArgument.ToString();
                DataRow dr = dtFlatRateItems.AsEnumerable().First(x => x.Field<string>("ID") == e.CommandArgument.ToString());

                txtItemName.Text = dr["ItemName"].ToString();
                txtItemRate.Text = dr["Amount"].ToString();

                modalPopupFlatRate.Show();
            }
        }

    }
}