using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

using ecn.common.classes;

namespace PaidPub.main.eNewsletter
{
    public partial class CategoryAdd : System.Web.UI.Page
    {
        private int CategoryID
        {
            get
            {
                try { return Convert.ToInt32(Request.QueryString["CategoryID"]); }
                catch { return 0; }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Visible = false;

            if (!IsPostBack)
            {
                if (CategoryID > 0)
                    LoadCategory();
            }
        }

        private void LoadCategory()
        {
            DataTable dtNewsletter = DataFunctions.GetDataTable("select * from ecn_misc..CANON_PAIDPUB_eNewsLetter_Category where customerID = " + Session["CustomerID"].ToString() + " and CategoryID = " + CategoryID, ConfigurationManager.ConnectionStrings["conn_misc"].ConnectionString);

            if (dtNewsletter.Rows.Count > 0)
            {
                txtName.Text = dtNewsletter.Rows[0]["name"].ToString();
                txtDescription.Text = dtNewsletter.Rows[0]["desc"].ToString();
                
            }
            else
            {
                lblErrorMessage.Text = "Category not exists.";
                lblErrorMessage.Visible = true;
                btnSave.Visible = false;
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("category.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string sqlquery = string.Empty;
            bool Exists = false;
            try
            {
                Exists = Convert.ToBoolean(DataFunctions.ExecuteScalar("misc", "if exists (select categoryID from CANON_PAIDPUB_eNewsLetter_Category where customerID = " + Session["CustomerID"].ToString() + " and categoryID <> " + CategoryID + " and [name] = '" + txtName.Text.Replace("'", "''") + "')   select 1 else select 0"));

                if (!Exists)
                {
                    if (CategoryID == 0)
                    {
                        sqlquery = " INSERT INTO CANON_PAIDPUB_eNewsLetter_Category (CustomerID, [name], [desc]) VALUES (" + Session["CustomerID"].ToString() + ",'" + txtName.Text.Replace("'", "''") + "','" + txtDescription.Text.Replace("'", "''") + "')";
                        DataFunctions.Execute("misc", sqlquery);
                    }
                    else
                    {
                        sqlquery = " update CANON_PAIDPUB_eNewsLetter_Category set [name] = '" + txtName.Text.Replace("'", "''") + "', [desc]='" + txtDescription.Text.Replace("'", "''") + "' where categoryID = " + CategoryID.ToString();
                        DataFunctions.Execute("misc", sqlquery);

                    }
                    Response.Redirect("Category.aspx");
                }
                else
                {
                    lblErrorMessage.Text = "ERROR : Category already exists. Please use a different Name";
                    lblErrorMessage.Visible = true;
                }

            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "ERROR : " + ex.Message;
                lblErrorMessage.Visible = true;
            }
        }
    }
}
