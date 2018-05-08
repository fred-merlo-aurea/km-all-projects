using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Security.Principal;

namespace KMPS_JF_Setup.MasterPages {
    public partial class Site : System.Web.UI.MasterPage {
        private string _menu = string.Empty;
        private string _submenu = string.Empty;
        JFSession jfsess = new JFSession();

        public string Menu {
            get {
                return _menu;
            }

            set {
                _menu = value;
            }
        }

        public string SubMenu {
            get {
                return _submenu;
            }

            set {
                _submenu = value;
            }
        }

        public int PubID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["PubID"]); ;
                }
                catch
                {
                    return 0;
                }
            }

           
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadMenu();
            lblUserName.Text = jfsess.UserName();

            Page.ClientScript.RegisterClientScriptInclude("validation", ResolveUrl("~/scripts/Validation.js"));
            //Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/scripts/thickbox/jquery.js"));
            //Page.ClientScript.RegisterClientScriptInclude("thickbox", ResolveUrl("~/scripts/thickbox/thickbox.js"));
            Page.ClientScript.RegisterClientScriptInclude("highslide", ResolveUrl("~/scripts/highslide/highslide-full.js"));
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "hsinitialize", "<script>hs.graphicsDir = '" + ResolveUrl("~/scripts/highslide/graphics/") + "'; hs.outlineType = 'rounded-white';hs.allowSizeReduction='false';hs.objectLoadTime = 'after';</script>");

            if (!IsPostBack)
            {
                if (PubID > 0)
                {

                    if (!Convert.ToBoolean(DataFunctions.ExecuteScalar("if exists (select pubID from publications where PubID = " + PubID + " and ECNCustomerID in (" + jfsess.AllowedCustoemerIDs() + ")) select 1 else select 0")))
                    {
                        FormsAuthentication.SignOut();
                        Response.Redirect(FormsAuthentication.LoginUrl);
                    }

                }
            }

        }

        private void LoadMenu() {
            string tdMenu = string.Empty;

            foreach (DataRow dr in GetMenu().Rows) {
                tdMenu += "<td nowrap><a href='" + dr["MenuURL"].ToString() + "' class='" + (Menu.ToLower() == dr["MenuName"].ToString().ToLower() ? "menu-selected" : "menu") + "'>" + dr["MenuName"].ToString() + "</a></td>";
            }

            phMenu.Controls.Add(new LiteralControl(tdMenu));
        }

         private DataTable GetMenu() {

            DataTable dtMenu = new DataTable();

            DataColumn dc;

            dc = new DataColumn("MenuName");
            dtMenu.Columns.Add(dc);

            dc = new DataColumn("MenuURL");
            dtMenu.Columns.Add(dc);

            DataRow row;

            row = dtMenu.NewRow();
            row["MenuName"] = "HOME";
            row["MenuURL"] = ResolveUrl("~/Publisher/PublisherList.aspx");
            dtMenu.Rows.Add(row);

            return dtMenu;

        }

        private DataTable GetSubMenu() {

            DataTable dtMenu = new DataTable();

            DataColumn dc;
            dc = new DataColumn("MenuName");
            dtMenu.Columns.Add(dc);

            dc = new DataColumn("MenuURL");
            dtMenu.Columns.Add(dc);

            /*DataRow row;
            row = dtMenu.NewRow();
            row["MenuName"] = "Add";
            row["MenuURL"] = ResolveUrl("~/Publisher/PublisherAdd.aspx");
            dtMenu.Rows.Add(row);
            */
            return dtMenu;
        }

        protected void lnkbtnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            FormsAuthentication.SignOut();

            Response.Redirect(FormsAuthentication.LoginUrl);
        }        
    }
}