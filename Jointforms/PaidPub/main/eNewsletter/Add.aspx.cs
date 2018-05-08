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
    public partial class Add : System.Web.UI.Page
    {
        private int GroupID
        {
            get
            {
                try { return Convert.ToInt32(Request.QueryString["GroupID"]); }
                catch { return 0; }
            }
        }
    
        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Visible = false;

            if (!IsPostBack)
            {
                LoadFrequency();
                LoadCategory();
                if (GroupID > 0)
                    LoadNewsletter();
            }
        }

        private void LoadNewsletter()
        {
            DataTable dtNewsletter = DataFunctions.GetDataTable("select * from ecn_misc..CANON_PAIDPUB_eNewsLetters n join ecn5_communicator..groups g on n.groupID = g.groupID left outer join  ecn_misc..CANON_PAIDPUB_Frequency f on n.frequencyID = f.frequencyID where n.customerID = " + Session["CustomerID"].ToString() + " and n.groupID = " + GroupID, ConfigurationManager.ConnectionStrings["conn_misc"].ConnectionString);

            if (dtNewsletter.Rows.Count > 0)
            {
                txtName.Text = dtNewsletter.Rows[0]["GroupName"].ToString();
                txtDescription.Text = dtNewsletter.Rows[0]["GroupDescription"].ToString();
                try
                {
                    drpFrequency.ClearSelection();
                    drpFrequency.Items.FindByValue(dtNewsletter.Rows[0]["FrequencyID"].ToString()).Selected = true;
                }
                catch { }

                try
                {
                    drpCategory.ClearSelection();
                    drpCategory.Items.FindByValue(dtNewsletter.Rows[0]["CategoryID"].ToString()).Selected = true;
                }
                catch { }
            }
            else
            {
                lblErrorMessage.Text = "Newsletter not exists.";
                lblErrorMessage.Visible = true;
                btnSave.Visible = false;
            }
        }


        private void LoadFrequency()
        {
            drpFrequency.DataSource = DataFunctions.GetDataTable("select * from CANON_PAIDPUB_Frequency", ConfigurationManager.ConnectionStrings["conn_misc"].ConnectionString);
            drpFrequency.DataTextField = "Frequency";
            drpFrequency.DataValueField = "FrequencyID";
            drpFrequency.DataBind();
            drpFrequency.Items.Insert(0, new ListItem("----- Select Frequency -----", "0"));
        }

        private void LoadCategory()
        {
            drpCategory.DataSource = DataFunctions.GetDataTable("select * from CANON_PAIDPUB_enewsletter_category where CustomerID=" + Session["CustomerID"].ToString(), ConfigurationManager.ConnectionStrings["conn_misc"].ConnectionString);
            drpCategory.DataTextField = "Name";
            drpCategory.DataValueField = "CategoryID";
            drpCategory.DataBind();
            drpCategory.Items.Insert(0, new ListItem("----- Select Category -----", "0"));
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string sqlquery = string.Empty;
            bool Exists = false;
            int gID = 0;
            int DataFieldsetID = 0;
            try
            {
                Exists = Convert.ToBoolean(DataFunctions.ExecuteScalar("communicator", "if exists (select groupID from Groups where customerID = " + Session["CustomerID"].ToString() + " and GroupID <> " + GroupID + " and GroupName = '" + txtName.Text.Replace("'", "''")  + "')   select 1 else select 0"));

                if (!Exists)
                {
                    if (GroupID == 0)
                    {
                        sqlquery = " INSERT INTO Groups (GroupName, GroupDescription, CustomerID, FolderID, OwnerTypeCode, PublicFolder) VALUES ('" + txtName.Text.Replace("'", "''") + "','" + txtDescription.Text.Replace("'", "''") + "', " + Session["CustomerID"].ToString() + ", 0, 'customer' , 0);select @@IDENTITY ";
                        Response.Write(sqlquery);
                        gID = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", sqlquery));

                        sqlquery = " INSERT INTO DatafieldSets (GroupID, MultivaluedYN, Name) VALUES (" + gID + ",'Y','" + txtName.Text.Replace("'", "''") + "');select @@IDENTITY ";
                        DataFieldsetID = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", sqlquery));

                        sqlquery = "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES  ('startdate', 'startdate'," + gID.ToString() +  "," + DataFieldsetID  + ", 'N');";
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('enddate', 'enddate'," + gID.ToString() +  "," + DataFieldsetID  + ", 'N');";
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('amountpaid', 'amountpaid'," + gID.ToString() +  "," + DataFieldsetID  + ", 'N');";
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('earnedamount', 'earnedamount'," + gID.ToString() +  "," + DataFieldsetID  + ", 'N');";
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('Deferredamount', 'Deferredamount'," + gID.ToString() +  "," + DataFieldsetID  + ", 'N');";
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('TotalSent', 'TotalSent'," + gID.ToString() +  "," + DataFieldsetID  + ", 'N');";
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('PromoCode', 'PromoCode'," + gID.ToString() + "," + DataFieldsetID + ", 'N');";
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('SubType', 'SubType'," + gID.ToString() + "," + DataFieldsetID + ", 'N');";
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('TransactionID', 'TransactionID'," + gID.ToString() + "," + DataFieldsetID + ", 'N');";
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('PaymentMethod', 'PaymentMethod'," + gID.ToString() + "," + DataFieldsetID + ", 'N');";
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('CardType', 'CardType'," + gID.ToString() + "," + DataFieldsetID + ", 'N');";
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('CardNumber', 'CardNumber'," + gID.ToString() + "," + DataFieldsetID + ", 'N');";
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('Business', 'Business'," + gID.ToString() + ",NULL, 'N');";
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('Responsibility', 'Responsibility'," + gID.ToString() + ",NULL, 'N');";
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('PaidOrFree', 'PaidOrFree'," + gID.ToString() + ",NULL, 'N');"; 
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('SubDate', 'SubDate'," + gID.ToString() + ",NULL, 'N');"; 
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('Original_Effort_Code', 'Original_Effort_Code'," + gID.ToString() + ",NULL, 'N');"; 
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('Effort_Code', 'Effort_Code'," + gID.ToString() + ",NULL, 'N');"; 
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('Sub_Account_Number', 'Sub_Account_Number'," + gID.ToString() + ",NULL, 'N');";
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('Verification_Date', 'Verification_Date'," + gID.ToString() + ",NULL, 'N');"; 

                        DataFunctions.Execute("communicator", sqlquery);

                        sqlquery = " INSERT INTO DatafieldSets (GroupID, MultivaluedYN, Name) VALUES (" + gID + ",'Y','" + (txtName.Text.Length > 50 ? txtName.Text.Replace("'", "''").Substring(1,40) : txtName.Text.Replace("'", "''")) + " - Adj');select @@IDENTITY ";
                        DataFieldsetID = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", sqlquery));

                        sqlquery = "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES  ('TransEntryID', 'TransEntryID'," + gID.ToString() + "," + DataFieldsetID + ", 'N');";
                        sqlquery = "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES  ('AdjDate', 'AdjDate'," + gID.ToString() + "," + DataFieldsetID + ", 'N');";
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('AdjType', 'AdjType'," + gID.ToString() + "," + DataFieldsetID + ", 'N');";
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('AdjAmount', 'AdjAmount'," + gID.ToString() + "," + DataFieldsetID + ", 'N');";
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('AdjExpDate', 'AdjExpDate'," + gID.ToString() + "," + DataFieldsetID + ", 'N');";
                        sqlquery += "INSERT INTO GroupDatafields (ShortName,LongName,GroupID,DataFieldsetID,IsPublic) VALUES ('AdjDesc', 'AdjDesc'," + gID.ToString() + "," + DataFieldsetID + ", 'N');";

                        sqlquery = " INSERT INTO CANON_PAIDPUB_eNewsletters (GroupID, FrequencyID, CustomerID, CategoryID) VALUES (" + gID.ToString() + "," + (drpFrequency.SelectedItem.Value == string.Empty ? "NULL" : drpFrequency.SelectedItem.Value) + "," + Session["CustomerID"].ToString() + "," + (drpCategory.SelectedItem.Value == "0" ? "NULL" : drpCategory.SelectedItem.Value) + ")";
                        DataFunctions.Execute("misc", sqlquery);
                    }
                    else
                    {
                        sqlquery =  " update Groups set GroupName = '" + txtName.Text.Replace("'", "''") + "', GroupDescription='" + txtDescription.Text.Replace("'", "''") + "' where GroupID = " + GroupID.ToString();
                        DataFunctions.Execute("communicator", sqlquery);

                        sqlquery = " update CANON_PAIDPUB_eNewsletters set CategoryID = " + (drpCategory.SelectedItem.Value == "0" ? "NULL" : drpCategory.SelectedItem.Value)  + ", FrequencyID= " + (drpFrequency.SelectedItem.Value == string.Empty ? "NULL" : drpFrequency.SelectedItem.Value) + " where GroupID = " + GroupID.ToString();
                        DataFunctions.Execute("misc", sqlquery);
                    }
                    Response.Redirect("default.aspx");
                }
                else
                {
                    lblErrorMessage.Text = "ERROR : Group by name <i>\"" + txtName.Text + "\"</i> already exists. Please use a different Name";
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
