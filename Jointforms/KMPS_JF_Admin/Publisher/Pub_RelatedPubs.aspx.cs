using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace KMPS_JF_Setup.Publisher
{
    public partial class Pub_RelatedPubs : System.Web.UI.Page
    {
        JFSession jfsess = new JFSession();

        protected void Page_Load(object sender, EventArgs e)
        {          
            try
            {
                if (!IsPostBack)
                    BoxPanel2.Title = "Manage Related Publications for " + Request.QueryString["PubName"] + ":";
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void SqlDataSourcePNonRelatedPubsConnect_Onselecting(object sender, EventArgs e)
        {
            string CustomerID = jfsess.AllowedCustoemerIDs();
            SqlDataSourcePNonRelatedPubsConnect.SelectParameters["ECNcustomerID"].DefaultValue = CustomerID.ToString();
           
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            this.moveSelectedItems(lstSource, lstDestination, false);
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            this.moveSelectedItems(lstDestination, lstSource, false);
        }

        private void moveSelectedItems(ListBox source, ListBox target, bool moveAllItems)
        {
            try
            {
                for (int i = source.Items.Count - 1; i >= 0; i--)
                {
                    ListItem item = source.Items[i];


                    if (item.Selected)
                    {

                        target.Items.Add(item);
                        item.Selected = false;

                        source.Items.Remove(item);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {                
                SqlDataSourcePRelatedPubsConnect.SelectParameters["AddedBy"].DefaultValue = jfsess.UserName();
                SqlDataSourcePRelatedPubsConnect.SelectParameters["iMod"].DefaultValue = "3";

                SqlDataSourcePRelatedPubsConnect.Select(DataSourceSelectArguments.Empty);


                foreach (ListItem ls in lstDestination.Items)
                {
                    SqlDataSourcePRelatedPubsConnect.SelectParameters["LinkedToPubID"].DefaultValue = ls.Value;
                    SqlDataSourcePRelatedPubsConnect.SelectParameters["AddedBy"].DefaultValue = jfsess.UserName();
                    SqlDataSourcePRelatedPubsConnect.SelectParameters["iMod"].DefaultValue = "1";
                    SqlDataSourcePRelatedPubsConnect.Select(DataSourceSelectArguments.Empty);
                }
                ClearData();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void ClearData()
        {
          
            SqlDataSourcePRelatedPubsConnect.SelectParameters["LinkedToPubID"].DefaultValue = "0";
            SqlDataSourcePRelatedPubsConnect.SelectParameters["iMod"].DefaultValue = "4";
        }
    }
}
