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
using System.Data.SqlClient; 


namespace KMPS_JF_Setup.Publisher
{
    public partial class PublisherList : System.Web.UI.Page
    {

        JFSession jfsess = new JFSession();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    grdPublisherList.PageSize = 20;

                    // bindCustomer
                    SqlCommand PublisherList = new SqlCommand();  
                    SqlDataSourcePCustomerConnect.SelectCommand = "select * from Customer where CustomerId in (" + jfsess.AllowedCustoemerIDs() + ")";
                    SqlDataSourcePCustomerConnect.Select(DataSourceSelectArguments.Empty);
                }
                else
                {
                    grdPublisherList.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            Response.Redirect("PublisherAdd.aspx");
        }


        protected void SqlDataSourcePListConnect_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            try 
            {
                if (ddlCustomer.SelectedItem.Value.ToString() == "0")
                    e.Command.CommandText = "select * from publications where ECNCustomerId in (" + jfsess.AllowedCustoemerIDs() + ")";
                else
                    e.Command.CommandText = "select * from publications where ECNCustomerId = " + ddlCustomer.SelectedItem.Value.ToString();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }


    }
}
