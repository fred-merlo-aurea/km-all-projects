using System;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;

namespace ecn.communicator.main.lists.reports
{
    public partial class FlashReport : ECN_Framework.WebPageHelper
    {
        protected System.Web.UI.WebControls.TextBox FromDate;
        protected System.Web.UI.WebControls.TextBox ToDate;
        protected System.Web.UI.WebControls.DropDownList PubGroupID;
        protected System.Web.UI.WebControls.TextBox PromoCode;
        protected System.Web.UI.WebControls.Button SubmitBtn;
        protected System.Web.UI.WebControls.DataGrid ResultsGrid;
               
        private void Page_Load(object sender, System.EventArgs e)
        {
           Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS; 
            Master.SubMenu = "reports";
            Master.Heading = "";
            Master.HelpContent = "";
            Master.HelpTitle = "";

            if (!(Page.IsPostBack))
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.FlashReporting, KMPlatform.Enums.Access.View))
                {
                    loadPubDR();
                }
                else
                    throw new ECN_Framework_Common.Objects.SecurityException();
            }
        }

        public void loadPubDR()
        {
            List<ECN_Framework_Entities.Communicator.Group> groupList= ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID_NoAccessCheck(Master.UserSession.CurrentUser.CustomerID);
            var result = from src in groupList
                         where !src.MasterSupression.HasValue || !src.MasterSupression.Value.Equals(1)
                         orderby src.GroupName
                         select new
                         {
                             GroupID = src.GroupID,
                             GroupName = src.GroupName
                         };
            PubGroupID.DataSource = result;
            PubGroupID.DataValueField = "GroupID";
            PubGroupID.DataTextField = "GroupName";
            PubGroupID.DataBind();
        }


        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SubmitBtn.Click += new System.EventHandler(this.SubmitBtn_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        private void SubmitBtn_Click(object sender, System.EventArgs e)
        {
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
          
             if (DateTime.TryParse(FromDate.Text.Trim(), out startDate)) 
             {
             }
             else 
             { DateTime.TryParse("1/1/2005", out startDate);}
             
            if (DateTime.TryParse(ToDate.Text.Trim(), out endDate)) 
            {
            }
            else 
             {endDate = DateTime.Now.AddDays(1);}
            
           int groupID = 0;
            
            try
            {
                groupID = Convert.ToInt32(PubGroupID.SelectedValue.ToString());
            }
            catch (Exception) { }
            int custID = Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID.ToString());
            List<ECN_Framework_Entities.Activity.Report.FlashReport> flashReport = ECN_Framework_BusinessLayer.Activity.Report.FlashReport.GetList(groupID, custID, PromoCode.Text, startDate, endDate);



            ResultsGrid.DataSource = flashReport;
            ResultsGrid.DataBind();
        }
    }
}