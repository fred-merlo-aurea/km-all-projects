using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ecn.publisher.main.Edition
{

    public partial class txtAlias : ECN_Framework_BusinessLayer.Application.WebPageHelper
	{

		private int getEditionID() 
		{
			try 
			{
				return Convert.ToInt32(Request.QueryString["EditionID"].ToString());
			}
			catch
			{
				return 0;
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Publisher.Enums.MenuCode.EDITION;
            Master.SubMenu = "Edition List";
            Master.Heading = "Link Alias";
            Master.HelpContent = "";
            Master.HelpTitle = "";

			if (!IsPostBack)
			{
				LoadGrid();
			}
		}

		private void LoadGrid()
		{
            List<ECN_Framework_Entities.Publisher.Page> page = ECN_Framework_BusinessLayer.Publisher.Page.GetByEditionID(getEditionID(), Master.UserSession.CurrentUser, true);

            var query = from p in page
                        from l in p.LinkList
                        where l.LinkType == "URI"
                        orderby l.LinkURL
                        select new { l.LinkID, l.LinkURL, l.Alias };

            grdAlias.DataSource = query.ToList();
            grdAlias.DataBind();
		}

        protected void grdAlias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlias.PageIndex = e.NewPageIndex;
           LoadGrid();
        }

        protected void btnsave_Click(object sender, System.EventArgs e)
        {
            foreach (GridViewRow row in grdAlias.Rows)
            {
                Label lblURL = (Label)row.FindControl("lblURL");
                TextBox txtAlias = (TextBox)row.FindControl("txtAlias");

                ECN_Framework_Entities.Publisher.Link link = ECN_Framework_BusinessLayer.Publisher.Link.GetByLinkID((int)grdAlias.DataKeys[row.RowIndex].Value, Master.UserSession.CurrentUser);
                link.Alias = txtAlias.Text.Replace("'", "''");
                link.UpdatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);

                try
                {
                    ECN_Framework_BusinessLayer.Publisher.Link.Save(link, Master.UserSession.CurrentUser,true);
                }
                catch (ECN_Framework_Common.Objects.ECNException ecnex)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (ECN_Framework_Common.Objects.ECNError err in ecnex.ErrorList)
                    {
                        sb.Append(err.ErrorMessage + "<BR>");
                    }
                    lblErrorMessage.Text = sb.ToString();
                    phError.Visible = true;
                    return;
                }
           }
		}
	}
}
