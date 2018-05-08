using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using CommonFunctions = ECN_Framework_Common.Functions;
using PubEntity = ECN_Framework_Entities.Publisher;
using PubBLL = ECN_Framework_BusinessLayer.Publisher;

namespace ecn.publisher.main.Edition
{
    public partial class _default : ECN_Framework_BusinessLayer.Application.WebPageHelper
	{

		int CustomerID = 0;
		
		private int getPublicationID() 
		{
			try 
			{
				if (!IsPostBack)
					return Convert.ToInt32(Request.QueryString["PublicationID"].ToString());
				else
					return (ddlPublication.SelectedIndex > 0?Convert.ToInt32(ddlPublication.SelectedValue):0);
			}
			catch
			{
				return 0;
			}
		}

		private string getEditionFilter() 
		{
			try 
			{
				if (!IsPostBack)
					return Request.QueryString["type"].ToString().ToLower();
				else
					return ((ddlType.SelectedIndex > 0)?ddlType.SelectedValue.ToLower():string.Empty);
			}
			catch
			{
				return string.Empty;
			}
		}

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

        public string sortField
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
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Publisher.Enums.MenuCode.EDITION;
            Master.SubMenu = "Edition List";
            Master.Heading = "Edition List";
            Master.HelpContent = "";
            Master.HelpTitle = "";

            CustomerID = Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID);

