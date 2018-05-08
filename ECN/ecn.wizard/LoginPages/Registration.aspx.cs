using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using ecn.wizard.wzAccountCreation;
using ecn.wizard.wzPushLeadsList;
using ecn.wizard.wzProcessLogin;

namespace ecn.wizard.LoginPages
{
	/// <summary>
	/// Summary description for Registration.
	/// </summary>
	public partial class Registration : ecn.wizard.MasterPage
	{
		protected System.Web.UI.WebControls.Label MsgLabel;
	
		int baseChannelID = 0;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			lblError.Visible= false;

			if (!IsPostBack)
			{
				if (Request["cID"] != null)
				{
					if (Request["cID"] != string.Empty)
					{
						baseChannelID = Convert.ToInt32(Request["cID"]);
					}
				}
			}
			else
				baseChannelID = Convert.ToInt32(lblChannelID.Text);

			if (baseChannelID == 0)
			{
				lblError.Text="Channel ID is required";
				lblError.Visible = true;
			}
			else
				lblChannelID.Text = baseChannelID.ToString();

		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void SaveButton_Click(object sender, System.EventArgs e)
		{
			int newCustomerID = -1;
			int newUserID = -1;
			if (Page.IsValid)
			{
				try
				{
					//Set Contact Information save it as Contact Object
					//Contact newContactInfo = ac.setCustomerContactInformation("ashok","palani","test Add", "Edina", "MN","USA","55435","111-111-1111","222-222-2222","ashanaa@hotmail.com","qrertvtts");
					Contact newContactInfo= new Contact();
					newContactInfo.ContactName = txtFirstName.Text + " " + txtLastName.Text ;
					newContactInfo.FirstName = txtFirstName.Text ;
					newContactInfo.LastName = txtLastName.Text  ;
					newContactInfo.ContactTitle = "";	//This is the UNIQUE ID for the Customer. 
					newContactInfo.StreetAddress = txtaddress.Text;
					newContactInfo.City = txtcity.Text;
					newContactInfo.State = ddlstate.SelectedValue;
					newContactInfo.Country = txtcountry.Text;
					newContactInfo.Zip = txtzip.Text;
					newContactInfo.Phone = txtphone.Text; 
					newContactInfo.Fax = txtfax.Text;
					newContactInfo.Email = txtEmailAddress.Text;
			

					AccountCreation ac = new AccountCreation();

					//Create Customer account
					newCustomerID = ac.setupCustomerAccount(baseChannelID, newContactInfo, txtCustomerName.Text);
			
					if(newCustomerID > 0)
					{
						//Create User Account
						newUserID = ac.setupUserAccount(txtEmailAddress.Text, txtPassword.Text, newCustomerID);
					}
			
					//Get Access Key for the User & Customer
					string accessKey = ""; 
					ProcessLogin login = new ProcessLogin();
						
					accessKey = login.setupLogin(newCustomerID,newUserID);

					//get the AutoLoginURL & pass the accessKey as a QueryStrig Param with the URL
					if(accessKey.Length > 0)
					{
						Response.Redirect(login.getAutoLoginURL()+accessKey,true);
					}
				}
				catch (System.Web.Services.Protocols.SoapException sEx)
				{
					lblError.Visible= true;
					lblError.Text = sEx.Message;
				}
				catch (Exception ex)
				{
					lblError.Visible= true;
					lblError.Text = ex.Message;
				}
			}
		}

		protected void btnCancel_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("wizchannel_" + baseChannelID + "_login.aspx",true);
		}
	}
}
