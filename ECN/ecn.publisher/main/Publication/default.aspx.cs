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
using System.Text;
using System.Linq;
using CommonFunctions = ECN_Framework_Common.Functions;
using PubEntity = ECN_Framework_Entities.Publisher;
using PubBLL = ECN_Framework_BusinessLayer.Publisher;

namespace ecn.publisher.main.Publication
{
	public partial class _default : ECN_Framework_BusinessLayer.Application.WebPageHelper
	{
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
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Publisher.Enums.MenuCode.PUBLICATION;
            Master.SubMenu = "Publication List";
            Master.Heading = "Publication List";
            Master.HelpContent = "";
            Master.HelpTitle = "";	

			lblErrorMessage.Visible=false;
            grdPublication.PageSize = 10;

			if (!IsPostBack)
			{
                  ViewState["sortField"] = "PublicationName";
                  ViewState["sortOrder"] = "DESC";
                  loadPublicationGrid(false, sortField, sortOrder);	
			}
		}

        private void loadPublicationGrid(bool session_data, string sortfield, string sortdirection) 
		{
            sortOrder = sortdirection;
            sortField = sortfield;

            List<PubEntity.Publication> lPublications = new List<PubEntity.Publication>();
            List<PubEntity.Edition> lEditions = new List<PubEntity.Edition>();

            if (!session_data || Session["PUBLICATION"] == null || Session["EDITION"] == null)
            {
                lPublications = ECN_Framework_BusinessLayer.Publisher.Publication.GetByCustomerID(Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentUser);

                lEditions = ECN_Framework_BusinessLayer.Publisher.Edition.GetByCustomerID(Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentUser);

                Session["PUBLICATION"] = lPublications;
                Session["EDITION"] = lEditions;
            }

            lPublications = (List<PubEntity.Publication>)Session["PUBLICATION"];
            lEditions = (List<PubEntity.Edition>)Session["EDITION"];

            var pub = from p in lPublications
                      let archievedCount =
                        (
                            from x in lEditions
                            where x.PublicationID == p.PublicationID && x.Status.ToString() == "Archieve"
                            select new { x }
                        )
                      let activeCount =
                      (
                          from e in lEditions
                          where e.PublicationID == p.PublicationID && e.Status.ToString() == "Active"
                          select new { e }
                      )
                      orderby p.PublicationName
                      select new
                      {
                          p.PublicationID,
                          p.PublicationName,
                          p.PublicationCode,
                          Active = p.Active.ToString() == "True" ? "Yes" : "No",
                          ActiveEdition = activeCount.Count(),
                          ArchievedEdition = archievedCount.Count()
                      };

            switch (sortField)
            {
                case "PublicationName":
                    if (sortdirection == "ASC")
                    {
                        pub = pub.OrderBy(x => x.PublicationName).ToList();
                    }
                    else
                    {
                        pub = pub.OrderByDescending(x => x.PublicationName).ToList();
                    }
                    break;
                case "PublicationCode":
                    if (sortdirection == "ASC")
                    {
                        pub = pub.OrderBy(x => x.PublicationCode).ToList();
                    }
                    else
                    {
                        pub = pub.OrderByDescending(x => x.PublicationCode).ToList();
                    }
                    break;
            }

            grdPublication.DataSource = pub.ToList();
            grdPublication.DataBind();
		}

        protected void grdPublication_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {        
        }

        protected void grdPublication_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPublication.PageIndex = e.NewPageIndex;
            loadPublicationGrid(true, sortField, ViewState["sortOrder"].ToString());
        }

        public void grdPublication_Sorting(Object sender, GridViewSortEventArgs e)
        {
            loadPublicationGrid(true, e.SortExpression, sortOrder);
		}

        protected void grdPublication_RowCommand(object sender, GridViewCommandEventArgs e)
		{
            if (e.CommandName == "Delete")
            {
                try
                {
                    PubBLL.Publication.Delete(Convert.ToInt32(e.CommandArgument.ToString()), Master.UserSession.CurrentUser);
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

                loadPublicationGrid(false, sortField, ViewState["sortOrder"].ToString());
            }
		}
	}
}
