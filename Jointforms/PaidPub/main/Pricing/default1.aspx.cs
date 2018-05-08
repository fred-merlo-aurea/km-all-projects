using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ecn.common.classes;


namespace PaidPub.main.Pricing
{
    public partial class _default1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Visible = false;

            if (!IsPostBack)
            {
                AddDecimalValidation(txtCombo2yr1);
                AddDecimalValidation(txtCombo2yr2);
                AddDecimalValidation(txtCombo2yr3);
                AddDecimalValidation(txtCombo3yr1);
                AddDecimalValidation(txtCombo3yr2);
                AddDecimalValidation(txtCombo3yr3);
                AddDecimalValidation(txtCombo4yr1);
                AddDecimalValidation(txtCombo4yr2);
                AddDecimalValidation(txtCombo4yr3);
                AddDecimalValidation(txtCombo5yr1);
                AddDecimalValidation(txtCombo5yr2);
                AddDecimalValidation(txtCombo5yr3);
                AddDecimalValidation(txtCombo6yr1);
                AddDecimalValidation(txtCombo6yr2);
                AddDecimalValidation(txtCombo6yr3);
                AddDecimalValidation(txtCombo7yr1);
                AddDecimalValidation(txtCombo7yr2);
                AddDecimalValidation(txtCombo7yr3);
                AddDecimalValidation(txtCombo8yr1);
                AddDecimalValidation(txtCombo8yr2);
                AddDecimalValidation(txtCombo8yr3);
                AddDecimalValidation(txtCombo9yr1);
                AddDecimalValidation(txtCombo9yr2);
                AddDecimalValidation(txtCombo9yr3);
                AddDecimalValidation(txtCombo10yr1);
                AddDecimalValidation(txtCombo10yr2);
                AddDecimalValidation(txtCombo10yr3);

                LoadRates();
            }
        }

        private void LoadRates()
        {
            DataTable dtRates = DataFunctions.GetDataTable("select * from CANON_PAIDPUB_ComboDiscounts where customerID = " + Session["CustomerID"].ToString(), ConfigurationManager.ConnectionStrings["conn_misc"].ConnectionString);

            if (dtRates.Rows.Count > 0)
            {
                txtCombo2yr1.Text = dtRates.Rows[0]["Combo2yr1"].ToString();
                txtCombo2yr2.Text = dtRates.Rows[0]["Combo2yr2"].ToString();
                txtCombo2yr3.Text = dtRates.Rows[0]["Combo2yr3"].ToString();
                txtCombo3yr1.Text = dtRates.Rows[0]["Combo3yr1"].ToString();
                txtCombo3yr2.Text = dtRates.Rows[0]["Combo3yr2"].ToString();
                txtCombo3yr3.Text = dtRates.Rows[0]["Combo3yr3"].ToString();
                txtCombo4yr1.Text = dtRates.Rows[0]["Combo4yr1"].ToString();
                txtCombo4yr2.Text = dtRates.Rows[0]["Combo4yr2"].ToString();
                txtCombo4yr3.Text = dtRates.Rows[0]["Combo4yr3"].ToString();
                txtCombo5yr1.Text = dtRates.Rows[0]["Combo5yr1"].ToString();
                txtCombo5yr2.Text = dtRates.Rows[0]["Combo5yr2"].ToString();
                txtCombo5yr3.Text = dtRates.Rows[0]["Combo5yr3"].ToString();
                txtCombo6yr1.Text = dtRates.Rows[0]["Combo6yr1"].ToString();
                txtCombo6yr2.Text = dtRates.Rows[0]["Combo6yr2"].ToString();
                txtCombo6yr3.Text = dtRates.Rows[0]["Combo6yr3"].ToString();
                txtCombo7yr1.Text = dtRates.Rows[0]["Combo7yr1"].ToString();
                txtCombo7yr2.Text = dtRates.Rows[0]["Combo7yr2"].ToString();
                txtCombo7yr3.Text = dtRates.Rows[0]["Combo7yr3"].ToString();
                txtCombo8yr1.Text = dtRates.Rows[0]["Combo8yr1"].ToString();
                txtCombo8yr2.Text = dtRates.Rows[0]["Combo8yr2"].ToString();
                txtCombo8yr3.Text = dtRates.Rows[0]["Combo8yr3"].ToString();
                txtCombo9yr1.Text = dtRates.Rows[0]["Combo9yr1"].ToString();
                txtCombo9yr2.Text = dtRates.Rows[0]["Combo9yr2"].ToString();
                txtCombo9yr3.Text = dtRates.Rows[0]["Combo9yr3"].ToString();
                txtCombo10yr1.Text = dtRates.Rows[0]["Combo10yr1"].ToString();
                txtCombo10yr2.Text = dtRates.Rows[0]["Combo10yr2"].ToString();
                txtCombo10yr3.Text = dtRates.Rows[0]["Combo10yr3"].ToString();

            }
        }

        private void AddDecimalValidation(TextBox tb)
        {
            tb.Attributes.Add("onkeypress", "return checkKeyPressForDecimal(this, event)");
            tb.Attributes.Add("onblur", "validateDecimal(this);");
            tb.Attributes.Add("decimalSeparator", ".");
            tb.Attributes.Add("groupSeparator", "");
            tb.Attributes.Add("decimalDigits", "2");
            tb.Attributes.Add("style", "text-align:right;padding-right:1px;");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string sqlquery = string.Empty;

            try
            {
                bool Exists = Convert.ToBoolean(DataFunctions.ExecuteScalar("misc", "if exists (select CustomerID from CANON_PAIDPUB_ComboDiscounts where customerID = " + Session["CustomerID"].ToString() + ")   select 1 else select 0"));

                if (!Exists)
                {
                    sqlquery = "INSERT INTO CANON_PAIDPUB_ComboDiscounts VALUES(" + Session["CustomerID"].ToString() + "," + ConvertNULLtoZero(txtCombo2yr1.Text) + "," + ConvertNULLtoZero(txtCombo2yr2.Text) + "," + ConvertNULLtoZero(txtCombo2yr3.Text) + "," + ConvertNULLtoZero(txtCombo3yr1.Text) + "," + ConvertNULLtoZero(txtCombo3yr2.Text) + "," + ConvertNULLtoZero(txtCombo3yr3.Text) + "," + ConvertNULLtoZero(txtCombo4yr1.Text) + "," + ConvertNULLtoZero(txtCombo4yr2.Text) + "," + ConvertNULLtoZero(txtCombo4yr3.Text) + "," + ConvertNULLtoZero(txtCombo5yr1.Text) + "," + ConvertNULLtoZero(txtCombo5yr2.Text) + "," + ConvertNULLtoZero(txtCombo5yr3.Text) + "," + ConvertNULLtoZero(txtCombo6yr1.Text) + "," + ConvertNULLtoZero(txtCombo6yr2.Text) + "," + ConvertNULLtoZero(txtCombo6yr3.Text) + "," + ConvertNULLtoZero(txtCombo7yr1.Text) + "," + ConvertNULLtoZero(txtCombo7yr2.Text) + "," + ConvertNULLtoZero(txtCombo7yr3.Text) + "," + ConvertNULLtoZero(txtCombo8yr1.Text) + "," + ConvertNULLtoZero(txtCombo8yr2.Text) + "," + ConvertNULLtoZero(txtCombo8yr3.Text) + "," + ConvertNULLtoZero(txtCombo9yr1.Text) + "," + ConvertNULLtoZero(txtCombo9yr2.Text) + "," + ConvertNULLtoZero(txtCombo9yr3.Text) + "," + ConvertNULLtoZero(txtCombo10yr1.Text) + "," + ConvertNULLtoZero(txtCombo10yr2.Text) + "," + ConvertNULLtoZero(txtCombo10yr3.Text) + ")";
                }
                else
                {
                    sqlquery = "UPDATE CANON_PAIDPUB_ComboDiscounts SET Combo2yr1 = " + ConvertNULLtoZero(txtCombo2yr1.Text) + ",Combo2yr2 = " + ConvertNULLtoZero(txtCombo2yr2.Text) + ", Combo2yr3 = " + ConvertNULLtoZero(txtCombo2yr3.Text) + ", Combo3yr1 = " + ConvertNULLtoZero(txtCombo3yr1.Text) + ", Combo3yr2 = " + ConvertNULLtoZero(txtCombo3yr2.Text) + ", Combo3yr3 = " + ConvertNULLtoZero(txtCombo3yr3.Text) + ", Combo4yr1 = " + ConvertNULLtoZero(txtCombo4yr1.Text) + ", Combo4yr2 = " + ConvertNULLtoZero(txtCombo4yr2.Text) + ", Combo4yr3 = " + ConvertNULLtoZero(txtCombo4yr3.Text) + ", Combo5yr1 = " + ConvertNULLtoZero(txtCombo5yr1.Text) + ", Combo5yr2 = " + ConvertNULLtoZero(txtCombo5yr2.Text) + ", Combo5yr3 = " + ConvertNULLtoZero(txtCombo5yr3.Text) + ", Combo6yr1 = " + ConvertNULLtoZero(txtCombo6yr1.Text) + ", Combo6yr2 = " + ConvertNULLtoZero(txtCombo6yr2.Text) + ", Combo6yr3 = " + ConvertNULLtoZero(txtCombo6yr3.Text) + ", Combo7yr1 = " + ConvertNULLtoZero(txtCombo7yr1.Text) + ", Combo7yr2 = " + ConvertNULLtoZero(txtCombo7yr2.Text) + ", Combo7yr3 = " + ConvertNULLtoZero(txtCombo7yr3.Text) + ", Combo8yr1 = " + ConvertNULLtoZero(txtCombo8yr1.Text) + ", Combo8yr2 = " + ConvertNULLtoZero(txtCombo8yr2.Text) + ", Combo8yr3 = " + ConvertNULLtoZero(txtCombo8yr3.Text) + ", Combo9yr1 = " + ConvertNULLtoZero(txtCombo9yr1.Text) + ", Combo9yr2 = " + ConvertNULLtoZero(txtCombo9yr2.Text) + ", Combo9yr3 = " + ConvertNULLtoZero(txtCombo9yr3.Text) + ", Combo10yr1 = " + ConvertNULLtoZero(txtCombo10yr1.Text) + ", Combo10yr2 = " + ConvertNULLtoZero(txtCombo10yr2.Text) + ", Combo10yr3 = " + ConvertNULLtoZero(txtCombo10yr3.Text) + " where CustomerID = " + Session["CustomerID"].ToString();
                }

                DataFunctions.Execute("misc", sqlquery);

                lblErrorMessage.Text = "Rates Updated.";
                lblErrorMessage.Visible = true;
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "ERROR : " + ex.Message;
                lblErrorMessage.Visible = true;
            }

        }

        private string ConvertNULLtoZero(string s)
        {
            if (s == string.Empty || s.Trim() == "")
                return "0";
            else
                return s;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/main/default.aspx");
        }

    }
}
