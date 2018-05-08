using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.SessionState;

namespace ecn.collector {
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["Collector_VirtualPath"] + "/main/default.aspx");
        }
        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }
        private void InitializeComponent()
        {
        }
        #endregion
    }
}
