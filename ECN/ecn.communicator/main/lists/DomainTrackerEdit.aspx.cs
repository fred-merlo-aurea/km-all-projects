using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text;
using System.Data;
using System.IO;
using System.Configuration;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;

namespace ecn.communicator.main.lists
{
    public partial class DomainTrackerEdit : System.Web.UI.Page
    {
        private DataTable DomainTrackerFields_DT
        {
            get
            {
                try
                {
                    return (DataTable)ViewState["DomainTrackerFields_DT"];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                ViewState["DomainTrackerFields_DT"] = value;
            }
        }

        private int getDomainTrackerID()
        {
            if (Request.QueryString["domainTrackerID"] != null)
            {
                return Convert.ToInt32(Request.QueryString["domainTrackerID"]);
            }
            else
                return -1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            if (!IsPostBack)
            {
                KMPlatform.Entity.User user = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
                if (!KM.Platform.User.IsSystemAdministrator(user) && !KM.Platform.User.IsChannelAdministrator(user))
                {
                    Response.Redirect("domainTrackerList.aspx");
                }
                if (KMPlatform.BusinessLogic.ClientGroup.HasServiceFeature(Master.UserSession.ClientGroupID, KMPlatform.Enums.Services.DOMAINTRACKING, KMPlatform.Enums.ServiceFeatures.DomainSetup))
                {
                    if (getDomainTrackerID() > 0)
                    {
                        txtDomainName.Visible = false;
                        lblDomainName.Visible = true;
                        loadData(getDomainTrackerID());
                    }
                }
                else
                {
                    throw new SecurityException();
                }
            }
        }

        private void loadData(int domainTrackerID)
        {
            ECN_Framework_Entities.DomainTracker.DomainTracker domainTracker = ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.GetByDomainTrackerID(domainTrackerID, Master.UserSession.CurrentUser);
            if (domainTracker != null)
            {
                lblDomainName.Text = domainTracker.Domain;
                DomainTrackerFields_DT = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerFields.GetByDomainTrackerID_DT(domainTrackerID, Master.UserSession.CurrentUser);
                loadGrid();
            }
        }

        protected void gvDomainTrackerFields_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string DomainTrackerFieldsID = e.CommandArgument.ToString();
            if (e.CommandName == "FieldDelete")
            {
                foreach (DataRow dr in DomainTrackerFields_DT.AsEnumerable())
                {
                    if (dr["DomainTrackerFieldsID"].Equals(DomainTrackerFieldsID))
                    {
                        dr["IsDeleted"] = true;
                    }
                }
                loadGrid();
            }
        }

        private void loadGrid()
        {
            var result = (from src in DomainTrackerFields_DT.AsEnumerable()
                          where src.Field<bool>("IsDeleted") == false
                          select new
                          {
                              DomainTrackerFieldsID = src.Field<string>("DomainTrackerFieldsID"),
                              FieldName = src.Field<string>("FieldName"),
                              IsDeleted = src.Field<bool>("IsDeleted"),
                              Source = src.Field<string>("Source"),
                              SourceID = src.Field<string>("SourceID")
                          }).ToList();
            gvDomainTrackerFields.DataSource = result;
            gvDomainTrackerFields.DataBind();
            if (result.Count > 0)
                gvDomainTrackerFields.Visible = true;
            else
                gvDomainTrackerFields.Visible = false;
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int domainTrackerID = getDomainTrackerID();
                if (getDomainTrackerID() < 0)
                {
                    if (txtDomainName.Text.Contains("http://") || txtDomainName.Text.Contains("https://"))
                    {
                        throwECNException("Please check the domain name entered. Do NOT include the protocol (http:// or https://) in the domain name.");
                    }

                    ECN_Framework_Entities.DomainTracker.DomainTracker domainTracker = new ECN_Framework_Entities.DomainTracker.DomainTracker();
                    domainTracker.Domain = txtDomainName.Text;
                    domainTracker.TrackerKey = Guid.NewGuid().ToString();
                    domainTracker.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                    domainTracker.BaseChannelID = Master.UserSession.CurrentBaseChannel.BaseChannelID;
                    domainTracker.DomainTrackerID = ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.Save(domainTracker, Master.UserSession.CurrentUser);
                    domainTrackerID = domainTracker.DomainTrackerID;
                }
                if (DomainTrackerFields_DT != null)
                {
                    foreach (DataRow dr in DomainTrackerFields_DT.AsEnumerable())
                    {
                        string isDeleted = dr["IsDeleted"].ToString();
                        if (dr["DomainTrackerFieldsID"].ToString().Contains("-") && isDeleted.Equals("False"))
                        {
                            ECN_Framework_Entities.DomainTracker.DomainTrackerFields domainTrackerFields = new ECN_Framework_Entities.DomainTracker.DomainTrackerFields();
                            domainTrackerFields.DomainTrackerID = domainTrackerID;
                            domainTrackerFields.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                            domainTrackerFields.Source = dr["Source"].ToString();
                            domainTrackerFields.SourceID = dr["SourceID"].ToString();
                            domainTrackerFields.FieldName = dr["FieldName"].ToString();
                            ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerFields.Save(domainTrackerFields, Master.UserSession.CurrentUser);
                        }
                        if (isDeleted.Equals("True") && !dr["DomainTrackerFieldsID"].ToString().Contains("-") && getDomainTrackerID() > 0)
                        {
                            ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerFields.Delete(Convert.ToInt32(dr["DomainTrackerFieldsID"].ToString()), getDomainTrackerID(), Master.UserSession.CurrentUser);
                        }
                    }
                }
                Response.Redirect("domainTrackerList.aspx");
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Blast, Enums.Method.Get, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            throw new ECNException(errorList, Enums.ExceptionLayer.WebSite);
        }

        protected void btnAddDomainTrackerFields_Click(object sender, EventArgs e)
        {
            if (!drpSource.SelectedValue.Equals("-Select-"))
            {
                DataTable dt = DomainTrackerFields_DT;
                if (dt == null)
                {
                    dt = new DataTable();
                    DataColumn DomainTrackerFieldsID = new DataColumn("DomainTrackerFieldsID", typeof(string));
                    dt.Columns.Add(DomainTrackerFieldsID);

                    DataColumn FieldName = new DataColumn("FieldName", typeof(string));
                    dt.Columns.Add(FieldName);

                    DataColumn Source = new DataColumn("Source", typeof(string));
                    dt.Columns.Add(Source);

                    DataColumn SourceID = new DataColumn("SourceID", typeof(string));
                    dt.Columns.Add(SourceID);

                    DataColumn IsDeleted = new DataColumn("IsDeleted", typeof(bool));
                    dt.Columns.Add(IsDeleted);
                }
                DataRow dr = dt.NewRow();
                dr["DomainTrackerFieldsID"] = Guid.NewGuid();
                dr["FieldName"] = txtFieldName.Text;
                dr["Source"] = drpSource.SelectedValue;
                dr["SourceID"] = txtSourceID.Text;
                dr["IsDeleted"] = false;
                dt.Rows.Add(dr);
                txtSourceID.Text = "";
                txtFieldName.Text = "";
                drpSource.SelectedValue = "-Select-";
                DomainTrackerFields_DT = dt;
                loadGrid();
            }
        }
    }
}