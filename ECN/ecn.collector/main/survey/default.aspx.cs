using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace ecn.collector.main.survey {

    public partial class _default : Page
    {            
		
		protected void Page_Load(object sender, System.EventArgs e) 
		{
			if (!IsPostBack)
                BuildSurveyList(Master.UserSession.CurrentUser.CustomerID);
		}

 		private void BuildSurveyList(int customerID) 
        {
            List<ECN_Framework_Entities.Collector.Survey> surveyList= ECN_Framework_BusinessLayer.Collector.Survey.GetByCustomerID(customerID, Master.UserSession.CurrentUser);
            var result = (from src in surveyList
                         where src.IsActive == false && src.ResponseCount==0 
                         select src).ToList();
            dgPendingSurveys.DataSource = result;
			dgPendingSurveys.DataBind();

            result = (from src in surveyList
                      where src.IsActive == true 
                      select src).ToList();
            dgLiveSurveys.DataSource = result;
			dgLiveSurveys.DataBind();

            result = (from src in surveyList
                      where src.IsActive == false && src.ResponseCount > 0
                      select src).ToList();
            dgCompletedSurveys.DataSource = result;
			dgCompletedSurveys.DataBind();
                
		}

		private void dgPendingSurveys_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
			{
                ImageButton btnDelete = (ImageButton)e.Item.FindControl("btnDelete");
				btnDelete.Attributes.Add("onclick", "return confirm('Are You Sure You want to delete?')");
			}
		}

        private void dgPendingSurveys_ItemCommand(object sender, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "delete")
            {
                ECN_Framework_BusinessLayer.Collector.Survey.Delete(Convert.ToInt32(e.CommandArgument.ToString()), Master.UserSession.CurrentUser);
                Response.Redirect("default.aspx");
            }
        }

        private void dgLiveSurveys_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblresponsecount1 = (Label)e.Item.FindControl("lblresponsecount1");

                if (Convert.ToInt32(lblresponsecount1.Text) == 0)
                {
                    HyperLink lnkReport1 = (HyperLink)e.Item.FindControl("lnkReport1");
                    lnkReport1.Visible = false;
                }

                ImageButton btnDelete = (ImageButton)e.Item.FindControl("btnDeleteResponse");
                btnDelete.Attributes.Add("onclick", "return confirm('Are You Sure You want to delete the responses?')");
            }
        }

        private void dgLiveSurveys_ItemCommand(object sender, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "create")
            {
                Response.Redirect("../Content/ContentEditor.aspx?SurveyID=" + dgLiveSurveys.DataKeys[e.Item.ItemIndex]);


            }
            else if (e.CommandName.ToLower() == "delete")
            {

                ECN_Framework_BusinessLayer.Collector.Survey.DeleteResponses(Convert.ToInt32(e.CommandArgument.ToString()), Master.UserSession.CurrentUser);
                Response.Redirect("default.aspx");
            }
        }

        private void dgCompletedSurveys_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                ImageButton btnDelete = (ImageButton)e.Item.FindControl("btnDeleteResponse");
                btnDelete.Attributes.Add("onclick", "return confirm('Are You Sure You want to delete the responses?')");
            }
        }


        private void dgCompletedSurveys_ItemCommand(object sender, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "delete")
            {

                ECN_Framework_BusinessLayer.Collector.Survey.DeleteResponses(Convert.ToInt32(e.CommandArgument.ToString()), Master.UserSession.CurrentUser);
                Response.Redirect("default.aspx");
            }
        }

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			InitializeComponent();
			base.OnInit(e);
		}

		private void InitializeComponent() { 			
			this.dgPendingSurveys.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgPendingSurveys_ItemDataBound);
            this.dgLiveSurveys.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgLiveSurveys_ItemDataBound);
            this.dgCompletedSurveys.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgCompletedSurveys_ItemDataBound);

            this.dgPendingSurveys.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgPendingSurveys_ItemCommand);
            this.dgLiveSurveys.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgLiveSurveys_ItemCommand);
            this.dgCompletedSurveys.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgCompletedSurveys_ItemCommand);


		}
		#endregion	
	}
}
