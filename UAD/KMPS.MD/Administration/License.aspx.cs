using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.SqlTypes;
using KMPS.MD.Objects;

namespace KMPS.MD.Administration
{
    public partial class License : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Sales View License";
            divError.Visible = false;
            lblErrorMessage.Text = "";

            if (!IsPostBack)
            {
                Config c = Config.getLicenseCount(Master.clientconnections);
                if (c.ConfigID != 0)
                {
                    txtLicenseCount.Text = c.Value;
                    hfConfigID.Value = c.ConfigID.ToString();
                }
            }
       }

        public void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Visible = true;
                lblMessage.Text = string.Empty;

                if (Convert.ToUInt32(hfConfigID.Value) >  0)
                {
                    int LicenseCount = Convert.ToInt32(Users.GetSalesViewAccessCount(Master.clientconnections).ToString()); 
                    if (Convert.ToInt32(txtLicenseCount.Text) < LicenseCount)
                    {
                        DisplayError("License Count cannot be less than users already assigned");
                        return;
                    }
                }

                Config config = new Config();
                config.Name = "License";
                config.ConfigID = Convert.ToInt32(hfConfigID.Value);
                config.Value = txtLicenseCount.Text;
                Config.Save(Master.clientconnections, config);

                lblMessage.Visible = true;
                lblMessage.Text = "License has been updated";
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("License.aspx");
        }
    }
}