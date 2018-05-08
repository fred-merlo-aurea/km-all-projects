
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace KMPS.MD.Main
{
    public partial class EditDashboard : KMPS.MD.Main.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Dashboard";      
        }
		
		protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack)
            {
                ctlEditor.Edit(Request["d"]);
            }
        }
    }
}
