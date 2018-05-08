using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ecn.communicator.admin.deliverability
{
    public partial class _default : ECN_Framework.WebPageHelper
    {
        private const string DeliverabilityCustomerKey = "DELIVERABILITY_CUST";
        private const string DeliverabilityIPKey = "DELIVERABILITY_IP";
        private const string SortOrderKey = "sortOrder";
        private const string IronPort = "IRONPORT";

        public string sortOrder
        {
            get
            {
                if (ViewState["sortOrder"].ToString() == "DESC")
                {
                    ViewState["sortOrder"] = "ASC";
                }
                else
                {
                    ViewState["sortOrder"] = "DESC";
                }

                return ViewState["sortOrder"].ToString();
            }
            set
            {
                ViewState["sortOrder"] = value;
            }
        }

        public string SortField
        {
            get
            {
                return ViewState["sortField"].ToString();
            }
            set
            {
                ViewState["sortField"] = value;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = MenuCode.INDEX;
            Master.SubMenu = "";
            Master.Heading = "";
            Master.HelpContent = "";
            Master.HelpTitle = "";

            if (!KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                throw new SecurityException();
            }
            if (!(Page.IsPostBack))
            {
                ViewState["sortOrder"] = "";
            }
        }

        private void GetData(string session_field)
        {
            if (session_field.Equals("DELIVERABILITY_CUST"))
            {
                Session[session_field] = ECN_Framework_BusinessLayer.Communicator.Report.Deliverability.Get(startdate.Text, enddate.Text);
            }
            else if (session_field.Equals("DELIVERABILITY_IP"))
            {
                Session[session_field] = ECN_Framework_BusinessLayer.Communicator.Report.Deliverability.GetByIP(startdate.Text, enddate.Text, IPFilter.Text);
            }
        }

        private void getDeliverability_byBlast(bool sessionData, string sortField, string sortDirection)
        {
            SortField = sortField;
            ViewState[SortOrderKey] = sortDirection;
            try
            {
                if (!sessionData)
                {
                    GetData(DeliverabilityCustomerKey);
                }
                var datatable = Session[DeliverabilityCustomerKey] as DataTable;
                if (datatable == null)
                {
                    throw new InvalidOperationException("DELIVERABILITY_CUST is not a DataTable");
                }

                var deliverability =
                    (from src in datatable.AsEnumerable()
                     group src by new
                     {
                         CustomerID = src.Field<int>("CustomerID"),
                         CustomerName = src.Field<string>("CustomerName")
                     }
                     into grp1
                     select new
                     {
                         CustomerID = grp1.Key.CustomerID,
                         CustomerName = grp1.Key.CustomerName,
                         Data = CalculateDeliverabilityData(grp1)
                     }).ToList();

                var query = deliverability.Select(src => new
                {
                    CustomerID = src.CustomerID,
                    CustomerName = src.CustomerName,
                    TotalSent = src.Data.TotalSent,
                    SoftBounces = src.Data.SoftBounces,
                    SBPerc = src.Data.SBPerc,
                    HardBounces = src.Data.HardBounces,
                    HBPerc = src.Data.HBPerc,
                    MailBlock = src.Data.MailBlock,
                    MBPerc = src.Data.MBPerc,
                    Complaint = src.Data.Complaint,
                    ComPerc = src.Data.ComPerc,
                    OptOut = src.Data.OptOut,
                    OptOPerc = src.Data.OptOPerc,
                    MasterSupp = src.Data.MasterSupp,
                    MSPerc = src.Data.MSPerc
                }).ToList();

                if (sortField.Equals("CustomerID"))
                {
                    query = query.OrderBy(x => x.CustomerID).ToList();
                }
                else if (sortField.Equals("CustomerName"))
                {
                    query = query.OrderBy(x => x.CustomerName).ToList();
                }
                else if (sortField.Equals("TotalSent"))
                {
                    query = query.OrderBy(x => x.TotalSent).ToList();
                }
                else if (sortField.Equals("SoftBounces"))
                {
                    query = query.OrderBy(x => x.SoftBounces).ToList();
                }
                else if (sortField.Equals("SBPerc"))
                {
                    query = query.OrderBy(x => x.SBPerc).ToList();
                }
                else if (sortField.Equals("HardBounces"))
                {
                    query = query.OrderBy(x => x.HardBounces).ToList();
                }
                else if (sortField.Equals("HBPerc"))
                {
                    query = query.OrderBy(x => x.HBPerc).ToList();
                }
                else if (sortField.Equals("MailBlock"))
                {
                    query = query.OrderBy(x => x.MailBlock).ToList();
                }
                else if (sortField.Equals("MBPerc"))
                {
                    query = query.OrderBy(x => x.MBPerc).ToList();
                }
                else if (sortField.Equals("Complaint"))
                {
                    query = query.OrderBy(x => x.Complaint).ToList();
                }
                else if (sortField.Equals("ComPerc"))
                {
                    query = query.OrderBy(x => x.ComPerc).ToList();
                }
                else if (sortField.Equals("OptOut"))
                {
                    query = query.OrderBy(x => x.OptOut).ToList();
                }
                else if (sortField.Equals("OptOPerc"))
                {
                    query = query.OrderBy(x => x.OptOPerc).ToList();
                }
                else if (sortField.Equals("MasterSupp"))
                {
                    query = query.OrderBy(x => x.MasterSupp).ToList();
                }
                else if (sortField.Equals("MSPerc"))
                {
                    query = query.OrderBy(x => x.MSPerc).ToList();
                }

                if (sortDirection.Equals("DESC"))
                {
                    query.Reverse();
                }

                gvBlasts.DataSource = query;
                gvBlasts.DataBind();

                highlight_threshold(gvBlasts, dropdownView.SelectedValue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void gvBlasts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBlasts.PageIndex = e.NewPageIndex;
            getDeliverability_byBlast(true, SortField, ViewState["sortOrder"].ToString());
        }

        protected void gvBlastsIP_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBlastsIP.PageIndex = e.NewPageIndex;
            getDeliverability_byIP(true, SortField, ViewState["sortOrder"].ToString());
        }

        private void getDeliverability_byIP(bool sessionData, string sortField, string sortDirection)
        {
            SortField = sortField;
            ViewState[SortOrderKey] = sortDirection;
            if (!sessionData)
            {
                GetData(DeliverabilityIPKey);
            }
            var datatable = Session[DeliverabilityIPKey] as DataTable;
            if (datatable == null)
            {
                throw new InvalidOperationException("DELIVERABILITY_IP is not a DataTable");
            }

            var deliverability =
                (from src in datatable.AsEnumerable()
                 group src by new
                 {
                     SourceIP = src.Field<string>("SourceIP"),
                     HostName = src.Field<string>("hostname")
                 }
                 into grp1
                 select new
                 {
                     SourceIP = grp1.Key.SourceIP,
                     HostName = grp1.Key.HostName,
                     Data = CalculateDeliverabilityData(grp1)
                 }).ToList();

            var query = deliverability.Select(src => new
            {
                SourceIP = src.SourceIP,
                HostName = src.HostName,
                TotalSent = src.Data.TotalSent,
                SoftBounces = src.Data.SoftBounces,
                SBPerc = src.Data.SBPerc,
                HardBounces = src.Data.HardBounces,
                HBPerc = src.Data.HBPerc,
                MailBlock = src.Data.MailBlock,
                MBPerc = src.Data.MBPerc,
                Complaint = src.Data.Complaint,
                ComPerc = src.Data.ComPerc,
                OptOut = src.Data.OptOut,
                OptOPerc = src.Data.OptOPerc,
                MasterSupp = src.Data.MasterSupp,
                MSPerc = src.Data.MSPerc
            }).ToList();

            if (sortField.Equals("SourceIP"))
            {
                query = query.OrderBy(x => x.SourceIP).ToList();
            }
            else if (sortField.Equals("HostName"))
            {
                query = query.OrderBy(x => x.HostName).ToList();
            }
            else if (sortField.Equals("TotalSent"))
            {
                query = query.OrderBy(x => x.TotalSent).ToList();
            }
            else if (sortField.Equals("SoftBounces"))
            {
                query = query.OrderBy(x => x.SoftBounces).ToList();
            }
            else if (sortField.Equals("SBPerc"))
            {
                query = query.OrderBy(x => x.SBPerc).ToList();
            }
            else if (sortField.Equals("HardBounces"))
            {
                query = query.OrderBy(x => x.HardBounces).ToList();
            }
            else if (sortField.Equals("HBPerc"))
            {
                query = query.OrderBy(x => x.HBPerc).ToList();
            }
            else if (sortField.Equals("MailBlock"))
            {
                query = query.OrderBy(x => x.MailBlock).ToList();
            }
            else if (sortField.Equals("MBPerc"))
            {
                query = query.OrderBy(x => x.MBPerc).ToList();
            }
            else if (sortField.Equals("Complaint"))
            {
                query = query.OrderBy(x => x.Complaint).ToList();
            }
            else if (sortField.Equals("ComPerc"))
            {
                query = query.OrderBy(x => x.ComPerc).ToList();
            }
            else if (sortField.Equals("OptOut"))
            {
                query = query.OrderBy(x => x.OptOut).ToList();
            }
            else if (sortField.Equals("OptOPerc"))
            {
                query = query.OrderBy(x => x.OptOPerc).ToList();
            }
            else if (sortField.Equals("MasterSupp"))
            {
                query = query.OrderBy(x => x.MasterSupp).ToList();
            }
            else if (sortField.Equals("MSPerc"))
            {
                query = query.OrderBy(x => x.MSPerc).ToList();
            }

            if (sortDirection.Equals("DESC"))
            {
                query.Reverse();
            }

            gvBlastsIP.DataSource = query;
            gvBlastsIP.DataBind();
            highlight_threshold(gvBlastsIP, dropdownViewIP.SelectedValue);
        }

        protected void dropdownView_SelectedIndexChanged(object sender, EventArgs e)
        {
            highlight_threshold(gvBlasts, dropdownView.SelectedValue);
        }

        protected void dropdownViewIP_SelectedIndexChanged(object sender, EventArgs e)
        {
            highlight_threshold(gvBlastsIP, dropdownViewIP.SelectedValue);
        }

        private void highlight_threshold(GridView gv, string value)
        {
            string controltext = null;
            if (gv.ID.Equals("gvBlasts"))
            {
                controltext = "lbl";
            }
            else if (gv.ID.Equals("gvBlastsIP"))
            {
                controltext = "lblIP";
            }
            try
            {
                foreach (GridViewRow gr in gv.Rows)
                {
                    if (double.Parse(((Label)gr.FindControl("gv" + controltext + "HBPerc")).Text.Replace("%", "")) > Int32.Parse(value))
                    {
                        gr.Cells[4].ForeColor = System.Drawing.Color.Red;
                        gr.Cells[4].Font.Bold = true;
                    }
                    else
                    {
                        gr.Cells[4].ForeColor = System.Drawing.Color.Black;
                        gr.Cells[4].Font.Bold = false;
                    }

                    if (double.Parse(((Label)gr.FindControl("gv" + controltext + "SBPerc")).Text.Replace("%", "")) > Int32.Parse(value))
                    {
                        gr.Cells[6].ForeColor = System.Drawing.Color.Red;
                        gr.Cells[6].Font.Bold = true;
                    }
                    else
                    {
                        gr.Cells[6].ForeColor = System.Drawing.Color.Black;
                        gr.Cells[6].Font.Bold = false;
                    }

                    if (double.Parse(((Label)gr.FindControl("gv" + controltext + "MBPerc")).Text.Replace("%", "")) > Int32.Parse(value))
                    {
                        gr.Cells[8].ForeColor = System.Drawing.Color.Red;
                        gr.Cells[8].Font.Bold = true;
                    }
                    else
                    {
                        gr.Cells[8].ForeColor = System.Drawing.Color.Black;
                        gr.Cells[8].Font.Bold = false;
                    }

                    if (double.Parse(((Label)gr.FindControl("gv" + controltext + "ComPerc")).Text.Replace("%", "")) > Int32.Parse(value))
                    {
                        gr.Cells[10].ForeColor = System.Drawing.Color.Red;
                        gr.Cells[10].Font.Bold = true;
                    }
                    else
                    {
                        gr.Cells[10].ForeColor = System.Drawing.Color.Black;
                        gr.Cells[10].Font.Bold = false;
                    }

                    if (double.Parse(((Label)gr.FindControl("gv" + controltext + "OptOPerc")).Text.Replace("%", "")) > Int32.Parse(value))
                    {
                        gr.Cells[12].ForeColor = System.Drawing.Color.Red;
                        gr.Cells[12].Font.Bold = true;
                    }
                    else
                    {
                        gr.Cells[12].ForeColor = System.Drawing.Color.Black;
                        gr.Cells[12].Font.Bold = false;
                    }
                    if (double.Parse(((Label)gr.FindControl("gv" + controltext + "MSPerc")).Text.Replace("%", "")) > Int32.Parse(value))
                    {
                        gr.Cells[14].ForeColor = System.Drawing.Color.Red;
                        gr.Cells[14].Font.Bold = true;
                    }
                    else
                    {
                        gr.Cells[14].ForeColor = System.Drawing.Color.Black;
                        gr.Cells[14].Font.Bold = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void gvSubBlasts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink myLink1 = ((HyperLink)e.Row.FindControl("gvlblBlastID"));
                string HyperLinkValue1 = "/ecn.communicator/main/blasts/bounces.aspx?BlastID=" + myLink1.Text;
                myLink1.NavigateUrl = HyperLinkValue1;
                myLink1.Target = "_blank";
            }
        }

        protected void gvSubBlastsIP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink myLink1 = ((HyperLink)e.Row.FindControl("gvSubIPBlastID"));
                string HyperLinkValue1 = "/ecn.communicator/main/blasts/bounces.aspx?BlastID=" + myLink1.Text;
                myLink1.NavigateUrl = HyperLinkValue1;
                myLink1.Target = "_blank";
            }
        }

        protected void lnkReport_Command(object sender, CommandEventArgs e)
        {
            var customerID = e.CommandArgument.ToString();
            var datatable = Session[DeliverabilityCustomerKey] as DataTable;
            if (datatable == null)
            {
                throw new InvalidOperationException("DELIVERABILITY_CUST is not a DataTable");
            }

            var query =
                (from src in datatable.AsEnumerable()
                 group src by new
                 {
                     CustomerID = src.Field<int>("CustomerID"),
                     CustomerName = src.Field<string>("CustomerName"),
                     BlastID = src.Field<int>("BlastID")
                 }
                 into grp1
                 select new
                 {
                     CustomerID = grp1.Key.CustomerID,
                     CustomerName = grp1.Key.CustomerName,
                     BlastID = grp1.Key.BlastID,
                     Data = CalculateDeliverabilityData(grp1)
                 }).ToList();

            var orderdDeliverability =
                (from src in query
                 where src.CustomerID == Int32.Parse(customerID)
                 select new
                 {
                     CustomerID = src.CustomerID,
                     CustomerName = src.CustomerName,
                     BlastID = src.BlastID,
                     TotalSent = src.Data.TotalSent,
                     SoftBounces = src.Data.SoftBounces,
                     SBPerc = src.Data.SBPerc,
                     HardBounces = src.Data.HardBounces,
                     HBPerc = src.Data.HBPerc,
                     MailBlock = src.Data.MailBlock,
                     MBPerc = src.Data.MBPerc,
                     Complaint = src.Data.Complaint,
                     ComPerc = src.Data.ComPerc,
                     OptOut = src.Data.OptOut,
                     OptOPerc = src.Data.OptOPerc,
                     MasterSupp = src.Data.MasterSupp,
                     MSPerc = src.Data.MSPerc
                 }).OrderBy(x => x.HBPerc).ToList();

            gvSubBlasts.DataSource = orderdDeliverability;
            gvSubBlasts.DataBind();
            mdlPopup.Show();
        }

        protected void lnkIPReport_Command(object sender, CommandEventArgs e)
        {
            var sourceIP = e.CommandArgument.ToString();
            if (string.CompareOrdinal(sourceIP, IronPort) != 0)
            {
                var datatable = Session[DeliverabilityIPKey] as DataTable;
                if (datatable == null)
                {
                    throw new InvalidOperationException("DELIVERABILITY_IP is not a DataTable");
                }

                var query =
                    (from src in datatable.AsEnumerable()
                     group src by new
                     {
                         SourceIP = src.Field<string>("SourceIP"),
                         BlastID = src.Field<int>("BlastID"),
                         CustomerName = src.Field<string>("CustomerName")
                     }
                     into grp1
                     select new
                     {
                         SourceIP = grp1.Key.SourceIP,
                         BlastID = grp1.Key.BlastID,
                         CustomerName = grp1.Key.CustomerName,
                         Data = CalculateDeliverabilityData(grp1)
                     }).ToList();

                var orderdDeliverability =
                    (from src in query
                     where src.SourceIP == sourceIP
                     select new
                     {
                         SourceIP = src.SourceIP,
                         BlastID = src.BlastID,
                         CustomerName = src.CustomerName,
                         TotalSent = src.Data.TotalSent,
                         SoftBounces = src.Data.SoftBounces,
                         SBPerc = src.Data.SBPerc,
                         HardBounces = src.Data.HardBounces,
                         HBPerc = src.Data.HBPerc,
                         MailBlock = src.Data.MailBlock,
                         MBPerc = src.Data.MBPerc,
                         Complaint = src.Data.Complaint,
                         ComPerc = src.Data.ComPerc,
                         OptOut = src.Data.OptOut,
                         OptOPerc = src.Data.OptOPerc,
                         MasterSupp = src.Data.MasterSupp,
                         MSPerc = src.Data.MSPerc
                     }).OrderBy(x => x.HBPerc).ToList();

                gvSubBlastsIP.DataSource = orderdDeliverability;
                gvSubBlastsIP.DataBind();
                gvSubBlastsIP.Visible = true;
                lblIPSubRptHeader.Text = "Email Deliverability by Blasts";
            }
            else
            {
                lblIPSubRptHeader.Text = "DATA NOT AVAILABLE FOR IRONPORT";
                gvSubBlastsIP.Visible = false;
            }

            mdlPopupIP.Show();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.mdlPopup.Hide();
        }

        protected void btnIPClose_Click(object sender, EventArgs e)
        {
            this.mdlPopupIP.Hide();
        }

        protected void btnSubmitCust_Click(object sender, EventArgs e)
        {
            getDeliverability_byBlast(false, "HardBounces", "DESC");
        }

        protected void btnSubmitIP_Click(object sender, EventArgs e)
        {
            getDeliverability_byIP(false, "HardBounces", "DESC");
        }

        protected void gvBlasts_Sorting(object sender, GridViewSortEventArgs e)
        {
            getDeliverability_byBlast(true, e.SortExpression, sortOrder);
        }

        protected void gvBlastsIP_Sorting(object sender, GridViewSortEventArgs e)
        {
            getDeliverability_byIP(true, e.SortExpression, sortOrder);
        }

        private DeliverabilityQueryResults CalculateDeliverabilityData(IGrouping<object, DataRow> group)
        {
            var deliverabilityResults = new DeliverabilityQueryResults();
            deliverabilityResults.CalculateDeliverability(group);
            return deliverabilityResults;
        }
    }
}