			if (!IsPostBack)
			{
                ViewState["sortField"] = "EnableDate";
                ViewState["sortOrder"] = "DESC";
                loadEditionGrid(false, sortField, sortOrder);	
				loadDropdowns();
			}
		}

		private void loadDropdowns()
		{
            ddlPublication.DataSource = PubBLL.Publication.GetByCustomerID(CustomerID, Master.UserSession.CurrentUser);
            ddlPublication.DataBind();

            ddlPublication.Items.Insert(0, "----- Select Publication -----");

			if (getPublicationID() > 0)
			{
                ddlPublication.ClearSelection();
                ddlPublication.Items.FindByValue(getPublicationID().ToString()).Selected = true;
			}

			ddlType.ClearSelection();
			if (getEditionFilter() != string.Empty)
                ddlType.Items.FindByValue(getEditionFilter().ToString().ToLower()).Selected = true;
		}

        private void loadEditionGrid(bool session_data, string sortfield, string sortdirection) 
		{
            sortOrder = sortdirection;
            sortField = sortfield;

            List<PubEntity.Publication> lPublications = new List<PubEntity.Publication>();
            List<PubEntity.Edition> lEditions = new List<PubEntity.Edition>();

            if (!session_data || Session["PUBLICATION"] == null || Session["EDITION"] == null)
            {
                lPublications = PubBLL.Publication.GetByCustomerID(CustomerID, Master.UserSession.CurrentUser);

                lEditions = PubBLL.Edition.GetByCustomerID(CustomerID, Master.UserSession.CurrentUser);

                Session["PUBLICATION"] = lPublications;
                Session["EDITION"] = lEditions;
            }

            lPublications = (List<PubEntity.Publication>)Session["PUBLICATION"];
            lEditions = (List<PubEntity.Edition>)Session["EDITION"];

            var query = from e in lEditions
                        join p in lPublications on e.PublicationID equals p.PublicationID  
                        select new {e.EditionID, e.EditionName, p.PublicationName, p.PublicationID, e.EnableDate, e.DisableDate, e.Pages, e.Status,
                        Searchable = e.IsSearchable == true ? 'Y' : 'N', e.CreatedDate};

            if (getEditionFilter() != string.Empty)
            {
                query = query.Where(a => a.Status.ToLower() ==  getEditionFilter().ToLower());
            }
            else 
            {
                List<string> EStatus = new List<string>() { ECN_Framework_Common.Objects.Publisher.Enums.Status.Active.ToString().ToLower(), ECN_Framework_Common.Objects.Publisher.Enums.Status.Inactive.ToString().ToLower(), ECN_Framework_Common.Objects.Publisher.Enums.Status.Archieve.ToString().ToLower(), ECN_Framework_Common.Objects.Publisher.Enums.Status.Pending.ToString().ToLower()};

                query = query.Where(a => EStatus.Contains(a.Status.ToLower()));
            }

			if (getPublicationID() > 0) 
			{
                query = query.Where(a => a.PublicationID == getPublicationID());
                grdEdition.Columns[1].Visible = false;
			}
			else
			{
                grdEdition.Columns[1].Visible = true;
			}

            switch (sortField)
            {
                case "PublicationName":
                    if (sortdirection == "ASC")
                    {
                        query = query.OrderBy(x => x.PublicationName).ToList();
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.PublicationName).ToList();
                    }
                    break;
                case "EditionName":
                    if (sortdirection == "ASC")
                    {
                        query = query.OrderBy(x => x.EditionName).ToList();
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.EditionName).ToList();
                    }
                    break;
                case "EnableDate":
                    if (sortdirection == "ASC")
                    {
                        query = query.OrderBy(x => x.EnableDate).ToList();
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.EnableDate).ToList();
                    }
                    break;
                case "DisableDate":
                    if (sortdirection == "ASC")
                    {
                        query = query.OrderBy(x => x.DisableDate).ToList();
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.DisableDate).ToList();
                    }
                    break;
                case "Pages":
                    if (sortdirection == "ASC")
                    {
                        query = query.OrderBy(x => x.Pages).ToList();
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Pages).ToList();
                    }
                    break;
                case "Status":
                    if (sortdirection == "ASC")
                    {
                        query = query.OrderBy(x => x.Status).ToList();
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Status).ToList();
                    }
                    break;
            }

            grdEdition.DataSource = query.ToList();
            grdEdition.DataBind();
		}


        protected void grdEdition_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdEdition.PageIndex = e.NewPageIndex;
            loadEditionGrid(true, sortField, ViewState["sortOrder"].ToString());
        }

        protected void grdEdition_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void grdEdition_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void grdEdition_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                try
                {
                    PubBLL.Edition.Delete(Convert.ToInt32(e.CommandArgument.ToString()), Master.UserSession.CurrentUser);

                    string Edition_Image_Path = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/Publisher/" + e.CommandArgument.ToString() + "/");

                    if (System.IO.Directory.Exists(Edition_Image_Path))
                    {
                        Directory.Delete(Edition_Image_Path, true);
                    }

                }
                catch (ECN_Framework_Common.Objects.ECNException ecnex)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (ECN_Framework_Common.Objects.ECNError err in ecnex.ErrorList)
                    {
                        sb.Append(err.ErrorMessage + "<BR>");
                    }
                    lblErrorMessage.Text = sb.ToString();
                    lblErrorMessage.Visible = true;
                }

                loadEditionGrid(false, sortField, ViewState["sortOrder"].ToString());
            }
        }

        public void grdEdition_Sorting(Object sender, GridViewSortEventArgs e)
        {
            loadEditionGrid(true, e.SortExpression, sortOrder);
        }

        public void grdPublication_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[5].Text.ToLower() == "pending")
                {
                    e.Row.Cells[7].Text = "";
                    e.Row.Cells[8].Text = "";
                    e.Row.Cells[9].Text = "";
                    e.Row.Cells[10].Text = "";
                    e.Row.Cells[11].Text = "";
                }
                else if (e.Row.Cells[5].Text.ToLower() == "inactive")
                {
                    e.Row.Cells[6].Text = "";
                }
            }
        }

		protected void ddlPublication_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            loadEditionGrid(false, sortField, ViewState["sortOrder"].ToString());
		}

		protected void ddlType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            loadEditionGrid(false, sortField, ViewState["sortOrder"].ToString());
		}
	}
}
