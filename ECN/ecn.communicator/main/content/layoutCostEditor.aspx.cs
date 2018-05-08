using System;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.contentmanager
{

	public partial class layoutCostEditor : System.Web.UI.Page
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
            if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ROIReporting))
            {
                phError.Visible = false;
                MessageLabel.Text = "";
                int layoutID = getLayoutID();
                if (!(Page.IsPostBack))
                {
                    if (layoutID > 0)
                    {
                        loadCostDetails(layoutID);
                    }
                }
            }
            else
            {
                throw new SecurityException();	
            }
		}

		#region get Request Params
		private int getLayoutID()
        {
			int theLayoutID = 0;
            if (Request.QueryString["LayoutID"] != null)
            {
                theLayoutID = Convert.ToInt32(Request.QueryString["LayoutID"].ToString());
            }
			return theLayoutID;
		}
		#endregion

		#region save CostDetails
		public void updateCostDetails(int layoutID)
        {
            try
            {
                ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(layoutID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
                layout.SetupCost = this.SetupCostTxtBx.Text.Trim().ToString();
                layout.OutboundCost = this.OutboundCostTxtBx.Text.Trim().ToString();
                layout.InboundCost = this.InboundCostTxtBx.Text.Trim().ToString();
                layout.DesignCost = this.DesignCostTxtBx.Text.Trim().ToString();
                layout.OtherCost = this.OtherCostTxtBx.Text.Trim().ToString();
                ECN_Framework_BusinessLayer.Communicator.Layout.Save(layout, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                MessageLabel.Text = "Updated Cost Details. <input class='formbuttonsmall' type='button' value='close window' onClick=\"javascript:window.close()\">";
            
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }

		}
		#endregion

		#region save CostDetails
		public void loadCostDetails(int layoutID)
        {
            ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(layoutID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
            this.SetupCostTxtBx.Text = layout.SetupCost;
            this.OutboundCostTxtBx.Text = layout.OutboundCost;
            this.InboundCostTxtBx.Text = layout.InboundCost;
            this.DesignCostTxtBx.Text = layout.DesignCost;
            this.OtherCostTxtBx.Text = layout.OtherCost;
		}
		#endregion
        
		protected void SaveButton_Click(object sender, System.EventArgs e)
        {
			int layoutID = getLayoutID();
			if(layoutID > 0)
            {
				updateCostDetails(layoutID);
			}
		}
	}
}
