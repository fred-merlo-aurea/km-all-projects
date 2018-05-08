using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.IO;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;

namespace ecn.communicator.contentmanager.ckeditor.dialog
{	
    public partial class dynamicTag : System.Web.UI.Page
    {
		private void Page_Load(object sender, EventArgs e)
        {
			if(!Page.IsPostBack)
            {
                List<ECN_Framework_Entities.Communicator.DynamicTag> dynamicTagList = ECN_Framework_BusinessLayer.Communicator.DynamicTag.GetByCustomerID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
                var result= (from src in dynamicTagList
                            select new 
                            {
                                TagValue = "ECN.DynamicTag." + src.Tag + ".ECN.DynamicTag",
                                Tag= src.Tag
                            }).ToList();
                drpDynamicTag.DataSource = result;
                drpDynamicTag.DataBind();
            }
		}

        protected void drpDynamicTag_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfDynamicTag.Value = drpDynamicTag.SelectedValue;
        }
	
	}
}