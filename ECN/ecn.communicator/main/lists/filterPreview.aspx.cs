using System;
using System.Data;
using System.Web.UI.WebControls;

namespace ecn.communicator.listsmanager
{	
	public partial class filterPreview : System.Web.UI.Page
    {
       
		protected void Page_Load(object sender, System.EventArgs e)
        {
            //if (KM.Platform.User.IsAdministratorOrHasUserPermission(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.grouppriv))
            //if (KM.Platform.User.IsAdministratorOrHasUserPermission(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Groups))
            if (KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupFilter, KMPlatform.Enums.Access.View))	
            {
				int filterID = getFilterID();
                int CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                ECN_Framework_Entities.Communicator.Filter filter= ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(filterID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                loadEmailsGrid(filterID, CustomerID, filter.GroupID.Value, filter.WhereClause.Trim());
                custID_Hidden.Value = CustomerID.ToString();
                grpID_Hidden.Value = filter.GroupID.Value.ToString();
                filterID_Hidden.Value = filterID.ToString();
			}
            else
            {
                throw new ECN_Framework_Common.Objects.SecurityException();			
			}
		}
		
		private int getFilterID()
        {
            if(Request.QueryString["FilterID"]!=null)                    
				return Convert.ToInt32(Request.QueryString["FilterID"].ToString());
            else
                return 0;
		}

		
        private void loadEmailsGrid(int FilterID, int CustomerID, int GroupID, string whereClause) 
        {
            if (whereClause.Length > 0)
            {
                whereClause = " AND (" + whereClause + ")";
			}

            DataTable emails = ECN_Framework_BusinessLayer.Communicator.EmailGroup.PreviewFilteredEmails(GroupID, CustomerID, whereClause.ToString());					
            EmailsGrid.DataSource=emails.DefaultView;
			EmailsGrid.DataBind();
			EmailsPager.RecordCount = emails.Rows.Count;

		}
       
	}
}
